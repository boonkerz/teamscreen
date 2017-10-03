using Driver.Windows.DfMirage;
using Network;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Common.EventArgs.Network;
using Driver.Windows.Desktop;
using System.IO;
using Network.Manager;

namespace Driver.Windows
{
	public class Display: Interfaces.Display
    {
        
        Interfaces.BaseDisplay Driver;

        private bool _RunningAsService = false;
        private DesktopInfo _DesktopInfo;
        private DateTime LastDeskSwitch = DateTime.Now.AddDays(-10);

        private StreamWriter f = new StreamWriter(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\display.txt", true);

        public Display()
		{
            f.AutoFlush = true;
            _DesktopInfo = new DesktopInfo();
            //_DesktopInfo.SwitchDesktop(Desktops.INPUT);
            _RunningAsService = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToLower().Contains(@"nt authority\system");
            if(!_RunningAsService)
            {
                _RunningAsService = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToLower().Contains(@"nt-autorität\system");
            }
            
            Windows.DfMirage.Display DfMirage = new DfMirage.Display();
            f.WriteLine("Running as Service" + _RunningAsService + "DF:" + DfMirage.DoesMirrorDriverExist());

            if (DfMirage.DoesMirrorDriverExist())
            {
                Driver = DfMirage;
            }
            else
            {
                Driver = new Simple.Display();
            }

        }

        public void RemoveAllClients()
        {
            Driver.RemoveAllClients();
        }

        public void RemoveClient(string clientSystemId)
        {
            Driver.RemoveClient(clientSystemId);
        }

        public void RequestScreenshot(ScreenshotRequestEventArgs e, Boolean fullscreen)
        {
            if (_RunningAsService && (DateTime.Now - LastDeskSwitch).TotalMilliseconds > 1000)
            {//only a program running as under the account     nt authority\system       is allowed to switch desktops
                //Driver.Clear();
                _DesktopInfo.SwitchToInputDesktop();

                LastDeskSwitch = DateTime.Now;
            }
            Driver.RequestScreenshot(e, fullscreen);
        }

        public void SetManager(HostManager hostManager)
        {
            Driver.SetManager(hostManager);
        }
    }
}
