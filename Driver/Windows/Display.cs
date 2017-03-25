using Driver.Windows.DfMirage;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Driver.Windows
{
	public class Display : Driver.Interfaces.Display
	{
        DesktopMirror MirrorDriver;


        public Display()
		{
			MirrorDriver = new DesktopMirror();
		}

        public bool DoesMirrorDriverExist()
        {
            return MirrorDriver.DriverExists();
        }

        public makeScreenshot()
		{

            if(DoesMirrorDriverExist())
            {



            }

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
            /*
            byte[] image = new byte[] { };

			var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
							   Screen.PrimaryScreen.Bounds.Height,
							   PixelFormat.Format32bppArgb);

			// Create a graphics object from the bitmap.
			var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

			// Take the screenshot from the upper left corner to the right bottom corner.
			gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
										Screen.PrimaryScreen.Bounds.Y,
										0,
										0,
										Screen.PrimaryScreen.Bounds.Size,
										CopyPixelOperation.SourceCopy);

			ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);

			// Create an Encoder object based on the GUID
			// for the Quality parameter category.
			System.Drawing.Imaging.Encoder myEncoder =
				System.Drawing.Imaging.Encoder.Quality;

			// Create an EncoderParameters object.
			// An EncoderParameters object has an array of EncoderParameter
			// objects. In this case, there is only one
			// EncoderParameter object in the array.
			EncoderParameters myEncoderParameters = new EncoderParameters(1);

			EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);
			myEncoderParameters.Param[0] = myEncoderParameter;

			bmpScreenshot.Save(stream, jpgEncoder, myEncoderParameters);

			image = stream.ToArray();
			return image;
            */
        }

		private ImageCodecInfo GetEncoder(ImageFormat format)
		{

			ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

			foreach (ImageCodecInfo codec in codecs)
			{
				if (codec.FormatID == format.Guid)
				{
					return codec;
				}
			}
			return null;
		}
	}
}
