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
        private const int DEFAULT_MIN_RANDOM_MINUTES = 3;
        private const int DEFAULT_MAX_RANDOM_MINUTES = 5;
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
        public int MinRandomMinutes { get; set; }
        public int MaxRandomMinutes { get; set; }
        public string SoundFile { get; set; }
        public bool ShowPopup { get; set; }
        public CloseAction DefaultCloseAction { get; set; }
        public bool SilentMinimize { get; set; }

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
            MinRandomMinutes = GetSetting("MinRandomMinutes", DEFAULT_MIN_RANDOM_MINUTES);
            MaxRandomMinutes = GetSetting("MaxRandomMinutes", DEFAULT_MAX_RANDOM_MINUTES);
            SoundFile = GetSetting("SoundFile", DEFAULT_SOUND_FILE);
            ShowPopup = GetSetting("ShowPopup", DEFAULT_SHOW_POPUP);
            DefaultCloseAction = GetSetting("DefaultCloseAction", DEFAULT_CLOSE_ACTION);
            SilentMinimize = GetSetting("SilentMinimize", DEFAULT_SILENT_MINIMIZE);
        }

        // 保存设置
        public void SaveSettings()
        {
            SaveSetting("StudyMinutes", StudyMinutes);
            SaveSetting("RestMinutes", RestMinutes);
            SaveSetting("MeditationSeconds", MeditationSeconds);
            SaveSetting("MinRandomMinutes", MinRandomMinutes);
            SaveSetting("MaxRandomMinutes", MaxRandomMinutes);
            SaveSetting("SoundFile", SoundFile);
            SaveSetting("ShowPopup", ShowPopup);
            SaveSetting("DefaultCloseAction", (int)DefaultCloseAction);
            SaveSetting("SilentMinimize", SilentMinimize);
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
    }
} 