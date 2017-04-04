using Driver.Mac;
using Network;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Common.EventArgs.Network;

namespace Driver.Mac
{
	public class Display: Interfaces.Display
    {
        
        Interfaces.BaseDisplay Driver;

        public Display()
		{
            
            Driver = new Simple.Display();
            
            
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
            
            Driver.RequestScreenshot(e, fullscreen);
        }

        public void SetManager(HostManager hostManager)
        {
            Driver.SetManager(hostManager);
        }
    }
}
