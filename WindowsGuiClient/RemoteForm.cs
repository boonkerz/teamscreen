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
        float Ratio;
        Rectangle Bounds;

        String SystemId { get; set; }
        private Pen pen = new Pen(Color.Magenta, 2.0f);

        public ClientThread Manager;

        delegate void SetDrawingAreaHeightCallback(int Height);
        delegate void DrawImageCallback(Image Image, Rectangle Bounds);
        delegate void CloseRemoteWindowCallback();

        public RemoteForm(String RemoteId)
        {
            this.SystemId = RemoteId;

            InitializeComponent();
   
        }

        protected void setDrawingAreaHeight(int height)
        {
            drawingArea1.Height = height;
            drawingArea1.BackColor = Color.Azure;
        }

        protected void drawImage(Image image, Rectangle bounds)
        {
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
            drawingArea1.Draw(image, bounds);
        }

        protected void CloseRemoteWindow()
        {
            this.Close();
        }


        protected void OnScreenshotReceive(object sender, ScreenshotReceivedEventArgs e)
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

        internal void setManager(ClientThread manager)
        {
            this.Manager = manager;

            Manager.ClientListener.OnScreenshotReceived += OnScreenshotReceive;
            Manager.ClientListener.OnHostClose += ClientListener_OnHostClose;

            Manager.Manager.sendMessage(new RequestScreenshotMessage { HostSystemId = this.SystemId, ClientSystemId = Manager.Manager.SystemId, Fullscreen = true });
        }

        private void ClientListener_OnHostClose(object sender, Common.EventArgs.Network.Client.HostCloseEventArgs e)
        {
            if (this.InvokeRequired)
            {
                CloseRemoteWindowCallback d = new CloseRemoteWindowCallback(CloseRemoteWindow);
                this.Invoke(d);
            }
            else
            {
                CloseRemoteWindow();
            }
        }

        private void drawingArea1_Click(object sender, EventArgs e)
        {

        }

        private void drawingArea1_MouseMove_1(object sender, MouseEventArgs e)
        {
            Manager.Manager.sendMessage(new Network.Messages.Connection.OneWay.MouseMoveMessage { ClientSystemId = Manager.Manager.SystemId, HostSystemId = this.SystemId, X = (e.X / Ratio), Y = (e.Y / Ratio) });
        }

        private void drawingArea1_MouseClick(object sender, MouseEventArgs e)
        {

            /*switch(e.Button)
            {
                case MouseButtons.Left:
                    Manager.Manager.sendMessage(new Network.Messages.Connection.OneWay.MouseClickMessage { Button = Network.Messages.Connection.OneWay.MouseClickMessage.ButtonType.Left, ClientSystemId = Manager.Manager.SystemId, HostSystemId = this.SystemId, X = (int)(e.X / Ratio), Y = (int)(e.Y / Ratio) });
                    break;
                case MouseButtons.Middle:
                    Manager.Manager.sendMessage(new Network.Messages.Connection.OneWay.MouseClickMessage { Button = Network.Messages.Connection.OneWay.MouseClickMessage.ButtonType.Middle, ClientSystemId = Manager.Manager.SystemId, HostSystemId = this.SystemId, X = (int)(e.X / Ratio), Y = (int)(e.Y / Ratio) });
                    break;
                case MouseButtons.Right:
                    Manager.Manager.sendMessage(new Network.Messages.Connection.OneWay.MouseClickMessage { Button = Network.Messages.Connection.OneWay.MouseClickMessage.ButtonType.Right, ClientSystemId = Manager.Manager.SystemId, HostSystemId = this.SystemId, X = (int)(e.X / Ratio), Y = (int)(e.Y / Ratio) });
                    break;
            }*/
            

        }

        private void drawingArea1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    Manager.Manager.sendMessage(new Network.Messages.Connection.OneWay.MouseClickMessage { DoubleClick = true, Button = Network.Messages.Connection.OneWay.MouseClickMessage.ButtonType.Left, ClientSystemId = Manager.Manager.SystemId, HostSystemId = this.SystemId, X = (int)(e.X / Ratio), Y = (int)(e.Y / Ratio) });
                    break;
                case MouseButtons.Middle:
                    Manager.Manager.sendMessage(new Network.Messages.Connection.OneWay.MouseClickMessage { DoubleClick = true, Button = Network.Messages.Connection.OneWay.MouseClickMessage.ButtonType.Middle, ClientSystemId = Manager.Manager.SystemId, HostSystemId = this.SystemId, X = (int)(e.X / Ratio), Y = (int)(e.Y / Ratio) });
                    break;
                case MouseButtons.Right:
                    Manager.Manager.sendMessage(new Network.Messages.Connection.OneWay.MouseClickMessage { DoubleClick = true, Button = Network.Messages.Connection.OneWay.MouseClickMessage.ButtonType.Right, ClientSystemId = Manager.Manager.SystemId, HostSystemId = this.SystemId, X = (int)(e.X / Ratio), Y = (int)(e.Y / Ratio) });
                    break;
            }
        }

        private void drawingArea1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            Manager.Manager.sendMessage(new Network.Messages.Connection.OneWay.KeyMessage { Key = (uint)e.KeyValue, ClientSystemId = Manager.Manager.SystemId, HostSystemId = this.SystemId, Alt = e.Alt, Control = e.Control, Shift = e.Shift, Mode = Network.Messages.Connection.OneWay.KeyMessage.KeyMode.Down });
        }

        private void drawingArea1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {

        }

        private void drawingArea1_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            Manager.Manager.sendMessage(new Network.Messages.Connection.OneWay.KeyMessage { Key = (uint)e.KeyValue, ClientSystemId = Manager.Manager.SystemId, HostSystemId = this.SystemId, Alt = e.Alt, Control = e.Control, Shift = e.Shift, Mode = Network.Messages.Connection.OneWay.KeyMessage.KeyMode.Up });
        }

        private void drawingArea1_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    Manager.Manager.sendMessage(new Network.Messages.Connection.OneWay.MouseClickMessage { Down = true, Button = Network.Messages.Connection.OneWay.MouseClickMessage.ButtonType.Left, ClientSystemId = Manager.Manager.SystemId, HostSystemId = this.SystemId, X = (int)(e.X / Ratio), Y = (int)(e.Y / Ratio) });
                    break;
                case MouseButtons.Middle:
                    Manager.Manager.sendMessage(new Network.Messages.Connection.OneWay.MouseClickMessage { Down = true, Button = Network.Messages.Connection.OneWay.MouseClickMessage.ButtonType.Middle, ClientSystemId = Manager.Manager.SystemId, HostSystemId = this.SystemId, X = (int)(e.X / Ratio), Y = (int)(e.Y / Ratio) });
                    break;
                case MouseButtons.Right:
                    Manager.Manager.sendMessage(new Network.Messages.Connection.OneWay.MouseClickMessage { Down = true, Button = Network.Messages.Connection.OneWay.MouseClickMessage.ButtonType.Right, ClientSystemId = Manager.Manager.SystemId, HostSystemId = this.SystemId, X = (int)(e.X / Ratio), Y = (int)(e.Y / Ratio) });
                    break;
            }
        }

        private void drawingArea1_MouseUp(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    Manager.Manager.sendMessage(new Network.Messages.Connection.OneWay.MouseClickMessage { Up = true, Button = Network.Messages.Connection.OneWay.MouseClickMessage.ButtonType.Left, ClientSystemId = Manager.Manager.SystemId, HostSystemId = this.SystemId, X = (int)(e.X / Ratio), Y = (int)(e.Y / Ratio) });
                    break;
                case MouseButtons.Middle:
                    Manager.Manager.sendMessage(new Network.Messages.Connection.OneWay.MouseClickMessage { Up = true, Button = Network.Messages.Connection.OneWay.MouseClickMessage.ButtonType.Middle, ClientSystemId = Manager.Manager.SystemId, HostSystemId = this.SystemId, X = (int)(e.X / Ratio), Y = (int)(e.Y / Ratio) });
                    break;
                case MouseButtons.Right:
                    Manager.Manager.sendMessage(new Network.Messages.Connection.OneWay.MouseClickMessage { Up = true, Button = Network.Messages.Connection.OneWay.MouseClickMessage.ButtonType.Right, ClientSystemId = Manager.Manager.SystemId, HostSystemId = this.SystemId, X = (int)(e.X / Ratio), Y = (int)(e.Y / Ratio) });
                    break;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Manager.Manager.sendMessage(new Network.Messages.Connection.Request.CloseHostConnectionMessage() { ClientSystemId = Manager.Manager.SystemId, HostSystemId = this.SystemId });
        }

        private void RemoteForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Manager.Manager.sendMessage(new Network.Messages.Connection.Request.CloseHostConnectionMessage() { ClientSystemId = Manager.Manager.SystemId, HostSystemId = this.SystemId });
        }
    }
}
