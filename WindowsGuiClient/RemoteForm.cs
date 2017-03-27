using Common;
using Common.EventArgs.Network;
using Network.Messages.Connection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsGuiClient
{
    public partial class RemoteForm : Form
    {
        String SystemId { get; set; }
        private Pen pen = new Pen(Color.Magenta, 2.0f);
        public ClientThread Manager { get { return Common.Instance.Client.Instance.Thread; } }

        public RemoteForm(String RemoteId)
        {
            this.SystemId = RemoteId;

            InitializeComponent();

            Manager.ClientListener.OnScreenshotReceived += OnScreenshotReceive;

            Manager.Manager.sendMessage(new RequestScreenshotMessage { HostSystemId = this.SystemId, ClientSystemId = Manager.Manager.SystemId });
        }

        protected void OnScreenshotReceive(object sender, ScreenshotReceivedEventArgs e)
        {
            if (e.SystemId == this.SystemId)
            {

                using (var stream = new MemoryStream(e.Image))
                {
                    Image image = Image.FromStream(stream);

                    var gfx = drawingArea1.CreateGraphics();
                    gfx.DrawLine(pen, new Point(e.Bounds.X, e.Bounds.Y), new Point(e.Bounds.X + e.Bounds.Width, e.Bounds.Y));
                    gfx.DrawLine(pen, new Point(e.Bounds.X + e.Bounds.Width, e.Bounds.Y), new Point(e.Bounds.X + e.Bounds.Width, e.Bounds.Y + e.Bounds.Y));
                    gfx.DrawLine(pen, new Point(e.Bounds.X + e.Bounds.Width, e.Bounds.Y + e.Bounds.Y), new Point(e.Bounds.X, e.Bounds.Y + e.Bounds.Y));
                    gfx.DrawLine(pen, new Point(e.Bounds.X, e.Bounds.Y + e.Bounds.Y), new Point(e.Bounds.X, e.Bounds.Y));
                    gfx.Dispose();
                

                    drawingArea1.Draw(image, e.Bounds);
                }

                Thread.Sleep(200);
                Manager.Manager.sendMessage(new RequestScreenshotMessage { HostSystemId = this.SystemId, ClientSystemId = Manager.Manager.SystemId });
            }
        }
    }
}
