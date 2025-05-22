using System;
using System.IO;
using System.Media;
using System.Windows.Forms;
using WMPLib; // 添加 Windows Media Player 库引用

namespace FocusStudyReminder
{
    public class SoundManager
    {
        private SoundPlayer _soundPlayer;
        private SoundPlayer _mainTimerSoundPlayer; // 新增大计时器音效播放器
        private WindowsMediaPlayer _mp3Player; // 添加 MP3 播放器
        private WindowsMediaPlayer _mainTimerMp3Player; // 添加主计时器 MP3 播放器
        private string _currentSoundFile;
        private string _currentMainTimerSoundFile; // 新增大计时器音效文件路径
        private bool _isPlayingMp3 = false; // 跟踪是否正在播放 MP3
        private bool _isPlayingMainTimerMp3 = false; // 跟踪是否正在播放主计时器 MP3
        
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
            _mainTimerSoundPlayer = new SoundPlayer(); // 初始化大计时器音效播放器
            _mp3Player = new WindowsMediaPlayer();
            _mainTimerMp3Player = new WindowsMediaPlayer();
            
            // 设置播放完成后自动关闭
            _mp3Player.PlayStateChange += Mp3Player_PlayStateChange;
            _mainTimerMp3Player.PlayStateChange += MainTimerMp3Player_PlayStateChange;
            
            // 设置播放器为静音模式避免显示界面
            _mp3Player.settings.autoStart = true;
            _mp3Player.settings.enableErrorDialogs = false;
            _mainTimerMp3Player.settings.autoStart = true;
            _mainTimerMp3Player.settings.enableErrorDialogs = false;
            
            LoadDefaultSound();
            LoadDefaultMainTimerSound(); // 加载默认大计时器音效
        }
        
        // MP3 播放器状态改变事件处理
        private void Mp3Player_PlayStateChange(int newState)
        {
            // 8 = 播放结束
            if (newState == 8)
            {
                _isPlayingMp3 = false;
            }
        }
        
        // 主计时器 MP3 播放器状态改变事件处理
        private void MainTimerMp3Player_PlayStateChange(int newState)
        {
            // 8 = 播放结束
            if (newState == 8)
            {
                _isPlayingMainTimerMp3 = false;
            }
        }
        
        // 加载默认小计时器音效
        public void LoadDefaultSound()
        {
            string soundFile = SettingsManager.Instance.SoundFile;
            LoadSound(soundFile);
        }
        
        // 加载默认大计时器音效
        public void LoadDefaultMainTimerSound()
        {
            string soundFile = SettingsManager.Instance.MainTimerSoundFile;
            LoadMainTimerSound(soundFile);
        }
        
        // 加载小计时器音效
        public bool LoadSound(string soundFile)
        {
            try
            {
                string fullPath = GetSoundFilePath(soundFile);
                if (string.IsNullOrEmpty(fullPath)) return false;
                
                _currentSoundFile = soundFile;
                
                // 根据文件扩展名决定使用哪个播放器
                if (Path.GetExtension(fullPath).ToLower() == ".mp3")
                {
                    _mp3Player.URL = fullPath;
                    _mp3Player.controls.stop(); // 加载但不播放
                }
                else
                {
                    _soundPlayer.SoundLocation = fullPath;
                    _soundPlayer.Load();
                }
                
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载音效文件失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        
        // 加载大计时器音效
        public bool LoadMainTimerSound(string soundFile)
        {
            try
            {
                string fullPath = GetSoundFilePath(soundFile);
                if (string.IsNullOrEmpty(fullPath)) return false;
                
                _currentMainTimerSoundFile = soundFile;
                
                // 根据文件扩展名决定使用哪个播放器
                if (Path.GetExtension(fullPath).ToLower() == ".mp3")
                {
                    _mainTimerMp3Player.URL = fullPath;
                    _mainTimerMp3Player.controls.stop(); // 加载但不播放
                }
                else
                {
                    _mainTimerSoundPlayer.SoundLocation = fullPath;
                    _mainTimerSoundPlayer.Load();
                }
                
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载大计时器音效文件失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        
        // 获取音效文件的完整路径
        private string GetSoundFilePath(string soundFile)
        {
            if (string.IsNullOrEmpty(soundFile))
                return null;
                
            if (Path.IsPathRooted(soundFile))
            {
                // 检查文件是否存在
                if (!File.Exists(soundFile))
                {
                    MessageBox.Show($"音效文件不存在: {soundFile}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return null;
                }
                return soundFile;
            }
            else
            {
                // 如果是相对路径，则尝试从应用程序目录加载
                string fullPath = Path.Combine(Application.StartupPath, "Sounds", soundFile);
                
                // 输出调试信息
                Console.WriteLine($"尝试加载音效文件: {fullPath}");
                
                // 如果文件不存在，尝试加载默认音效
                if (!File.Exists(fullPath))
                {
                    Console.WriteLine($"音效文件不存在: {fullPath}");
                    
                    // 确定文件扩展名和默认文件名
                    string extension = Path.GetExtension(soundFile).ToLower();
                    string defaultFile;
                    
                    if (extension == ".mp3")
                    {
                        defaultFile = "default.mp3";
                        Console.WriteLine("尝试加载默认MP3文件");
                    }
                    else
                    {
                        defaultFile = "default.wav";
                        Console.WriteLine("尝试加载默认WAV文件");
                    }
                    
                    // 尝试使用对应格式的默认文件
                    string defaultPath = Path.Combine(Application.StartupPath, "Sounds", defaultFile);
                    Console.WriteLine($"尝试加载默认音效: {defaultPath}");
                    
                    // 如果默认文件不存在，尝试另一种格式
                    if (!File.Exists(defaultPath))
                    {
                        string alternateDefault = (extension == ".mp3") ? "default.wav" : "default.mp3";
                        defaultPath = Path.Combine(Application.StartupPath, "Sounds", alternateDefault);
                        Console.WriteLine($"默认音效不存在，尝试替代音效: {defaultPath}");
                    }
                    
                    // 确保Sounds目录存在
                    Directory.CreateDirectory(Path.Combine(Application.StartupPath, "Sounds"));
                    
                    // 如果所有默认音效都不存在
                    if (!File.Exists(defaultPath))
                    {
                        Console.WriteLine("没有可用的默认音效，将使用系统声音");
                        return null;
                    }
                    
                    fullPath = defaultPath;
                }
                
                return fullPath;
            }
        }
        
        // 播放小计时器音效
        public void Play()
        {
            try
            {
                if (_currentSoundFile == null)
                {
                    // 如果没有加载音效，使用系统提示音
                    System.Media.SystemSounds.Exclamation.Play();
                    return;
                }
                
                string fullPath = GetSoundFilePath(_currentSoundFile);
                if (string.IsNullOrEmpty(fullPath))
                {
                    System.Media.SystemSounds.Exclamation.Play();
                    return;
                }
                
                // 根据文件扩展名决定使用哪个播放器
                if (Path.GetExtension(fullPath).ToLower() == ".mp3")
                {
                    _mp3Player.URL = fullPath;
                    _mp3Player.controls.play();
                    _isPlayingMp3 = true;
                }
                else
                {
                    _soundPlayer.Play();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"播放音效失败: {ex.Message}");
                System.Media.SystemSounds.Exclamation.Play();
            }
        }
        
        // 播放大计时器音效
        public void PlayMainTimerSound()
        {
            try
            {
                if (_currentMainTimerSoundFile == null)
                {
                    // 如果没有加载音效，使用系统提示音
                    System.Media.SystemSounds.Asterisk.Play();
                    return;
                }
                
                string fullPath = GetSoundFilePath(_currentMainTimerSoundFile);
                if (string.IsNullOrEmpty(fullPath))
                {
                    System.Media.SystemSounds.Asterisk.Play();
                    return;
                }
                
                // 根据文件扩展名决定使用哪个播放器
                if (Path.GetExtension(fullPath).ToLower() == ".mp3")
                {
                    _mainTimerMp3Player.URL = fullPath;
                    _mainTimerMp3Player.controls.play();
                    _isPlayingMainTimerMp3 = true;
                }
                else
                {
                    _mainTimerSoundPlayer.Play();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"播放大计时器音效失败: {ex.Message}");
                System.Media.SystemSounds.Asterisk.Play();
            }
        }
        
        // 停止音效
        public void Stop()
        {
            try
            {
                // 停止 WAV 播放
                if (_soundPlayer != null)
                {
                    _soundPlayer.Stop();
                }
                
                if (_mainTimerSoundPlayer != null)
                {
                    _mainTimerSoundPlayer.Stop();
                }
                
                // 停止 MP3 播放
                if (_mp3Player != null && _isPlayingMp3)
                {
                    _mp3Player.controls.stop();
                    _isPlayingMp3 = false;
                }
                
                if (_mainTimerMp3Player != null && _isPlayingMainTimerMp3)
                {
                    _mainTimerMp3Player.controls.stop();
                    _isPlayingMainTimerMp3 = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"停止音效失败: {ex.Message}");
            }
        }
        
        // 获取当前小计时器音效文件
        public string GetCurrentSoundFile()
        {
            return _currentSoundFile;
        }
        
        // 获取当前大计时器音效文件
        public string GetCurrentMainTimerSoundFile()
        {
            return _currentMainTimerSoundFile;
        }
    }
} 