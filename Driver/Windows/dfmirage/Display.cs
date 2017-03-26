using Common.EventArgs.Network;
using Network;
using Network.Messages.Connection;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Driver.Windows.DfMirage
{

    class Display
    {
        DesktopMirror MirrorDriver;

        private List<System.Drawing.Rectangle> DesktopChanges { get; set; }
        public Display()
        {
            MirrorDriver = new DesktopMirror();
            DesktopChanges = new List<System.Drawing.Rectangle>();
            MirrorDriver.DesktopChange += new EventHandler<DesktopMirror.DesktopChangeEventArgs>(MirrorDriver_DesktopChange);
        }

        private void MirrorDriver_DesktopChange(object sender, DesktopMirror.DesktopChangeEventArgs e)
        {
            var rectangle = new System.Drawing.Rectangle(e.Region.x1, e.Region.y1, e.Region.x2 - e.Region.x1,
                                                e.Region.y2 - e.Region.y1);
            DesktopChanges.Add(rectangle);

        }
        public bool DoesMirrorDriverExist()
        {
            return MirrorDriver.DriverExists();
        }

        public void RequestScreenshot(ScreenshotRequestEventArgs e, HostManager hm)
        {

            var stream = new System.IO.MemoryStream();

            if (MirrorDriver.State != DesktopMirror.MirrorState.Running)
            {
                stream = new System.IO.MemoryStream();
                // Most likely first time
                // Start the mirror driver
                MirrorDriver.Load();

                MirrorDriver.Connect();
                MirrorDriver.Start();

                Bitmap screenshot = MirrorDriver.GetScreen();
                screenshot.Save(stream, ImageFormat.Png);

                ResponseScreenshotMessage rs = new ResponseScreenshotMessage();
                rs.HostSystemId = e.HostSystemId;
                rs.ClientSystemId = e.ClientSystemId;
                rs.Bounds = Screen.PrimaryScreen.Bounds;
                rs.Image = stream.ToArray();

                hm.sendMessage(rs);
            }
            else if (MirrorDriver.State == DesktopMirror.MirrorState.Running)
            {
                // Send them the list of queued up changes
                var regions = (List<System.Drawing.Rectangle>)GetOptimizedRectangleRegions();

                Bitmap screenshot = MirrorDriver.GetScreen();

                if (regions.Count == 0)
                {
                    ResponseEmptyScreenshotMessage rs = new ResponseEmptyScreenshotMessage();
                    rs.HostSystemId = e.HostSystemId;
                    rs.ClientSystemId = e.ClientSystemId;

                    hm.sendMessage(rs);
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
                    stream = new System.IO.MemoryStream();

                    regionShot.Save(stream, ImageFormat.Png);

                    ResponseScreenshotMessage rs = new ResponseScreenshotMessage();
                    rs.HostSystemId = e.HostSystemId;
                    rs.ClientSystemId = e.ClientSystemId;
                    rs.Bounds = regions[i];
                    rs.Image = stream.ToArray();

                    hm.sendMessage(rs);
                }


            }
        }
        /// <summary>
        /// Combines intersecting rectangles to reduce redundant sends.
        /// </summary>
        /// <returns></returns>
        public IList<System.Drawing.Rectangle> GetOptimizedRectangleRegions()
        {

            var desktopChangesCopy = new List<System.Drawing.Rectangle>(DesktopChanges);
            var desktopChangesCopyFound = new List<System.Drawing.Rectangle>();
            DesktopChanges.Clear();

            desktopChangesCopy.ForEach((x) => desktopChangesCopy.ForEach((y) =>
            {
                if (x != y && x.Contains(y))
                {
                    desktopChangesCopyFound.Add(y);
                }
            }));

            desktopChangesCopyFound.ForEach((y) =>
            {
                desktopChangesCopy.Remove(y);
            });
            

            return desktopChangesCopy;
        }

    }
}
