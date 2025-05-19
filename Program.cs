using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace FocusStudyReminder
{
    internal static class Program
    {
        // Win32 API
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        
        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);
        
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);
        
        [DllImport("user32.dll")]
        private static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);
        
        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);
        
        [DllImport("kernel32.dll")]
        private static extern uint GetCurrentThreadId();

        private const int SW_RESTORE = 9;
        private const int SW_SHOW = 5;
        private const int SW_SHOWNORMAL = 1;
        
        [StructLayout(LayoutKind.Sequential)]
        private struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public int showCmd;
            public System.Drawing.Point ptMinPosition;
            public System.Drawing.Point ptMaxPosition;
            public System.Drawing.Rectangle rcNormalPosition;
        }

        // 全局互斥体，确保单实例运行
        private static Mutex _mutex = null;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            const string appName = "FocusStudyReminderApp";
            bool createdNew;

            // 尝试创建命名互斥体
            _mutex = new Mutex(true, appName, out createdNew);

            if (!createdNew)
            {
                // 程序已经在运行，查找已有实例并激活它
                Process current = Process.GetCurrentProcess();
                foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                {
                    if (process.Id != current.Id)
                    {
                        // 找到已存在的实例
                        try
                        {
                            ForceWindowToForeground(process.MainWindowHandle);
                        }
                        catch
                        {
                            // 如果激活窗口失败，继续尝试其他方法
                        }
                        
                        // 不论结果如何，退出当前实例
                        return;
                    }
                }
            }

            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            finally
            {
                // 确保释放互斥体
                if (_mutex != null)
                {
                    _mutex.ReleaseMutex();
                    _mutex.Dispose();
                }
            }
        }
        
        /// <summary>
        /// 强制将窗口恢复并置于前台
        /// </summary>
        private static void ForceWindowToForeground(IntPtr hWnd)
        {
            if (hWnd == IntPtr.Zero)
                return;
                
            // 检查窗口是否最小化
            if (IsIconic(hWnd))
            {
                // 恢复最小化窗口
                ShowWindow(hWnd, SW_RESTORE);
            }
            
            // 获取窗口线程ID
            uint foregroundThreadId = GetWindowThreadProcessId(GetForegroundWindow(), IntPtr.Zero);
            uint appThreadId = GetCurrentThreadId();
            
            // 将线程输入状态联接，以便将窗口置于前台
            if (foregroundThreadId != appThreadId)
            {
                AttachThreadInput(foregroundThreadId, appThreadId, true);
                SetForegroundWindow(hWnd);
                AttachThreadInput(foregroundThreadId, appThreadId, false);
            }
            else
            {
                SetForegroundWindow(hWnd);
            }
            
            // 确保窗口可见
            ShowWindow(hWnd, SW_SHOW);
        }
        
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
    }
}
