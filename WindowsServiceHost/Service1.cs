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
using System.Security.Principal;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceHost
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }
        protected override void OnStart(string[] args)
        {
            WindowsServiceHost.Toolkit.ApplicationLoader.PROCESS_INFORMATION proc;
            WindowsServiceHost.Toolkit.ApplicationLoader.StartProcessAndBypassUAC(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\WindowsConsoleHost.exe", out proc);
        }

        protected override void OnStop()
        {
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
