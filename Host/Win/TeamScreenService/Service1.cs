using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace TeamScreenService
{
    partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            TeamScreenService.Toolkit.ApplicationLoader.PROCESS_INFORMATION proc;
            TeamScreenService.Toolkit.ApplicationLoader.StartProcessAndBypassUAC(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\TeamScreenHostConsole.exe", out proc);
        }

        protected override void OnStop()
        {
            var processes = Process.GetProcessesByName("TeamScreenHostConsole");
            // set break point here
            foreach (Process process in processes)
            {
                process.Kill();
            }
        }

    }
}
