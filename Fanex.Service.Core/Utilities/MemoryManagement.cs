// -----------------------------------------------------------------------
// <copyright file="MemoryManagement.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Utilities
{
    using System;
    using System.Runtime.InteropServices;
    using System.ServiceProcess;
    using Fanex.Service.Core.Extensions;
    using System.Timers;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MemoryManagement
    {
        [DllImportAttribute("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int SetProcessWorkingSetSize(IntPtr process, int minimumWorkingSetSize, int maximumWorkingSetSize);

        public static void FlushMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            }
        }

        public static void RestartService(string serviceName, int timeoutMilliseconds)
        {
            ServiceController service = new ServiceController(serviceName);
            try
            {
                Timer timer = new Timer();
                timer.Interval = timeoutMilliseconds;
                timer.Elapsed += (sender, e) =>
                {
                    int millisec1 = Environment.TickCount;
                    TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                    service.Stop();
                    service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);

                    // count the rest of the timeout
                    int millisec2 = Environment.TickCount;
                    timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds - (millisec2 - millisec1));

                    service.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                };

                timer.Start();
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }
    }
}