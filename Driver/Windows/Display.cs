using Driver.Windows.DfMirage;
using Network;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Common.EventArgs.Network;

namespace Driver.Windows
{
	public class Display: Interfaces.Display
    {
        
        Interfaces.BaseDisplay Driver;

        public Display()
		{
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

        public void RemoveClient(string clientSystemId)
        {
            Driver.RemoveClient(clientSystemId);
        }

        public void RequestScreenshot(ScreenshotRequestEventArgs e, Boolean fullscreen)
        {
            
            Driver.RequestScreenshot(e, fullscreen);
        }

        public void SetManager(HostManager hostManager)
        {
            Driver.SetManager(hostManager);
        }
    }
}
