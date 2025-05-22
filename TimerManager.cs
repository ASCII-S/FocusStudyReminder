using System;
using System.Windows.Forms;

namespace FocusStudyReminder
{
    // 定义计时器状态枚举
    public enum TimerState
    {
        Stopped,
        Study,
        Rest,
        Meditation
    }

    public class TimerManager
    {
        // 单例模式
        private static TimerManager _instance;
        public static TimerManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new TimerManager();
                return _instance;
            }
        }

        // 计时器
        private Timer _mainTimer;
        private Timer _subTimer;
        
        // 当前状态
        private TimerState _currentState;
        public TimerState CurrentState => _currentState;
        
        // 计时器剩余时间（秒）
        private int _mainRemainingSeconds;
        private int _subRemainingSeconds;
        
        // 子计时器初始总秒数，用于进度条计算
        private int _subInitialSeconds;
        
        // 随机数生成器
        private Random _random;

        // 事件
        public event EventHandler<TimerState> StateChanged;
        public event EventHandler<int> MainTimerTick;
        public event EventHandler<int> SubTimerTick;
        public event EventHandler MeditationStarted;
        public event EventHandler MeditationEnded;
        public event EventHandler StudySessionStarted;
        public event EventHandler RestSessionStarted;

        private TimerManager()
        {
            _mainTimer = new Timer();
            _subTimer = new Timer();
            _mainTimer.Interval = 1000; // 1秒
            _subTimer.Interval = 1000; // 1秒
            _mainTimer.Tick += MainTimer_Tick;
            _subTimer.Tick += SubTimer_Tick;
            _currentState = TimerState.Stopped;
            _random = new Random();
        }

        // 开始学习会话
        public void StartStudySession()
        {
            if (_currentState != TimerState.Stopped && _currentState != TimerState.Rest)
                return;

            // 设置主计时器为学习时间
            _mainRemainingSeconds = SettingsManager.Instance.StudyMinutes * 60;
            _currentState = TimerState.Study;
            _mainTimer.Start();
            
            // 开始子计时器（随机倒计时）
            StartRandomSubTimer();
            
            // 播放提示音
            SoundManager.Instance.Play();
            
            // 触发事件
            StateChanged?.Invoke(this, _currentState);
            StudySessionStarted?.Invoke(this, EventArgs.Empty);
        }

        // 开始休息会话
        public void StartRestSession()
        {
            if (_currentState != TimerState.Stopped && _currentState != TimerState.Study)
                return;

            // 停止子计时器
            _subTimer.Stop();
            
            // 设置主计时器为休息时间
            _mainRemainingSeconds = SettingsManager.Instance.RestMinutes * 60;
            _currentState = TimerState.Rest;
            _mainTimer.Start();
            
            // 播放提示音
            SoundManager.Instance.Play();
            
            // 触发事件
            StateChanged?.Invoke(this, _currentState);
            RestSessionStarted?.Invoke(this, EventArgs.Empty);
        }

        // 开始冥想
        private void StartMeditation()
        {
            if (_currentState != TimerState.Study)
                return;

            // 停止子计时器
            _subTimer.Stop();
            
            // 设置冥想时间
            _subRemainingSeconds = SettingsManager.Instance.MeditationSeconds;
            // 记录初始总秒数，用于进度条计算
            _subInitialSeconds = _subRemainingSeconds;
            _currentState = TimerState.Meditation;
            
            // 播放提示音
            SoundManager.Instance.Play();
            
            // 启动子计时器
            _subTimer.Start();
            
            // 触发事件
            StateChanged?.Invoke(this, _currentState);
            MeditationStarted?.Invoke(this, EventArgs.Empty);
        }

        // 结束冥想
        private void EndMeditation()
        {
            if (_currentState != TimerState.Meditation)
                return;

            // 停止子计时器
            _subTimer.Stop();
            
            // 回到学习状态
            _currentState = TimerState.Study;
            
            // 播放提示音
            SoundManager.Instance.Play();
            
            // 触发事件
            StateChanged?.Invoke(this, _currentState);
            MeditationEnded?.Invoke(this, EventArgs.Empty);
            
            // 开始新的随机子计时器
            StartRandomSubTimer();
        }

        // 开始随机子计时器
        private void StartRandomSubTimer()
        {
            if (_currentState != TimerState.Study)
                return;

            // 生成随机时间（秒）
            int minSeconds = SettingsManager.Instance.MinRandomSeconds;
            int maxSeconds = SettingsManager.Instance.MaxRandomSeconds;
            int randomSeconds = _random.Next(minSeconds, maxSeconds + 1);
            
            // 设置子计时器时间
            _subRemainingSeconds = randomSeconds;
            // 记录初始总秒数，用于进度条计算
            _subInitialSeconds = _subRemainingSeconds;
            
            // 启动子计时器
            _subTimer.Start();
        }

        // 暂停计时器
        public void Pause()
        {
            _mainTimer.Stop();
            _subTimer.Stop();
        }

        // 继续计时器
        public void Resume()
        {
            if (_currentState != TimerState.Stopped)
            {
                _mainTimer.Start();
                if (_currentState == TimerState.Study || _currentState == TimerState.Meditation)
                {
                    _subTimer.Start();
                }
            }
        }

        // 停止计时器
        public void Stop()
        {
            _mainTimer.Stop();
            _subTimer.Stop();
            _currentState = TimerState.Stopped;
            _mainRemainingSeconds = 0;
            _subRemainingSeconds = 0;
            _subInitialSeconds = 0;
            
            // 触发事件
            StateChanged?.Invoke(this, _currentState);
        }
        
        // 更新设置
        public void UpdateSettings()
        {
            // 当前正在进行学习或休息时，更新对应的计时器剩余时间
            if (_currentState == TimerState.Study)
            {
                // 如果当前剩余时间大于设置的最大时间，则重置为设置的时间
                if (_mainRemainingSeconds > SettingsManager.Instance.StudyMinutes * 60)
                {
                    _mainRemainingSeconds = SettingsManager.Instance.StudyMinutes * 60;
                }
            }
            else if (_currentState == TimerState.Rest)
            {
                // 如果当前剩余时间大于设置的最大时间，则重置为设置的时间
                if (_mainRemainingSeconds > SettingsManager.Instance.RestMinutes * 60)
                {
                    _mainRemainingSeconds = SettingsManager.Instance.RestMinutes * 60;
                }
            }
            else if (_currentState == TimerState.Meditation)
            {
                // 冥想状态下，如果设置的时间小于当前剩余时间，则更新
                if (_subRemainingSeconds > SettingsManager.Instance.MeditationSeconds)
                {
                    _subRemainingSeconds = SettingsManager.Instance.MeditationSeconds;
                    _subInitialSeconds = _subRemainingSeconds;
                }
            }
        }

        // 主计时器Tick事件处理
        private void MainTimer_Tick(object sender, EventArgs e)
        {
            _mainRemainingSeconds--;
            
            // 触发Tick事件
            MainTimerTick?.Invoke(this, _mainRemainingSeconds);
            
            // 检查是否结束当前状态
            if (_mainRemainingSeconds <= 0)
            {
                _mainTimer.Stop();
                
                // 播放大计时器提示音
                SoundManager.Instance.PlayMainTimerSound();
                
                if (_currentState == TimerState.Study)
                {
                    // 学习结束后开始休息
                    StartRestSession();
                }
                else if (_currentState == TimerState.Rest)
                {
                    // 休息结束后开始学习
                    StartStudySession();
                }
            }
        }

        // 子计时器Tick事件处理
        private void SubTimer_Tick(object sender, EventArgs e)
        {
            _subRemainingSeconds--;
            
            // 触发Tick事件
            SubTimerTick?.Invoke(this, _subRemainingSeconds);
            
            // 检查是否结束当前子状态
            if (_subRemainingSeconds <= 0)
            {
                _subTimer.Stop();
                
                if (_currentState == TimerState.Study)
                {
                    // 随机倒计时结束后开始冥想
                    StartMeditation();
                }
                else if (_currentState == TimerState.Meditation)
                {
                    // 冥想结束后回到学习状态
                    EndMeditation();
                }
            }
        }

        // 获取主计时器剩余时间
        public int GetMainRemainingSeconds()
        {
            return _mainRemainingSeconds;
        }

        // 获取子计时器剩余时间
        public int GetSubRemainingSeconds()
        {
            return _subRemainingSeconds;
        }

        // 获取子计时器初始时间
        public int GetSubInitialSeconds()
        {
            return _subInitialSeconds;
        }

        // 格式化时间显示（秒转为mm:ss格式）
        public static string FormatTime(int seconds)
        {
            int minutes = seconds / 60;
            int remainingSeconds = seconds % 60;
            return $"{minutes:00}:{remainingSeconds:00}";
        }
    }
} 