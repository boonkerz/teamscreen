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

        public void requestScreenshot(HostManager nm)
		{
            var stream = new System.IO.MemoryStream();

            if (MirrorDriver.State != DesktopMirror.MirrorState.Running)
            {
                // Most likely first time
                // Start the mirror driver
                MirrorDriver.Load();

                MirrorDriver.Connect();
                MirrorDriver.Start();

                Bitmap screenshot = MirrorDriver.GetScreen();
                screenshot.Save(stream, ImageFormat.Png);

                return stream.ToArray();
            }
            else if (MirrorDriver.State == DesktopMirror.MirrorState.Running)
            {
                // Send them the list of queued up changes
                var regions = (List<Rectangle>)GetOptimizedRectangleRegions();

                Bitmap screenshot = MirrorDriver.GetScreen();

                if (regions.Count == 0)
                {
                    Network.SendMessage(new ResponseEmptyScreenshotMessage(), NetDeliveryMethod.ReliableOrdered, 0);
                    return;
                }

                for (int i = 0; i < regions.Count; i++)
                {
                    if (regions[i].IsEmpty) continue;

                    Bitmap regionShot = null;

                    try
                    {
                        regionShot = screenshot.Clone(regions[i], PixelFormat.Format16bppRgb565);
                    }
                    catch (OutOfMemoryException ex)
                    {
                        
                    }
                    
                    regionShot.Save(stream, ImageFormat.Png);

                    
                }
            }

            return stream.ToArray();
           
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
