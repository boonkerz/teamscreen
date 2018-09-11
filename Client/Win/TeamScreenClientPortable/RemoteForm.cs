using Messages.Connection;
using Network.Thread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TeamScreenClientPortable
{
    public partial class RemoteForm : Form
    {
        float Ratio;
        Rectangle Bounds;

        String SystemId { get; set; }
        String Password { get; set; }
        private Pen pen = new Pen(Color.Magenta, 2.0f);

        public ClientThread clientThread;

        protected Utils.Config.Manager ConfigManager;

        delegate void SetDrawingAreaHeightCallback(int Height);
        delegate void DrawImageCallback(Image Image, Rectangle Bounds);
        delegate void setTransferedCallback(String text);
        delegate void CloseRemoteWindowCallback();

        public RemoteForm(String remoteId, String password)
        {
            this.SystemId = remoteId;
            this.Password = password;

            InitializeComponent();
            ConfigManager = new Utils.Config.Manager();
        }

        public void setThread(ClientThread clientThread)
        {
            this.clientThread = clientThread;
        }

        public void Start()
        {
            clientThread.Events.OnScreenshotReceived += Events_OnScreenshotReceived; ;
            //clientThread.Events.OnHostClose += ClientListener_OnHostClose;
            //clientThread.Events.OnReceive += ClientListener_OnReceive;

            clientThread.Manager.sendMessage(new StartScreenSharingMessage { SymmetricKey = clientThread.Manager.getSymmetricKeyForRemoteId(this.SystemId), HostSystemId = this.SystemId, ClientSystemId = clientThread.Manager.SystemId });

        }

        private void Events_OnScreenshotReceived(object sender, Messages.EventArgs.Network.ScreenshotReceivedEventArgs e)
        {
            if (e.Fullscreen)
            {
                Bounds = e.Bounds;
            }

            Ratio = (float)this.drawingArea1.Width / (float)Bounds.Width;

            if (this.drawingArea1.InvokeRequired)
            {
                SetDrawingAreaHeightCallback d = new SetDrawingAreaHeightCallback(setDrawingAreaHeight);
                this.Invoke(d, new object[] { (int)((float)Bounds.Height * Ratio) });
            }
            else
            {
                setDrawingAreaHeight((int)((float)Bounds.Height * Ratio));
            }

            if (e.Nothing)
            {
                return;
            }

            if (e.SystemId == this.SystemId)
            {
                using (var stream = new MemoryStream(e.Image))
                {
                    Image image = Image.FromStream(stream);

                    if (this.drawingArea1.InvokeRequired)
                    {
                        DrawImageCallback d = new DrawImageCallback(drawImage);
                        this.Invoke(d, new object[] { image, e.Bounds });
                    }
                    else
                    {
                        drawImage(image, e.Bounds);
                    }


                }
            }
        }

        protected void setDrawingAreaHeight(int height)
        {
            drawingArea1.Height = height;
            drawingArea1.BackColor = Color.Azure;
        }

        protected void drawImage(Image image, Rectangle bounds)
        {

            if (!drawingArea1.IsDisposed && !drawingArea1.Disposing)
            {
                drawingArea1.Draw(image, bounds);

                var gfx = drawingArea1.CreateGraphics();

                int x = (int)((float)bounds.X * Ratio);
                int y = (int)((float)bounds.Y * Ratio);
                int width = (int)((float)bounds.Width * Ratio);
                int height = (int)((float)bounds.Height * Ratio);

                gfx.DrawLine(pen, new Point(x, y), new Point(x + width, y));
                gfx.DrawLine(pen, new Point(x + width, y), new Point(x + width, y + y));
                gfx.DrawLine(pen, new Point(x + width, y + y), new Point(x, y + y));
                gfx.DrawLine(pen, new Point(x, y + y), new Point(x, y));
                gfx.Dispose();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RemoteForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            clientThread.Manager.sendMessage(new StopScreenSharingMessage { SymmetricKey = clientThread.Manager.getSymmetricKeyForRemoteId(this.SystemId), HostSystemId = this.SystemId, ClientSystemId = clientThread.Manager.SystemId });
        }

        private void drawingArea1_Click(object sender, EventArgs e)
        {

        }

        private void drawingArea1_DoubleClick(object sender, EventArgs e)
        {

        }

        private void drawingArea1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void drawingArea1_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void drawingArea1_MouseMove(object sender, MouseEventArgs e)
        {

        }
    }
}
