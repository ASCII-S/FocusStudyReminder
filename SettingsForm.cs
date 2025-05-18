using System;
using System.IO;
using System.Windows.Forms;

namespace FocusStudyReminder
{
    public partial class SettingsForm : Form
    {
        private SettingsManager _settingsManager;
        
        public SettingsForm()
        {
            InitializeComponent();
            _settingsManager = SettingsManager.Instance;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            // 加载当前设置
            nudStudyMinutes.Value = SettingsManager.Instance.StudyMinutes;
            nudRestMinutes.Value = SettingsManager.Instance.RestMinutes;
            nudMeditationSeconds.Value = SettingsManager.Instance.MeditationSeconds;
            nudMinRandomMinutes.Value = SettingsManager.Instance.MinRandomMinutes;
            nudMaxRandomMinutes.Value = SettingsManager.Instance.MaxRandomMinutes;
            txtSoundFile.Text = SettingsManager.Instance.SoundFile;
            chkShowPopup.Checked = SettingsManager.Instance.ShowPopup;
            
            // 设置关闭行为选项
            switch (SettingsManager.Instance.DefaultCloseAction)
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
            chkSilentMinimize.Checked = SettingsManager.Instance.SilentMinimize;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // 验证随机时间范围
            if (nudMinRandomMinutes.Value > nudMaxRandomMinutes.Value)
            {
                MessageBox.Show("随机时间最小值不能大于最大值", "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 保存设置
            SettingsManager.Instance.StudyMinutes = (int)nudStudyMinutes.Value;
            SettingsManager.Instance.RestMinutes = (int)nudRestMinutes.Value;
            SettingsManager.Instance.MeditationSeconds = (int)nudMeditationSeconds.Value;
            SettingsManager.Instance.MinRandomMinutes = (int)nudMinRandomMinutes.Value;
            SettingsManager.Instance.MaxRandomMinutes = (int)nudMaxRandomMinutes.Value;
            SettingsManager.Instance.SoundFile = txtSoundFile.Text;
            SettingsManager.Instance.ShowPopup = chkShowPopup.Checked;
            
            // 保存关闭行为设置
            if (radExit.Checked)
                SettingsManager.Instance.DefaultCloseAction = CloseAction.Exit;
            else if (radMinimize.Checked)
                SettingsManager.Instance.DefaultCloseAction = CloseAction.Minimize;
            else if (radMinimizeToTray.Checked)
                SettingsManager.Instance.DefaultCloseAction = CloseAction.MinimizeToTray;
            else if (radAskEveryTime.Checked)
                SettingsManager.Instance.DefaultCloseAction = CloseAction.AskEveryTime;
                
            // 保存最小化静默设置
            SettingsManager.Instance.SilentMinimize = chkSilentMinimize.Checked;
            
            // 保存到配置
            SettingsManager.Instance.SaveSettings();
            
            // 重新加载音效
            SoundManager.Instance.LoadSound(SettingsManager.Instance.SoundFile);

            // 关闭窗体
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnBrowseSoundFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "WAV音频文件(*.wav)|*.wav|所有文件(*.*)|*.*";
                openFileDialog.Title = "选择提示音效文件";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtSoundFile.Text = openFileDialog.FileName;
                    
                    // 测试播放选择的音效
                    SoundManager.Instance.LoadSound(openFileDialog.FileName);
                    SoundManager.Instance.Play();
                }
            }
        }

        private void btnTestSound_Click(object sender, EventArgs e)
        {
            // 测试播放当前音效
            if (!string.IsNullOrEmpty(txtSoundFile.Text))
            {
                SoundManager.Instance.LoadSound(txtSoundFile.Text);
                SoundManager.Instance.Play();
            }
            else
            {
                MessageBox.Show("请先选择一个音效文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnDefaultSound_Click(object sender, EventArgs e)
        {
            // 恢复默认音效
            txtSoundFile.Text = "default.wav";
            SoundManager.Instance.LoadDefaultSound();
            SoundManager.Instance.Play();
        }
    }
} 