using Common.EventArgs.Network;
using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driver.Windows.DfMirage
{

    class Display
    {
        DesktopMirror MirrorDriver;

        Display()
        {
            MirrorDriver = new DesktopMirror();
        }

        public bool DoesMirrorDriverExist()
        {
            return MirrorDriver.DriverExists();
        }

        public void RequestScreenshot(ScreenshotRequestEventArgs e, HostManager hm)
        {
        }
    }
}
