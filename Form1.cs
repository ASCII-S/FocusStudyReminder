using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FocusStudyReminder
{
    public partial class Form1 : Form
    {
        private TimerManager _timerManager;
        private SettingsManager _settingsManager;
        private NotifyIcon _notifyIcon;
        private bool _isClosing = false;

        public Form1()
        {
            InitializeComponent();
            
            // 初始化管理器
            _timerManager = TimerManager.Instance;
            _settingsManager = SettingsManager.Instance;
            
            // 注册事件
            _timerManager.StateChanged += TimerManager_StateChanged;
            _timerManager.MainTimerTick += TimerManager_MainTimerTick;
            _timerManager.SubTimerTick += TimerManager_SubTimerTick;
            _timerManager.MeditationStarted += TimerManager_MeditationStarted;
            _timerManager.MeditationEnded += TimerManager_MeditationEnded;
            _timerManager.StudySessionStarted += TimerManager_StudySessionStarted;
            _timerManager.RestSessionStarted += TimerManager_RestSessionStarted;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 初始化系统托盘图标
            InitializeNotifyIcon();
            
            // 初始化界面
            UpdateUI();
            
            // 添加窗体大小改变事件处理
            this.SizeChanged += Form1_SizeChanged;
        }
        
        // 初始化系统托盘图标
        private void InitializeNotifyIcon()
        {
            _notifyIcon = new NotifyIcon();
            _notifyIcon.Icon = this.Icon;
            _notifyIcon.Text = "专注学习提醒器";
            _notifyIcon.Visible = true;
            
            // 创建右键菜单
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            
            ToolStripMenuItem openItem = new ToolStripMenuItem("打开主界面");
            openItem.Click += (s, args) => { Show(); WindowState = FormWindowState.Normal; };
            
            ToolStripMenuItem startItem = new ToolStripMenuItem("开始学习");
            startItem.Click += btnStart_Click;
            
            ToolStripMenuItem stopItem = new ToolStripMenuItem("停止");
            stopItem.Click += btnStop_Click;
            
            ToolStripMenuItem exitItem = new ToolStripMenuItem("退出");
            exitItem.Click += (s, args) => { _isClosing = true; Application.Exit(); };
            
            contextMenu.Items.Add(openItem);
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add(startItem);
            contextMenu.Items.Add(stopItem);
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add(exitItem);
            
            _notifyIcon.ContextMenuStrip = contextMenu;
            
            // 双击打开主界面
            _notifyIcon.DoubleClick += (s, args) => { Show(); WindowState = FormWindowState.Normal; };
        }
        
        // 更新UI
        private void UpdateUI()
        {
            // 更新状态
            switch (_timerManager.CurrentState)
            {
                case TimerState.Stopped:
                    lblStatus.Text = "已停止";
                    lblStatus.ForeColor = Color.Gray;
                    btnStart.Enabled = true;
                    btnStop.Enabled = false;
                    lblMainTimer.Text = "00:00";
                    lblSubTimer.Text = "00:00";
                    progressBarMain.Value = 0;
                    progressBarSub.Value = 0;
                    break;
                    
                case TimerState.Study:
                    lblStatus.Text = "学习中";
                    lblStatus.ForeColor = Color.Green;
                    btnStart.Enabled = false;
                    btnStop.Enabled = true;
                    break;
                    
                case TimerState.Rest:
                    lblStatus.Text = "休息中";
                    lblStatus.ForeColor = Color.Blue;
                    btnStart.Enabled = false;
                    btnStop.Enabled = true;
                    break;
                    
                case TimerState.Meditation:
                    lblStatus.Text = "冥想中";
                    lblStatus.ForeColor = Color.Purple;
                    btnStart.Enabled = false;
                    btnStop.Enabled = true;
                    break;
            }
        }

        #region 事件处理
        
        // 计时器状态变化事件
        private void TimerManager_StateChanged(object sender, TimerState state)
        {
            // 在UI线程中更新界面
            if (InvokeRequired)
            {
                Invoke(new EventHandler<TimerState>(TimerManager_StateChanged), sender, state);
                return;
            }
            
            UpdateUI();
        }
        
        // 主计时器Tick事件
        private void TimerManager_MainTimerTick(object sender, int remainingSeconds)
        {
            if (InvokeRequired)
            {
                Invoke(new EventHandler<int>(TimerManager_MainTimerTick), sender, remainingSeconds);
                return;
            }
            
            // 更新主计时器显示
            lblMainTimer.Text = TimerManager.FormatTime(remainingSeconds);
            
            // 更新进度条
            int totalSeconds = 0;
            switch (_timerManager.CurrentState)
            {
                case TimerState.Study:
                    totalSeconds = _settingsManager.StudyMinutes * 60;
                    break;
                case TimerState.Rest:
                    totalSeconds = _settingsManager.RestMinutes * 60;
                    break;
            }
            
            if (totalSeconds > 0)
            {
                int progress = (int)(100 - ((double)remainingSeconds / totalSeconds * 100));
                progressBarMain.Value = Math.Min(progress, 100);
            }
            
            // 更新系统托盘提示
            UpdateNotifyIconText();
        }
        
        // 子计时器Tick事件
        private void TimerManager_SubTimerTick(object sender, int remainingSeconds)
        {
            if (InvokeRequired)
            {
                Invoke(new EventHandler<int>(TimerManager_SubTimerTick), sender, remainingSeconds);
                return;
            }
            
            // 更新子计时器显示
            lblSubTimer.Text = TimerManager.FormatTime(remainingSeconds);
            
            // 更新进度条
            int totalSeconds = 0;
            if (_timerManager.CurrentState == TimerState.Meditation)
            {
                totalSeconds = _settingsManager.MeditationSeconds;
            }
            else if (_timerManager.CurrentState == TimerState.Study)
            {
                // 子计时器为随机时间，不应该使用剩余时间作为总时间
                // 应该获取初始设置的随机时间
                int minMinutes = _settingsManager.MinRandomMinutes;
                int maxMinutes = _settingsManager.MaxRandomMinutes;
                int initialSeconds = _timerManager.GetSubInitialSeconds();
                totalSeconds = initialSeconds > 0 ? initialSeconds : remainingSeconds + 1; // 防止除零错误
            }
            
            if (totalSeconds > 0)
            {
                int progress = (int)(100 - ((double)remainingSeconds / totalSeconds * 100));
                progressBarSub.Value = Math.Min(progress, 100);
            }
        }
        
        // 冥想开始事件
        private void TimerManager_MeditationStarted(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new EventHandler(TimerManager_MeditationStarted), sender, e);
                return;
            }
            
            // 如果设置了显示弹窗，则显示冥想提示
            if (_settingsManager.ShowPopup)
            {
                ShowBalloonTip("冥想提醒", $"请闭上眼睛休息 {_settingsManager.MeditationSeconds} 秒", ToolTipIcon.Info);
            }
        }
        
        // 冥想结束事件
        private void TimerManager_MeditationEnded(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new EventHandler(TimerManager_MeditationEnded), sender, e);
                return;
            }
            
            // 如果设置了显示弹窗，则显示冥想结束提示
            if (_settingsManager.ShowPopup)
            {
                ShowBalloonTip("冥想结束", "冥想时间已结束，继续专注学习", ToolTipIcon.Info);
            }
        }
        
        // 学习会话开始事件
        private void TimerManager_StudySessionStarted(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new EventHandler(TimerManager_StudySessionStarted), sender, e);
                return;
            }
            
            // 如果设置了显示弹窗，则显示学习开始提示
            if (_settingsManager.ShowPopup)
            {
                ShowBalloonTip("学习时间", $"开始学习 {_settingsManager.StudyMinutes} 分钟", ToolTipIcon.Info);
            }
        }
        
        // 休息会话开始事件
        private void TimerManager_RestSessionStarted(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new EventHandler(TimerManager_RestSessionStarted), sender, e);
                return;
            }
            
            // 如果设置了显示弹窗，则显示休息开始提示
            if (_settingsManager.ShowPopup)
            {
                ShowBalloonTip("休息时间", $"开始休息 {_settingsManager.RestMinutes} 分钟", ToolTipIcon.Info);
            }
        }
        
        #endregion
        
        #region 辅助方法
        
        // 显示托盘气泡提示
        private void ShowBalloonTip(string title, string text, ToolTipIcon icon)
        {
            _notifyIcon.BalloonTipTitle = title;
            _notifyIcon.BalloonTipText = text;
            _notifyIcon.BalloonTipIcon = icon;
            _notifyIcon.ShowBalloonTip(5000); // 显示5秒
        }
        
        // 更新托盘图标提示文字
        private void UpdateNotifyIconText()
        {
            string status = "";
            switch (_timerManager.CurrentState)
            {
                case TimerState.Stopped:
                    status = "已停止";
                    break;
                case TimerState.Study:
                    status = "学习中";
                    break;
                case TimerState.Rest:
                    status = "休息中";
                    break;
                case TimerState.Meditation:
                    status = "冥想中";
                    break;
            }
            
            string mainTime = lblMainTimer.Text;
            _notifyIcon.Text = $"专注学习提醒器 - {status}\n剩余时间: {mainTime}";
        }
        
        #endregion
        
        #region 控件事件处理
        
        // 开始按钮点击事件
        private void btnStart_Click(object sender, EventArgs e)
        {
            _timerManager.StartStudySession();
        }
        
        // 停止按钮点击事件
        private void btnStop_Click(object sender, EventArgs e)
        {
            _timerManager.Stop();
        }
        
        // 设置按钮点击事件
        private void btnSettings_Click(object sender, EventArgs e)
        {
            // 暂停计时器
            bool wasRunning = _timerManager.CurrentState != TimerState.Stopped;
            if (wasRunning)
            {
                _timerManager.Pause();
            }
            
            // 显示设置窗体
            using (SettingsForm settingsForm = new SettingsForm())
            {
                DialogResult result = settingsForm.ShowDialog();
                
                // 无论用户是保存(OK)还是取消，只要之前在运行，就恢复计时器
                if (wasRunning)
                {
                    // 如果保存了新设置，先更新UI
                    if (result == DialogResult.OK)
                    {
                        UpdateUI();
                    }
                    // 恢复计时器运行
                    _timerManager.Resume();
                }
            }
        }
        
        // 窗体大小改变事件
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            // 处理最小化事件
            if (this.WindowState == FormWindowState.Minimized)
            {
                // 如果设置为静默最小化，则不显示提示
                if (!_settingsManager.SilentMinimize)
                {
                    // 主动最小化时显示提示
                    ShowBalloonTip("提示", "应用程序已最小化，将在后台继续运行", ToolTipIcon.Info);
                }
                
                // 如果设置为最小化到托盘，则隐藏窗口
                if (_settingsManager.DefaultCloseAction == CloseAction.MinimizeToTray)
                {
                    this.Hide();
                }
            }
        }
        
        // 窗体关闭事件
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_isClosing && e.CloseReason == CloseReason.UserClosing)
            {
                // 根据设置选择关闭行为
                switch (_settingsManager.DefaultCloseAction)
                {
                    case CloseAction.Exit:
                        _isClosing = true;
                        break;
                        
                    case CloseAction.Minimize:
                        e.Cancel = true;
                        this.WindowState = FormWindowState.Minimized;
                        break;
                        
                    case CloseAction.MinimizeToTray:
                        e.Cancel = true;
                        this.Hide();
                        if (!_settingsManager.SilentMinimize)
                        {
                            ShowBalloonTip("提示", "应用程序将在后台运行，双击托盘图标可以重新打开界面", ToolTipIcon.Info);
                        }
                        break;
                        
                    case CloseAction.AskEveryTime:
                        e.Cancel = true;
                        using (var confirmDialog = new Form())
                        {
                            confirmDialog.Text = "关闭选项";
                            confirmDialog.StartPosition = FormStartPosition.CenterParent;
                            confirmDialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                            confirmDialog.MaximizeBox = false;
                            confirmDialog.MinimizeBox = false;
                            confirmDialog.Size = new System.Drawing.Size(300, 180);
                            confirmDialog.BackColor = System.Drawing.Color.White;
                            confirmDialog.Font = new System.Drawing.Font("Segoe UI", 9F);
                            
                            var lblMessage = new Label();
                            lblMessage.Text = "请选择关闭操作：";
                            lblMessage.Location = new System.Drawing.Point(20, 20);
                            lblMessage.AutoSize = true;
                            
                            var btnExit = new Button();
                            btnExit.Text = "退出应用";
                            btnExit.Size = new System.Drawing.Size(120, 30);
                            btnExit.Location = new System.Drawing.Point(20, 55);
                            btnExit.Click += (s, args) => {
                                _isClosing = true;
                                confirmDialog.DialogResult = DialogResult.OK;
                                confirmDialog.Close();
                            };
                            
                            var btnMinimize = new Button();
                            btnMinimize.Text = "最小化到任务栏";
                            btnMinimize.Size = new System.Drawing.Size(120, 30);
                            btnMinimize.Location = new System.Drawing.Point(150, 55);
                            btnMinimize.Click += (s, args) => {
                                this.WindowState = FormWindowState.Minimized;
                                confirmDialog.DialogResult = DialogResult.Cancel;
                                confirmDialog.Close();
                            };
                            
                            var btnMinimizeToTray = new Button();
                            btnMinimizeToTray.Text = "最小化到系统托盘";
                            btnMinimizeToTray.Size = new System.Drawing.Size(120, 30);
                            btnMinimizeToTray.Location = new System.Drawing.Point(20, 95);
                            btnMinimizeToTray.Click += (s, args) => {
                                this.Hide();
                                confirmDialog.DialogResult = DialogResult.Cancel;
                                confirmDialog.Close();
                                if (!_settingsManager.SilentMinimize)
                                {
                                    ShowBalloonTip("提示", "应用程序将在后台运行，双击托盘图标可以重新打开界面", ToolTipIcon.Info);
                                }
                            };
                            
                            var chkRemember = new CheckBox();
                            chkRemember.Text = "记住我的选择";
                            chkRemember.Location = new System.Drawing.Point(150, 100);
                            chkRemember.AutoSize = true;
                            
                            confirmDialog.Controls.Add(lblMessage);
                            confirmDialog.Controls.Add(btnExit);
                            confirmDialog.Controls.Add(btnMinimize);
                            confirmDialog.Controls.Add(btnMinimizeToTray);
                            confirmDialog.Controls.Add(chkRemember);
                            
                            confirmDialog.ShowDialog(this);
                            
                            // 如果勾选了"记住我的选择"，则保存设置
                            if (chkRemember.Checked)
                            {
                                if (confirmDialog.DialogResult == DialogResult.OK)
                                {
                                    _settingsManager.DefaultCloseAction = CloseAction.Exit;
                                    _settingsManager.SaveSettings();
                                }
                                else if (confirmDialog.DialogResult == DialogResult.Cancel)
                                {
                                    // 根据当前窗口状态判断操作类型
                                    if (this.Visible)
                                    {
                                        _settingsManager.DefaultCloseAction = CloseAction.Minimize;
                                    }
                                    else
                                    {
                                        _settingsManager.DefaultCloseAction = CloseAction.MinimizeToTray;
                                    }
                                    _settingsManager.SaveSettings();
                                }
                            }
                        }
                        break;
                }
            }
            else
            {
                // 实际关闭程序时做清理工作
                if (_notifyIcon != null)
                {
                    _notifyIcon.Dispose();
                }
            }
        }
        #endregion

        private void lblMainTimer_Click(object sender, EventArgs e)
        {

        }
    }
}
