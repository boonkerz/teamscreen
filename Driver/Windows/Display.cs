using Driver.Windows.DfMirage;
using Network;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Common.EventArgs.Network;

namespace Driver.Windows
{
	public class Display : Driver.Interfaces.Display
	{
        
        Simple.Display Simple;
        DfMirage.Display DfMirage;


        public Display()
		{
			
		}

        public void RequestScreenshot(ScreenshotRequestEventArgs e, HostManager hm)
        {
            if (DfMirage.DoesMirrorDriverExist())
            {
                DfMirage.RequestScreenshot(e, hm);
            }
            else
            {
                DfMirage.RequestScreenshot(e, hm);
            }
        }

	}
}
