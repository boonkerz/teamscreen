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
            DfMirage = new DfMirage.Display();
            Simple = new Simple.Display();
        }

        public void RequestScreenshot(ScreenshotRequestEventArgs e, HostManager hm, Boolean fullscreen)
        {
            if (DfMirage.DoesMirrorDriverExist())
            {
                DfMirage.RequestScreenshot(e, hm, fullscreen);
            }
            else
            {
                Simple.RequestScreenshot(e, hm, fullscreen);
            }
        }

	}
}
