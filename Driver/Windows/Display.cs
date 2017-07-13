using Driver.Windows.DfMirage;
using Network;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Common.EventArgs.Network;
using Driver.Windows.Desktop;

namespace Driver.Windows
{
	public class Display: Interfaces.Display
    {
        
        Interfaces.BaseDisplay Driver;

        private bool _RunningAsService = false;
        private DesktopInfo _DesktopInfo;
        private DateTime LastDeskSwitch = DateTime.Now.AddDays(-10);

        public Display()
		{
            _DesktopInfo = new DesktopInfo();
            _RunningAsService = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToLower().Contains(@"nt authority\system");
            Windows.DfMirage.Display DfMirage = new DfMirage.Display();
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
            if (_RunningAsService && (DateTime.Now - LastDeskSwitch).TotalMilliseconds > 500)
            {//only a program running as under the account     nt authority\system       is allowed to switch desktops
                var d = _DesktopInfo.GetActiveDesktop();
                //_ScreenCapture.ReleaseHandles();//make sure to release any handles before switching
                if (d != _DesktopInfo.Current_Desktop)
                {
                    _DesktopInfo.SwitchDesktop(d);
                }
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
