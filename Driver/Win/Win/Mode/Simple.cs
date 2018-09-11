using Driver.Win.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SixLabors.ImageSharp;
using System.Threading.Tasks;
using SixLabors.ImageSharp.PixelFormats;

namespace Driver.Win.Mode
{
    public class Simple
    {
        
        System.Drawing.Rectangle bounds;
        private ScreenCapture _ScreenCapture;
        public RawImage _LastImage = null;
        public Image<Rgba32> image;

        public Simple()
        {
            bounds = System.Windows.Forms.Screen.GetBounds(System.Drawing.Point.Empty);
            _ScreenCapture = new ScreenCapture(80);
        }

        public int getScreenHeight()
        {
            return bounds.Height;
        }

        public int getScreenWidth()
        {
            return bounds.Width;
        }

        public void Clear()
        {
            _ScreenCapture.ReleaseHandles();
        }
       
        public RawImage createScreenshot(Boolean fullscreen)
        {

            RawImage img = _ScreenCapture.GetScreen(new System.Drawing.Size(getScreenWidth(),getScreenHeight()));

            return img;
        }
    }
}
