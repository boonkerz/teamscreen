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
using System.Timers;

namespace Driver.Windows.DfMirage
{

    class Display : BaseDisplay
    {
        DesktopMirror MirrorDriver;

        private List<System.Drawing.Rectangle> DesktopChanges { get; set; }
      
        public Display()
        {
            MirrorDriver = new DesktopMirror();
            DesktopChanges = new List<System.Drawing.Rectangle>();
            MirrorDriver.DesktopChange += new EventHandler<DesktopMirror.DesktopChangeEventArgs>(MirrorDriver_DesktopChange);
            
        }

        public override void SendScreenshot(Boolean fullscreen)
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
                rs.Bounds = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
                rs.HostSystemId = HostManager.SystemId;
                rs.Fullscreen = true;
                rs.Image = stream.ToArray();

                foreach (var ID in ConnectedClients)
                {
                    rs.ClientSystemId = ID;
                    HostManager.sendMessage(rs);
                }
            }
            else if (MirrorDriver.State == DesktopMirror.MirrorState.Running)
            {
                // Send them the list of queued up changes
                var regions = (List<System.Drawing.Rectangle>)GetOptimizedRectangleRegions();

                Bitmap screenshot = MirrorDriver.GetScreen();

                if (fullscreen)
                {
                    MirrorDriver.Stop();
                    MirrorDriver.Start();
                    screenshot.Save(stream, ImageFormat.Png);
                    this.DesktopChanges.Clear();
                    ResponseScreenshotMessage rs = new ResponseScreenshotMessage();
                    rs.Fullscreen = true;
                    rs.Bounds = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
                    rs.HostSystemId = HostManager.SystemId;
                    rs.Image = stream.ToArray();

                    foreach (var ID in ConnectedClients)
                    {
                        rs.ClientSystemId = ID;
                        HostManager.sendMessage(rs);
                    }
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
                    rs.Bounds = regions[i];
                    rs.HostSystemId = HostManager.SystemId;
                    rs.Image = stream.ToArray();

                    foreach (var ID in ConnectedClients)
                    {
                        rs.ClientSystemId = ID;
                        HostManager.sendMessage(rs);
                    }
                }


            }
        }

        private void MirrorDriver_DesktopChange(object sender, DesktopMirror.DesktopChangeEventArgs e)
        {
            var rectangle = new System.Drawing.Rectangle(e.x1, e.y1, e.x2 - e.x1,
                                                e.y2 - e.y1);
            DesktopChanges.Add(rectangle);

        }
        public bool DoesMirrorDriverExist()
        {
            return MirrorDriver.DriverExists();
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
