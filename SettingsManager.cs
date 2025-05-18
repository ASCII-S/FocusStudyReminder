using System;
using System.Configuration;
using System.IO;

namespace FocusStudyReminder
{
    // 定义关闭行为枚举
    public enum CloseAction
    {
        Exit,           // 退出应用
        Minimize,       // 最小化到任务栏
        MinimizeToTray, // 最小化到系统托盘
        AskEveryTime    // 每次询问
    }

    public class SettingsManager
    {
        // 默认设置值
        private const int DEFAULT_STUDY_MINUTES = 90;
        private const int DEFAULT_REST_MINUTES = 20;
        private const int DEFAULT_MEDITATION_SECONDS = 20;
        private const int DEFAULT_MIN_RANDOM_SECONDS = 180; // 3分钟 = 180秒
        private const int DEFAULT_MAX_RANDOM_SECONDS = 300; // 5分钟 = 300秒
        private const string DEFAULT_SOUND_FILE = "default.wav";
        private const bool DEFAULT_SHOW_POPUP = true;
        private const CloseAction DEFAULT_CLOSE_ACTION = CloseAction.MinimizeToTray;
        private const bool DEFAULT_SILENT_MINIMIZE = true;

        // 单例模式
        private static SettingsManager _instance;
        public static SettingsManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SettingsManager();
                return _instance;
            }
        }

        // 设置属性
        public int StudyMinutes { get; set; }
        public int RestMinutes { get; set; }
        public int MeditationSeconds { get; set; }
        public int MinRandomSeconds { get; set; } // 改为秒
        public int MaxRandomSeconds { get; set; } // 改为秒
        public string SoundFile { get; set; }
        public bool ShowPopup { get; set; }
        public CloseAction DefaultCloseAction { get; set; }
        public bool SilentMinimize { get; set; }
        public int WindowWidth { get; set; }
        public int WindowHeight { get; set; }

        private SettingsManager()
        {
            LoadSettings();
        }

        // 加载设置
        public void LoadSettings()
        {
            StudyMinutes = GetSetting("StudyMinutes", DEFAULT_STUDY_MINUTES);
            RestMinutes = GetSetting("RestMinutes", DEFAULT_REST_MINUTES);
            MeditationSeconds = GetSetting("MeditationSeconds", DEFAULT_MEDITATION_SECONDS);
            
            // 兼容旧配置 - 如果存在MinRandomMinutes，则转换为秒
            if (ConfigurationManager.AppSettings["MinRandomMinutes"] != null && int.TryParse(ConfigurationManager.AppSettings["MinRandomMinutes"], out int minMinutes))
            {
                MinRandomSeconds = minMinutes * 60;
            }
            else
            {
                MinRandomSeconds = GetSetting("MinRandomSeconds", DEFAULT_MIN_RANDOM_SECONDS);
            }
            
            // 兼容旧配置 - 如果存在MaxRandomMinutes，则转换为秒
            if (ConfigurationManager.AppSettings["MaxRandomMinutes"] != null && int.TryParse(ConfigurationManager.AppSettings["MaxRandomMinutes"], out int maxMinutes))
            {
                MaxRandomSeconds = maxMinutes * 60;
            }
            else
            {
                MaxRandomSeconds = GetSetting("MaxRandomSeconds", DEFAULT_MAX_RANDOM_SECONDS);
            }
            
            SoundFile = GetSetting("SoundFile", DEFAULT_SOUND_FILE);
            ShowPopup = GetSetting("ShowPopup", DEFAULT_SHOW_POPUP);
            DefaultCloseAction = GetSetting("DefaultCloseAction", DEFAULT_CLOSE_ACTION);
            SilentMinimize = GetSetting("SilentMinimize", DEFAULT_SILENT_MINIMIZE);
            WindowWidth = GetSetting("WindowWidth", 0);
            WindowHeight = GetSetting("WindowHeight", 0);
        }

        // 保存设置
        public void SaveSettings()
        {
            SaveSetting("StudyMinutes", StudyMinutes);
            SaveSetting("RestMinutes", RestMinutes);
            SaveSetting("MeditationSeconds", MeditationSeconds);
            SaveSetting("MinRandomSeconds", MinRandomSeconds);
            SaveSetting("MaxRandomSeconds", MaxRandomSeconds);
            SaveSetting("SoundFile", SoundFile);
            SaveSetting("ShowPopup", ShowPopup);
            SaveSetting("DefaultCloseAction", (int)DefaultCloseAction);
            SaveSetting("SilentMinimize", SilentMinimize);
            SaveSetting("WindowWidth", WindowWidth);
            SaveSetting("WindowHeight", WindowHeight);
            
            // 移除旧的分钟配置（如果存在）
            RemoveSetting("MinRandomMinutes");
            RemoveSetting("MaxRandomMinutes");
        }

        // 获取整数设置
        private int GetSetting(string key, int defaultValue)
        {
            string value = ConfigurationManager.AppSettings[key];
            if (int.TryParse(value, out int result))
                return result;
            return defaultValue;
        }

        // 获取字符串设置
        private string GetSetting(string key, string defaultValue)
        {
            string value = ConfigurationManager.AppSettings[key];
            return string.IsNullOrEmpty(value) ? defaultValue : value;
        }

        // 获取布尔设置
        private bool GetSetting(string key, bool defaultValue)
        {
            string value = ConfigurationManager.AppSettings[key];
            if (bool.TryParse(value, out bool result))
                return result;
            return defaultValue;
        }

        // 获取枚举设置
        private T GetSetting<T>(string key, T defaultValue) where T : struct
        {
            string value = ConfigurationManager.AppSettings[key];
            if (int.TryParse(value, out int intValue) && Enum.IsDefined(typeof(T), intValue))
                return (T)Enum.ToObject(typeof(T), intValue);
            return defaultValue;
        }

        // 保存设置
        private void SaveSetting(string key, object value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings[key] != null)
                config.AppSettings.Settings[key].Value = value.ToString();
            else
                config.AppSettings.Settings.Add(key, value.ToString());
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        // 移除设置项
        private void RemoveSetting(string key)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings[key] != null)
            {
                config.AppSettings.Settings.Remove(key);
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }
    }
} 