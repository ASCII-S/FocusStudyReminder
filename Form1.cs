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
        private bool _isPaused = false;
        
        // 添加专注模式变量
        private bool _focusMode = false;
        private Button btnToggleFocus;
        private ToolTip toolTip;
        
        // 添加页面导航相关变量
        private Panel mainContentPanel;
        private Panel settingsContentPanel;
        private Button activeNavButton;
        private Panel navPanel;
        
        // 设置相关控件
        private NumericUpDown nudStudyMinutes;
        private NumericUpDown nudRestMinutes;
        private NumericUpDown nudMeditationSeconds;
        private NumericUpDown nudMinRandomMinutes;
        private NumericUpDown nudMaxRandomMinutes;
        private ComboBox cboSoundFile;
        private CheckBox chkShowPopup;
        private RadioButton radExit;
        private RadioButton radMinimize;
        private RadioButton radMinimizeToTray;
        private RadioButton radAskEveryTime;
        private CheckBox chkSilentMinimize;
        private Button btnBrowseSoundFile;
        private Button btnTestSound;
        private Button btnDefaultSound;
        
        // 设置标签页控件
        private TabControl settingsTabControl;
        private TabPage tabGeneral;
        private TabPage tabNotification;
        private TabPage tabWindow;
        private Button btnSaveSettings;

        // 添加默认音效列表
        private readonly string[] DefaultSounds = new string[]
        {
            "default.wav",
            "bell.wav",
            "chime.wav",
            "notification.wav",
            "ding.wav"
        };
        
        // 用户自定义音效历史记录
        private List<string> customSounds;

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
            
            // 初始化导航UI
            InitializeNavigationUI();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 初始化系统托盘图标
            InitializeNotifyIcon();
            
            // 初始化界面
            UpdateUI();
            
            // 加载设置
            LoadSettings();

            // 修改窗体最大化和大小调整行为
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            
            // 恢复保存的窗口大小
            if (_settingsManager.WindowWidth > 0 && _settingsManager.WindowHeight > 0)
            {
                // 确保窗口不会超出屏幕
                int maxWidth = Screen.PrimaryScreen.WorkingArea.Width;
                int maxHeight = Screen.PrimaryScreen.WorkingArea.Height;
                int width = Math.Min(_settingsManager.WindowWidth, maxWidth);
                int height = Math.Min(_settingsManager.WindowHeight, maxHeight);
                
                this.Size = new Size(width, height);
                
                // 将窗口居中显示
                this.StartPosition = FormStartPosition.CenterScreen;
            }
            
            // 添加窗体大小改变事件处理
            this.SizeChanged += Form1_SizeChanged;
            this.Resize += Form1_Resize;
            
            // 默认显示主页面
            ShowMainPanel();
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
                    btnStart.Visible = true;
                    btnStart.Enabled = true;
                    btnPause.Visible = false;
                    btnStop.Enabled = false;
                    lblMainTimer.Text = "00:00";
                    lblSubTimer.Text = "00:00";
                    progressBarMain.Value = 0;
                    progressBarSub.Value = 0;
                    _isPaused = false;
                    break;
                    
                case TimerState.Study:
                    lblStatus.Text = _isPaused ? "已暂停" : "学习中";
                    lblStatus.ForeColor = _isPaused ? Color.Orange : Color.Green;
                    btnStart.Visible = false;
                    btnPause.Visible = true;
                    btnPause.Enabled = true;
                    btnPause.Text = _isPaused ? "继续" : "暂停";
                    btnStop.Enabled = true;
                    break;
                    
                case TimerState.Rest:
                    lblStatus.Text = _isPaused ? "已暂停" : "休息中";
                    lblStatus.ForeColor = _isPaused ? Color.Orange : Color.Blue;
                    btnStart.Visible = false;
                    btnPause.Visible = true;
                    btnPause.Enabled = true;
                    btnPause.Text = _isPaused ? "继续" : "暂停";
                    btnStop.Enabled = true;
                    break;
                    
                case TimerState.Meditation:
                    lblStatus.Text = _isPaused ? "已暂停" : "冥想中";
                    lblStatus.ForeColor = _isPaused ? Color.Orange : Color.Purple;
                    btnStart.Visible = false;
                    btnPause.Visible = true;
                    btnPause.Enabled = true;
                    btnPause.Text = _isPaused ? "继续" : "暂停";
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
        
        // 暂停按钮点击事件
        private void btnPause_Click(object sender, EventArgs e)
        {
            if (_isPaused)
            {
                // 如果已暂停，则继续
                _timerManager.Resume();
                _isPaused = false;
                btnPause.Text = "暂停";
            }
            else
            {
                // 如果未暂停，则暂停
                _timerManager.Pause();
                _isPaused = true;
                btnPause.Text = "继续";
            }
            
            // 更新UI状态
            UpdateUI();
        }
        
        // 停止按钮点击事件
        private void btnStop_Click(object sender, EventArgs e)
        {
            _timerManager.Stop();
        }
        
        // 窗体大小改变事件
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                if (_settingsManager.SilentMinimize)
                {
                    Hide();
                    _notifyIcon.ShowBalloonTip(1000, "专注学习提醒器", "程序已最小化到系统托盘运行", ToolTipIcon.Info);
                }
            }
        }
        
        // 窗体大小调整事件
        private void Form1_Resize(object sender, EventArgs e)
        {
            // 仅在窗口处于正常状态时保存大小
            if (WindowState == FormWindowState.Normal)
            {
                _settingsManager.WindowWidth = this.Width;
                _settingsManager.WindowHeight = this.Height;
                _settingsManager.SaveSettings();
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
        
        #region 导航与页面UI
        
        // 初始化导航UI
        private void InitializeNavigationUI()
        {
            // 设置窗体大小和属性
            this.ClientSize = new Size(800, 500);
            this.MinimumSize = new Size(650, 400);
            
            // 初始化工具提示
            toolTip = new ToolTip();
            toolTip.AutoPopDelay = 5000;
            toolTip.InitialDelay = 500;
            toolTip.ReshowDelay = 200;
            toolTip.ShowAlways = true;
            
            // 创建内容面板
            Panel contentPanel = new Panel
            {
                Dock = DockStyle.Fill
            };
            this.Controls.Add(contentPanel);

            // 创建导航面板
            navPanel = new Panel
            {
                BackColor = Color.FromArgb(240, 240, 240),
                Dock = DockStyle.Left,
                Width = 180
            };
            this.Controls.Add(navPanel);
            // 创建主页和设置页
            mainContentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };
            settingsContentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Visible = false
            };
            
            contentPanel.Controls.Add(settingsContentPanel);
            contentPanel.Controls.Add(mainContentPanel);
            
            // 初始创建专注模式按钮
            CreateFocusModeButton();
            
            // 创建导航按钮
            Button btnNavHome = new Button
            {
                Text = "主页",
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                Size = new Size(180, 45),
                Font = new Font("微软雅黑", 10.5f),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0),
                Image = null, // 如果有图标，可以在这里设置
                ImageAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Top
            };
            btnNavHome.Click += BtnNavHome_Click;
            
            Button btnNavSettings = new Button
            {
                Text = "设置",
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                Size = new Size(180, 45),
                Font = new Font("微软雅黑", 10.5f),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0),
                Image = null, // 如果有图标，可以在这里设置
                ImageAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Top
            };
            btnNavSettings.Click += BtnNavSettings_Click;
            
            // 添加导航标题
            Label lblNavTitle = new Label
            {
                Text = "专注学习提醒器",
                Dock = DockStyle.Top,
                Height = 60,
                Font = new Font("微软雅黑", 12f, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };
            
            // 添加控件到导航面板
            navPanel.Controls.Add(btnNavSettings);
            navPanel.Controls.Add(btnNavHome);
            navPanel.Controls.Add(lblNavTitle);
            
            // 初始化设置面板
            CreateSettingsContent();
            
            // 将主窗体控件移动到mainContentPanel
            MoveControlsToMainPanel();
            
            // 激活主页按钮
            ActiveNavButton(btnNavHome);
        }
        
        private void MoveControlsToMainPanel()
        {
            // 获取所有需要移动的控件
            List<Control> controlsToMove = new List<Control>();
            foreach (Control control in this.Controls)
            {
                if (control != navPanel && 
                    control != mainContentPanel && 
                    control != settingsContentPanel &&
                    !(control is Panel && control.Controls.Contains(mainContentPanel)))
                {
                    controlsToMove.Add(control);
                }
            }
            
            // 移动控件到主内容面板
            foreach (Control control in controlsToMove)
            {
                this.Controls.Remove(control);
                mainContentPanel.Controls.Add(control);
            }
            
            // 创建布局面板用于自动调整布局
            TableLayoutPanel layoutPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 6,
                ColumnStyles = {
                    new ColumnStyle(SizeType.Percent, 20),
                    new ColumnStyle(SizeType.Percent, 60),
                    new ColumnStyle(SizeType.Percent, 20)
                },
                RowStyles = {
                    new RowStyle(SizeType.Percent, 10),
                    new RowStyle(SizeType.Percent, 30),  // 主计时器行 - 增加比例
                    new RowStyle(SizeType.Percent, 5),
                    new RowStyle(SizeType.Percent, 20),  // 子计时器行 - 减小比例
                    new RowStyle(SizeType.Percent, 10),
                    new RowStyle(SizeType.Percent, 25)
                },
                Padding = new Padding(15),
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None
            };
            
            mainContentPanel.Controls.Add(layoutPanel);
            
            // 临时保存需要保留的控件
            List<Control> controlsToKeep = new List<Control>();
            
            // 设置所有控件的Dock属性为Fill
            foreach (Control control in mainContentPanel.Controls)
            {
                if (control != layoutPanel && control != btnToggleFocus)
                {
                    controlsToKeep.Add(control);
                }
            }
            
            // 移除不需要的控件
            foreach (Control control in controlsToKeep)
            {
                mainContentPanel.Controls.Remove(control);
            }
            
            // 确保专注模式按钮显示在最上层
            if (mainContentPanel.Controls.Contains(btnToggleFocus))
            {
                mainContentPanel.Controls.SetChildIndex(btnToggleFocus, 0);
            }
            else
            {
                // 如果按钮不在控件集合中，重新添加
                CreateFocusModeButton();
            }
            
            // 创建标签组
            Panel pnlMainTimer = new Panel
            {
                Dock = DockStyle.Fill
            };
            
            Panel pnlSubTimer = new Panel
            {
                Dock = DockStyle.Fill
            };
            
            // 设置主计时器控件
            lblMainLabel.Dock = DockStyle.Top;
            lblMainLabel.TextAlign = ContentAlignment.MiddleCenter;
            lblMainLabel.Font = new Font("微软雅黑", 11, FontStyle.Bold);
            lblMainLabel.Height = 25;
            
            lblMainTimer.Dock = DockStyle.Fill;
            lblMainTimer.TextAlign = ContentAlignment.MiddleCenter;
            lblMainTimer.Font = new Font("微软雅黑", 48, FontStyle.Bold);
            
            progressBarMain.Dock = DockStyle.Bottom;
            progressBarMain.Height = 24;
            
            // 设置子计时器控件
            lblSubLabel.Dock = DockStyle.Top;
            lblSubLabel.TextAlign = ContentAlignment.MiddleCenter;
            lblSubLabel.Font = new Font("微软雅黑", 9);
            lblSubLabel.Height = 20;
            
            lblSubTimer.Dock = DockStyle.Fill;
            lblSubTimer.TextAlign = ContentAlignment.MiddleCenter;
            lblSubTimer.Font = new Font("微软雅黑", 32, FontStyle.Bold);
            
            progressBarSub.Dock = DockStyle.Bottom;
            progressBarSub.Height = 16;
            
            // 状态标签
            lblStatus.Dock = DockStyle.Fill;
            lblStatus.TextAlign = ContentAlignment.MiddleCenter;
            lblStatus.Font = new Font("微软雅黑", 24, FontStyle.Bold);
            
            // 添加控件到面板
            pnlMainTimer.Controls.Add(lblMainTimer);
            pnlMainTimer.Controls.Add(lblMainLabel);
            pnlMainTimer.Controls.Add(progressBarMain);
            
            pnlSubTimer.Controls.Add(lblSubTimer);
            pnlSubTimer.Controls.Add(lblSubLabel);
            pnlSubTimer.Controls.Add(progressBarSub);
            
            // 添加控件到布局面板
            layoutPanel.Controls.Add(pnlMainTimer, 1, 1);
            layoutPanel.Controls.Add(pnlSubTimer, 1, 3);
            layoutPanel.Controls.Add(lblStatus, 1, 4);
            
            // 创建按钮面板
            Panel buttonPanel = new Panel
            {
                Dock = DockStyle.Fill
            };
            
            buttonPanel.Controls.Add(btnStart);
            buttonPanel.Controls.Add(btnPause);
            buttonPanel.Controls.Add(btnStop);
            
            // 设置按钮属性
            int btnWidth = 120;
            int btnHeight = 40;
            btnStart.Size = new Size(btnWidth, btnHeight);
            btnPause.Size = new Size(btnWidth, btnHeight);
            btnStop.Size = new Size(btnWidth, btnHeight);
            
            // 动态调整按钮位置
            buttonPanel.Resize += (s, e) => 
            {
                // 开始和暂停按钮共用同一位置，按钮间增加更多间距
                btnStart.Location = new Point(buttonPanel.Width / 4 - btnWidth / 2, buttonPanel.Height / 2 - btnHeight / 2);
                btnPause.Location = new Point(buttonPanel.Width / 4 - btnWidth / 2, buttonPanel.Height / 2 - btnHeight / 2);
                btnStop.Location = new Point(buttonPanel.Width * 3 / 4 - btnWidth / 2, buttonPanel.Height / 2 - btnHeight / 2);
            };
            
            layoutPanel.Controls.Add(buttonPanel, 1, 5);
            
            // 初始调整按钮位置
            buttonPanel.Size = new Size(layoutPanel.GetColumnWidths()[1], layoutPanel.GetRowHeights()[5]);
            btnStart.Location = new Point(buttonPanel.Width / 4 - btnWidth / 2, buttonPanel.Height / 2 - btnHeight / 2);
            btnPause.Location = new Point(buttonPanel.Width / 4 - btnWidth / 2, buttonPanel.Height / 2 - btnHeight / 2);
            btnStop.Location = new Point(buttonPanel.Width * 3 / 4 - btnWidth / 2, buttonPanel.Height / 2 - btnHeight / 2);
        }
        
        // 添加创建专注模式按钮的独立方法
        private void CreateFocusModeButton()
        {
            // 创建专注模式按钮
            btnToggleFocus = new Button
            {
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                Size = new Size(32, 32),
                Location = new Point(5, 5),
                Text = "◀",
                Font = new Font("微软雅黑", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(240, 240, 240),
                ForeColor = Color.FromArgb(80, 80, 80),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            
            // 添加鼠标事件
            btnToggleFocus.MouseEnter += BtnToggleFocus_MouseEnter;
            btnToggleFocus.MouseLeave += BtnToggleFocus_MouseLeave;
            
            toolTip.SetToolTip(btnToggleFocus, "进入专注模式");
            btnToggleFocus.Click += BtnToggleFocus_Click;
            mainContentPanel.Controls.Add(btnToggleFocus);
            mainContentPanel.Controls.SetChildIndex(btnToggleFocus, 0);
        }
        
        // 鼠标进入按钮区域
        private void BtnToggleFocus_MouseEnter(object sender, EventArgs e)
        {
            if (_focusMode)
            {
                // 在专注模式下，鼠标进入时显示按钮
                btnToggleFocus.BackColor = Color.FromArgb(240, 240, 240);
                btnToggleFocus.ForeColor = Color.FromArgb(80, 80, 80);
            }
        }
        
        // 鼠标离开按钮区域
        private void BtnToggleFocus_MouseLeave(object sender, EventArgs e)
        {
            if (_focusMode)
            {
                // 在专注模式下，鼠标离开时使按钮透明
                btnToggleFocus.BackColor = Color.Transparent;
                btnToggleFocus.ForeColor = Color.FromArgb(0, 0, 0, 0);
            }
        }
        
        // 添加专注模式切换事件处理
        private void BtnToggleFocus_Click(object sender, EventArgs e)
        {
            _focusMode = !_focusMode;
            
            if (_focusMode)
            {
                // 进入专注模式 - 隐藏导航栏
                navPanel.Visible = false;
                btnToggleFocus.Text = "▶";
                toolTip.SetToolTip(btnToggleFocus, "退出专注模式");
                
                // 使按钮透明
                btnToggleFocus.BackColor = Color.Transparent;
                btnToggleFocus.ForeColor = Color.FromArgb(0, 0, 0, 0);
            }
            else
            {
                // 退出专注模式 - 显示导航栏
                navPanel.Visible = true;
                btnToggleFocus.Text = "◀";
                toolTip.SetToolTip(btnToggleFocus, "进入专注模式");
                
                // 恢复按钮可见性
                btnToggleFocus.BackColor = Color.FromArgb(240, 240, 240);
                btnToggleFocus.ForeColor = Color.FromArgb(80, 80, 80);
            }
        }
        
        private void CreateSettingsContent()
        {
            // 创建标签页控件
            settingsTabControl = new TabControl
            {
                Dock = DockStyle.Top,
                Padding = new Point(12, 8),
                Margin = new Padding(15),
                Font = new Font("微软雅黑", 10),
                Height = settingsContentPanel.Height - 70 // 留出底部空间给保存按钮
            };
            
            // 创建常规标签页（包含时间和随机提醒设置）
            tabGeneral = new TabPage("常规");
            
            // 创建通知设置标签页
            tabNotification = new TabPage("通知设置");
            
            // 创建窗口行为标签页
            tabWindow = new TabPage("窗口行为");
            
            // 添加标签页到标签页控件
            settingsTabControl.Controls.Add(tabGeneral);
            settingsTabControl.Controls.Add(tabNotification);
            settingsTabControl.Controls.Add(tabWindow);
            
            // 创建FlowLayoutPanel用于自适应布局
            FlowLayoutPanel generalPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true,
                Padding = new Padding(10),
                WrapContents = false
            };
            tabGeneral.Controls.Add(generalPanel);
            
            FlowLayoutPanel notificationPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true,
                Padding = new Padding(10),
                WrapContents = false
            };
            tabNotification.Controls.Add(notificationPanel);
            
            FlowLayoutPanel windowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true,
                Padding = new Padding(10),
                WrapContents = false
            };
            tabWindow.Controls.Add(windowPanel);
            
            // 创建常规设置控件
            Panel pnlStudyMinutes = new Panel { Width = 350, Height = 40, Margin = new Padding(3, 5, 3, 5) };
            Label lblStudyMinutes = new Label
            {
                Text = "学习时长(分钟):",
                AutoSize = true,
                Font = new Font("微软雅黑", 10),
                Location = new Point(0, 10)
            };
            nudStudyMinutes = new NumericUpDown
            {
                Minimum = 1,
                Maximum = 180,
                Width = 120,
                Font = new Font("微软雅黑", 10),
                Location = new Point(220, 8)
            };
            pnlStudyMinutes.Controls.Add(lblStudyMinutes);
            pnlStudyMinutes.Controls.Add(nudStudyMinutes);
            
            Panel pnlRestMinutes = new Panel { Width = 350, Height = 40, Margin = new Padding(3, 5, 3, 5) };
            Label lblRestMinutes = new Label
            {
                Text = "休息时长(分钟):",
                AutoSize = true,
                Font = new Font("微软雅黑", 10),
                Location = new Point(0, 10)
            };
            nudRestMinutes = new NumericUpDown
            {
                Minimum = 1,
                Maximum = 60,
                Width = 120,
                Font = new Font("微软雅黑", 10),
                Location = new Point(220, 8)
            };
            pnlRestMinutes.Controls.Add(lblRestMinutes);
            pnlRestMinutes.Controls.Add(nudRestMinutes);
            
            Panel pnlMeditationSeconds = new Panel { Width = 350, Height = 40, Margin = new Padding(3, 5, 3, 5) };
            Label lblMeditationSeconds = new Label
            {
                Text = "冥想时长(秒):",
                AutoSize = true,
                Font = new Font("微软雅黑", 10),
                Location = new Point(0, 10)
            };
            nudMeditationSeconds = new NumericUpDown
            {
                Minimum = 5,
                Maximum = 60,
                Width = 120,
                Font = new Font("微软雅黑", 10),
                Location = new Point(220, 8)
            };
            pnlMeditationSeconds.Controls.Add(lblMeditationSeconds);
            pnlMeditationSeconds.Controls.Add(nudMeditationSeconds);
            
            Panel pnlMinRandomMinutes = new Panel { Width = 350, Height = 40, Margin = new Padding(3, 5, 3, 5) };
            Label lblMinRandomMinutes = new Label
            {
                Text = "随机提醒最小间隔(秒):",
                AutoSize = true,
                Font = new Font("微软雅黑", 10),
                Location = new Point(0, 10)
            };
            nudMinRandomMinutes = new NumericUpDown
            {
                Minimum = 30,  // 最少30秒
                Maximum = 1800, // 最多30分钟
                Width = 120,
                Font = new Font("微软雅黑", 10),
                Location = new Point(220, 8)
            };
            pnlMinRandomMinutes.Controls.Add(lblMinRandomMinutes);
            pnlMinRandomMinutes.Controls.Add(nudMinRandomMinutes);
            
            Panel pnlMaxRandomMinutes = new Panel { Width = 350, Height = 40, Margin = new Padding(3, 5, 3, 5) };
            Label lblMaxRandomMinutes = new Label
            {
                Text = "随机提醒最大间隔(秒):",
                AutoSize = true,
                Font = new Font("微软雅黑", 10),
                Location = new Point(0, 10)
            };
            nudMaxRandomMinutes = new NumericUpDown
            {
                Minimum = 30,  // 最少30秒
                Maximum = 3600, // 最多60分钟
                Width = 120,
                Font = new Font("微软雅黑", 10),
                Location = new Point(220, 8)
            };
            pnlMaxRandomMinutes.Controls.Add(lblMaxRandomMinutes);
            pnlMaxRandomMinutes.Controls.Add(nudMaxRandomMinutes);
            
            // 修改音效文件选择控件
            Panel pnlSoundFile = new Panel { Width = 370, Height = 40, Margin = new Padding(3, 5, 3, 5) };
            Label lblSoundFile = new Label
            {
                Text = "提示音效文件:",
                AutoSize = true,
                Font = new Font("微软雅黑", 10),
                Location = new Point(0, 10)
            };

            Panel soundFilePanel = new Panel
            {
                Width = 240,
                Height = 30,
                Location = new Point(120, 5)
            };
            
            // 替换TextBox为ComboBox
            cboSoundFile = new ComboBox
            {
                Width = 240,
                Font = new Font("微软雅黑", 9),
                Location = new Point(0, 0),
                DropDownStyle = ComboBoxStyle.DropDown,
                AutoCompleteMode = AutoCompleteMode.SuggestAppend,
                AutoCompleteSource = AutoCompleteSource.ListItems
            };
            
            // 加载默认音效和自定义音效历史
            LoadSoundFiles();
            
            // 添加ComboBox改变事件
            cboSoundFile.SelectedIndexChanged += CboSoundFile_SelectedIndexChanged;
            
            btnBrowseSoundFile = new Button
            {
                Text = "浏览",
                Width = 60,
                Location = new Point(245, 0),
                Font = new Font("微软雅黑", 9)
            };
            btnBrowseSoundFile.Click += BtnBrowseSoundFile_Click;
            
            btnTestSound = new Button
            {
                Text = "测试",
                Width = 60,
                Location = new Point(310, 0),
                Font = new Font("微软雅黑", 9)
            };
            btnTestSound.Click += BtnTestSound_Click;
            
            soundFilePanel.Controls.Add(cboSoundFile);
            pnlSoundFile.Controls.Add(lblSoundFile);
            pnlSoundFile.Controls.Add(soundFilePanel);
            
            Panel pnlDefaultSound = new Panel { Width = 370, Height = 40, Margin = new Padding(3, 5, 3, 5) };
            btnDefaultSound = new Button
            {
                Text = "恢复默认音效",
                AutoSize = true,
                Font = new Font("微软雅黑", 9),
                Location = new Point(120, 5)
            };
            btnDefaultSound.Click += BtnDefaultSound_Click;
            pnlDefaultSound.Controls.Add(btnDefaultSound);
            
            // 添加控件到通知面板
            notificationPanel.Controls.Add(pnlSoundFile);
            
            // 添加浏览和测试按钮到控件面板
            Panel btnPanel = new Panel { Width = 370, Height = 40, Margin = new Padding(3, 5, 3, 5) };
            btnPanel.Controls.Add(btnBrowseSoundFile);
            btnPanel.Controls.Add(btnTestSound);
            btnBrowseSoundFile.Location = new Point(120, 5);
            btnTestSound.Location = new Point(190, 5);
            notificationPanel.Controls.Add(btnPanel);

            notificationPanel.Controls.Add(pnlDefaultSound);

            Panel pnlShowPopup = new Panel { Width = 370, Height = 40, Margin = new Padding(3, 5, 3, 5) };
            chkShowPopup = new CheckBox
            {
                Text = "提示时弹出系统弹窗",
                AutoSize = true,
                Font = new Font("微软雅黑", 10),
                Location = new Point(120, 10)
            };
            pnlShowPopup.Controls.Add(chkShowPopup);
            notificationPanel.Controls.Add(pnlShowPopup);
            
            
            // 创建窗口行为控件
            Panel pnlCloseAction = new Panel { Width = 370, Height = 160, Margin = new Padding(3, 5, 3, 5) };
            Label lblCloseAction = new Label
            {
                Text = "关闭窗口行为:",
                AutoSize = true,
                Font = new Font("微软雅黑", 10),
                Location = new Point(0, 10)
            };
            
            GroupBox groupCloseAction = new GroupBox
            {
                Text = "",
                Width = 240,
                Height = 130,
                Location = new Point(120, 0),
                Font = new Font("微软雅黑", 10)
            };
            
            radExit = new RadioButton
            {
                Text = "退出程序",
                AutoSize = true,
                Location = new Point(10, 15),
                Font = new Font("微软雅黑", 9.5f)
            };
            
            radMinimize = new RadioButton
            {
                Text = "最小化到任务栏",
                AutoSize = true,
                Location = new Point(10, 40),
                Font = new Font("微软雅黑", 9.5f)
            };
            
            radMinimizeToTray = new RadioButton
            {
                Text = "最小化到系统托盘",
                AutoSize = true,
                Location = new Point(10, 65),
                Font = new Font("微软雅黑", 9.5f)
            };
            
            radAskEveryTime = new RadioButton
            {
                Text = "每次询问",
                AutoSize = true,
                Location = new Point(10, 90),
                Font = new Font("微软雅黑", 9.5f)
            };
            
            groupCloseAction.Controls.Add(radExit);
            groupCloseAction.Controls.Add(radMinimize);
            groupCloseAction.Controls.Add(radMinimizeToTray);
            groupCloseAction.Controls.Add(radAskEveryTime);
            
            pnlCloseAction.Controls.Add(lblCloseAction);
            pnlCloseAction.Controls.Add(groupCloseAction);
            
            Panel pnlSilentMinimize = new Panel { Width = 370, Height = 40, Margin = new Padding(3, 5, 3, 5) };
            chkSilentMinimize = new CheckBox
            {
                Text = "静默最小化到托盘(不显示提示)",
                AutoSize = true,
                Font = new Font("微软雅黑", 10),
                Location = new Point(120, 10)
            };
            pnlSilentMinimize.Controls.Add(chkSilentMinimize);
            
            // 添加控件到各自的面板
            generalPanel.Controls.Add(pnlStudyMinutes);
            generalPanel.Controls.Add(pnlRestMinutes);
            generalPanel.Controls.Add(pnlMeditationSeconds);
            generalPanel.Controls.Add(pnlMinRandomMinutes);
            generalPanel.Controls.Add(pnlMaxRandomMinutes);
            
            // 添加控件到窗口行为面板
            windowPanel.Controls.Add(pnlCloseAction);
            windowPanel.Controls.Add(pnlSilentMinimize);
            
            // 创建保存按钮
            btnSaveSettings = new Button
            {
                Text = "保存设置",
                Width = 120,
                Height = 35,
                Font = new Font("微软雅黑", 10),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                Location = new Point(settingsContentPanel.Width - 140, settingsContentPanel.Height - 55)
            };
            btnSaveSettings.Click += BtnSaveSettings_Click;
            
            // 添加控件到设置内容面板
            settingsContentPanel.Controls.Add(settingsTabControl);
            settingsContentPanel.Controls.Add(btnSaveSettings);
            
            // 设置Resize事件处理
            settingsContentPanel.Resize += (s, e) => {
                btnSaveSettings.Location = new Point(settingsContentPanel.Width - 140, settingsContentPanel.Height - 55);
                settingsTabControl.Height = settingsContentPanel.Height - 70; // 更新TabControl高度
            };
        }
        
        private void BtnNavHome_Click(object sender, EventArgs e)
        {
            ActiveNavButton((Button)sender);
            ShowMainPanel();
        }
        
        private void BtnNavSettings_Click(object sender, EventArgs e)
        {
            ActiveNavButton((Button)sender);
            ShowSettingsPanel();
        }
        
        private void ActiveNavButton(Button button)
        {
            if (activeNavButton != null)
            {
                activeNavButton.BackColor = Color.FromArgb(240, 240, 240);
                activeNavButton.ForeColor = Color.FromArgb(64, 64, 64);
                activeNavButton.Font = new Font("微软雅黑", 10.5f, FontStyle.Regular);
            }
            
            button.BackColor = Color.FromArgb(0, 122, 204);
            button.ForeColor = Color.White;
            button.Font = new Font("微软雅黑", 10.5f, FontStyle.Bold);
            activeNavButton = button;
        }
        
        private void ShowMainPanel()
        {
            mainContentPanel.BringToFront();
            mainContentPanel.Visible = true;
            settingsContentPanel.Visible = false;
        }
        
        private void ShowSettingsPanel()
        {
            // 每次显示设置面板时，更新设置控件的值
            nudStudyMinutes.Value = _settingsManager.StudyMinutes;
            nudRestMinutes.Value = _settingsManager.RestMinutes;
            nudMeditationSeconds.Value = _settingsManager.MeditationSeconds;
            nudMinRandomMinutes.Value = _settingsManager.MinRandomSeconds;
            nudMaxRandomMinutes.Value = _settingsManager.MaxRandomSeconds;
            cboSoundFile.Text = _settingsManager.SoundFile;
            chkShowPopup.Checked = _settingsManager.ShowPopup;
            
            // 设置关闭行为选项
            switch (_settingsManager.DefaultCloseAction)
            {
                case CloseAction.Exit:
                    radExit.Checked = true;
                    break;
                case CloseAction.Minimize:
                    radMinimize.Checked = true;
                    break;
                case CloseAction.MinimizeToTray:
                    radMinimizeToTray.Checked = true;
                    break;
                case CloseAction.AskEveryTime:
                    radAskEveryTime.Checked = true;
                    break;
            }
            
            // 设置最小化静默选项
            chkSilentMinimize.Checked = _settingsManager.SilentMinimize;
            
            settingsContentPanel.BringToFront();
            settingsContentPanel.Visible = true;
            mainContentPanel.Visible = false;
        }
        
        private void BtnSaveSettings_Click(object sender, EventArgs e)
        {
            // 验证随机时间范围
            if (nudMinRandomMinutes.Value > nudMaxRandomMinutes.Value)
            {
                MessageBox.Show("随机时间最小值不能大于最大值", "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 保存设置
            _settingsManager.StudyMinutes = (int)nudStudyMinutes.Value;
            _settingsManager.RestMinutes = (int)nudRestMinutes.Value;
            _settingsManager.MeditationSeconds = (int)nudMeditationSeconds.Value;
            _settingsManager.MinRandomSeconds = (int)nudMinRandomMinutes.Value;
            _settingsManager.MaxRandomSeconds = (int)nudMaxRandomMinutes.Value;
            _settingsManager.SoundFile = cboSoundFile.Text;
            _settingsManager.ShowPopup = chkShowPopup.Checked;
            
            // 保存关闭行为设置
            if (radExit.Checked)
                _settingsManager.DefaultCloseAction = CloseAction.Exit;
            else if (radMinimize.Checked)
                _settingsManager.DefaultCloseAction = CloseAction.Minimize;
            else if (radMinimizeToTray.Checked)
                _settingsManager.DefaultCloseAction = CloseAction.MinimizeToTray;
            else if (radAskEveryTime.Checked)
                _settingsManager.DefaultCloseAction = CloseAction.AskEveryTime;
                
            // 保存最小化静默设置
            _settingsManager.SilentMinimize = chkSilentMinimize.Checked;
            
            // 保存到配置
            _settingsManager.SaveSettings();
            
            // 重新加载音效
            SoundManager.Instance.LoadSound(_settingsManager.SoundFile);
            
            // 更新计时器设置
            _timerManager.UpdateSettings();
            
            // 显示保存成功提示
            MessageBox.Show("设置已保存", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            // 切换到主页面
            Button homeButton = navPanel.Controls.OfType<Button>().FirstOrDefault(b => b.Text == "主页");
            if (homeButton != null)
            {
                BtnNavHome_Click(homeButton, EventArgs.Empty);
            }
        }
        
        // 加载音效文件列表
        private void LoadSoundFiles()
        {
            cboSoundFile.Items.Clear();
            
            // 添加默认音效
            foreach (string sound in DefaultSounds)
            {
                cboSoundFile.Items.Add(sound);
            }
            
            // 加载自定义音效历史
            customSounds = _settingsManager.GetCustomSounds();
            if (customSounds != null && customSounds.Count > 0)
            {
                foreach (string sound in customSounds)
                {
                    if (File.Exists(sound) && !cboSoundFile.Items.Contains(sound))
                    {
                        cboSoundFile.Items.Add(sound);
                    }
                }
            }
            
            // 设置当前选中的音效
            string currentSound = _settingsManager.SoundFile;
            if (!string.IsNullOrEmpty(currentSound))
            {
                cboSoundFile.Text = currentSound;
            }
            else
            {
                cboSoundFile.SelectedIndex = 0; // 默认选择第一个音效
            }
        }

        // ComboBox选中项改变事件
        private void CboSoundFile_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cboSoundFile.Text))
            {
                // 测试播放选择的音效
                SoundManager.Instance.LoadSound(cboSoundFile.Text);
                SoundManager.Instance.Play();
            }
        }

        private void BtnBrowseSoundFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "WAV音频文件(*.wav)|*.wav|所有文件(*.*)|*.*";
                openFileDialog.Title = "选择提示音效文件";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFile = openFileDialog.FileName;
                    
                    // 添加到ComboBox并选中
                    if (!cboSoundFile.Items.Contains(selectedFile))
                    {
                        cboSoundFile.Items.Add(selectedFile);
                        
                        // 添加到自定义音效历史
                        if (customSounds == null) customSounds = new List<string>();
                        if (!customSounds.Contains(selectedFile))
                        {
                            customSounds.Add(selectedFile);
                            _settingsManager.SaveCustomSounds(customSounds);
                        }
                    }
                    cboSoundFile.Text = selectedFile;
                    
                    // 测试播放选择的音效
                    SoundManager.Instance.LoadSound(selectedFile);
                    SoundManager.Instance.Play();
                }
            }
        }

        private void BtnTestSound_Click(object sender, EventArgs e)
        {
            // 测试播放当前音效
            if (!string.IsNullOrEmpty(cboSoundFile.Text))
            {
                SoundManager.Instance.LoadSound(cboSoundFile.Text);
                SoundManager.Instance.Play();
            }
            else
            {
                MessageBox.Show("请先选择一个音效文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnDefaultSound_Click(object sender, EventArgs e)
        {
            // 选择第一个默认音效
            cboSoundFile.SelectedIndex = 0;
        }
        
        private void LoadSettings()
        {
            // 已在ShowSettingsPanel方法中实现设置加载
        }
        
        #endregion
    }
}
