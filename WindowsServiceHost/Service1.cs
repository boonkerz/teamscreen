using Common.EventArgs.Network;
using Common.Thread;
using Driver.Windows;
using Driver.Windows.Desktop;
using Driver.Windows.Screen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowsConsoleHost;

namespace WindowsServiceHost
{
    public partial class Service1 : ServiceBase
    {
        public enum ServiceState
        {
            SERVICE_STOPPED = 0x00000001,
            SERVICE_START_PENDING = 0x00000002,
            SERVICE_STOP_PENDING = 0x00000003,
            SERVICE_RUNNING = 0x00000004,
            SERVICE_CONTINUE_PENDING = 0x00000005,
            SERVICE_PAUSE_PENDING = 0x00000006,
            SERVICE_PAUSED = 0x00000007,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceStatus
        {
            public long dwServiceType;
            public ServiceState dwCurrentState;
            public long dwControlsAccepted;
            public long dwWin32ExitCode;
            public long dwServiceSpecificExitCode;
            public long dwCheckPoint;
            public long dwWaitHint;
        };

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

        ScreenCaptureService service;
        
        public Service1()
        {
            InitializeComponent();
            //service = new ScreenCaptureService();
            //service.OnStart();

        }
        protected override void OnStart(string[] args)
        {
            /* ServiceStatus serviceStatus = new ServiceStatus();
             serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
             serviceStatus.dwWaitHint = 100000;
             SetServiceStatus(this.ServiceHandle, ref serviceStatus);
             */

            //service = new ScreenCaptureService();
            //Thread dateServerThread = new Thread(new ThreadStart(service.OnStart));
            //dateServerThread.Start();

            WindowsServiceHost.Toolkit.ApplicationLoader.PROCESS_INFORMATION proc;
            WindowsServiceHost.Toolkit.ApplicationLoader.StartProcessAndBypassUAC(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\WindowsConsoleHost.exe", out proc);
        }

        protected override void OnStop()
        {
            //service.OnStop();
            using (var f = System.IO.File.CreateText(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\log2.txt"))
             {
                 var processes = Process.GetProcesses();
                 // set break point here
                 foreach (Process process in processes)
                 {
                     f.WriteLine(process.ProcessName);
                 }
                 processes = Process.GetProcessesByName("WindowsConsoleHost");
                 // set break point here
                 foreach (Process process in processes)
                 {
                     process.Kill();
                 }
             }
        }
        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {

            switch (changeDescription.Reason)
            {
                case SessionChangeReason.SessionLogon:
                    Debug.WriteLine(changeDescription.SessionId + " logon");
                    break;
                case SessionChangeReason.SessionLogoff:
                    Debug.WriteLine(changeDescription.SessionId + " logoff");
                    break;
                case SessionChangeReason.SessionLock:
                    Debug.WriteLine(changeDescription.SessionId + " lock");
                    break;
                case SessionChangeReason.SessionUnlock:
                    Debug.WriteLine(changeDescription.SessionId + " unlock");
                    break;
            }
            Debug.WriteLine(GetUserName(changeDescription.SessionId));
            base.OnSessionChange(changeDescription);
        }
        public static WindowsIdentity GetUserName(int sessionId)
        {
            foreach (Process p in Process.GetProcesses())
            {
                if (p.SessionId == sessionId)
                {
                    return new WindowsIdentity(p.Handle);
                }
            }
            return null;
        }
    }
}
