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

		public BaseDisplay() : base()
        {
            refreshThread = new System.Timers.Timer(1000);
            refreshThread.Elapsed += RefreshThread_Elapsed;
        }

        private void RefreshThread_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.SendScreenshot(true);
        }

        public void Clear()
        {

        }

        public void SetManager(HostManager hostManager)
        {
            HostManager = hostManager;
        }

        public virtual void RequestScreenshot(ScreenshotRequestEventArgs e, bool fullscreen)
        {
            if (!this.ConnectedClients.Contains(e.ClientSystemId))
            {
                this.ConnectedClients.Add(e.ClientSystemId);
            }

            refreshThread.Start();
        }

        public virtual void SendScreenshot(Boolean fullscreen)
        {
            
        }

        public void RemoveClient(string clientSystemId)
        {
            this.ConnectedClients.Remove(clientSystemId);
        }

        public void RemoveAllClients()
        {
            this.ConnectedClients.Clear();
            this.refreshThread.Stop();
        }
    }
}
