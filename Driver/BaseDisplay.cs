using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.EventArgs.Network;
using Network;
using Network.Messages.Connection;

namespace Driver
{
    public class BaseDisplay : Interfaces.BaseDisplay
    {

        protected List<String> ConnectedClients = new List<string>();

        protected HostManager HostManager;

        protected System.Timers.Timer refreshThread;

        public BaseDisplay()
        {
            refreshThread = new System.Timers.Timer(200);
            refreshThread.Elapsed += RefreshThread_Elapsed;
        }

        private void RefreshThread_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.SendScreenshot(false);
        }

        public void SetManager(HostManager hostManager)
        {
            HostManager = hostManager;
        }

        public virtual void RequestScreenshot(ScreenshotRequestEventArgs e, bool fullscreen)
        {
            this.ConnectedClients.Add(e.ClientSystemId);

            refreshThread.Start();
        }

        public virtual void SendScreenshot(Boolean fullscreen)
        {
            
        }
        
    }
}
