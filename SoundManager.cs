using System;
using System.IO;
using System.Media;
using System.Windows.Forms;

namespace FocusStudyReminder
{
    public class SoundManager
    {
        private SoundPlayer _soundPlayer;
        private string _currentSoundFile;
        
        // 单例模式
        private static SoundManager _instance;
        public static SoundManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SoundManager();
                return _instance;
            }
        }
        
        private SoundManager()
        {
            _soundPlayer = new SoundPlayer();
            LoadDefaultSound();
        }
        
        // 加载默认音效
        public void LoadDefaultSound()
        {
            string soundFile = SettingsManager.Instance.SoundFile;
            LoadSound(soundFile);
        }
        
        // 加载指定音效
        public bool LoadSound(string soundFile)
        {
            try
            {
                string fullPath;
                if (Path.IsPathRooted(soundFile))
                {
                    fullPath = soundFile;
                }
                else
                {
                    // 如果是相对路径，则尝试从应用程序目录加载
                    fullPath = Path.Combine(Application.StartupPath, "Sounds", soundFile);
                    
                    // 输出调试信息
                    Console.WriteLine($"尝试加载音效文件: {fullPath}");
                    
                    // 如果文件不存在，尝试加载默认音效
                    if (!File.Exists(fullPath))
                    {
                        MessageBox.Show($"音效文件不存在: {fullPath}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        fullPath = Path.Combine(Application.StartupPath, "Sounds", "default.wav");
                        
                        // 输出调试信息
                        Console.WriteLine($"尝试加载默认音效: {fullPath}");
                        
                        // 确保Sounds目录存在
                        Directory.CreateDirectory(Path.Combine(Application.StartupPath, "Sounds"));
                        
                        // 如果默认音效也不存在，使用系统提示音
                        if (!File.Exists(fullPath))
                        {
                            MessageBox.Show($"默认音效文件不存在: {fullPath}，将使用系统音效", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            _soundPlayer.SoundLocation = null;
                            _soundPlayer.Stream = null;
                            _currentSoundFile = null;
                            return false;
                        }
                    }
                }
                
                _soundPlayer.SoundLocation = fullPath;
                _currentSoundFile = soundFile;
                _soundPlayer.Load();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载音效文件失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        
        // 播放音效
        public void Play()
        {
            try
            {
                if (_soundPlayer != null)
                {
                    if (string.IsNullOrEmpty(_soundPlayer.SoundLocation) && _soundPlayer.Stream == null)
                    {
                        // 如果没有加载音效，使用系统提示音
                        System.Media.SystemSounds.Exclamation.Play();
                    }
                    else
                    {
                        _soundPlayer.Play();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"播放音效失败: {ex.Message}");
            }
        }
        
        // 停止音效
        public void Stop()
        {
            try
            {
                if (_soundPlayer != null)
                {
                    _soundPlayer.Stop();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"停止音效失败: {ex.Message}");
            }
        }
        
        // 获取当前音效文件
        public string GetCurrentSoundFile()
        {
            return _currentSoundFile;
        }
    }
} 