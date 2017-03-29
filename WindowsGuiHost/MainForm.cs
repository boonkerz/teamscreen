using Common.EventArgs.Network;
using Common.Thread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsGuiHost
{
    public partial class MainForm : Form
    {
        public HostThread Manager { get { return Common.Instance.Host.Instance.Thread; } }

        protected Common.Config.Manager ConfigManager;

        public Driver.Interfaces.Mouse Mouse { get { return Driver.Manager.Instance.Mouse; } }
        public Driver.Interfaces.Keyboard Keyboard { get { return Driver.Manager.Instance.Keyboard; } }
        public Driver.Interfaces.Display Display { get { return Driver.Manager.Instance.Display; } }

        delegate void SetSystemIdAndPasswordCallback(String SystemId, String Password);



        public MainForm()
        {
            InitializeComponent();
            ConfigManager = new Common.Config.Manager();

            Manager.HostListener.OnConnected += new EventHandler<ConnectedEventArgs>(Network_OnConnected);

            Manager.HostListener.OnClientInitalizeConnected += (object sender, Common.EventArgs.Network.Host.ClientInitalizeConnectedEventArgs e) =>
            {
                this.lblStatus.Text = "Initaliziere Verbindung";
                var pair = Manager.Manager.CreateNewKeyPairKey(e.ClientSystemId);

                Network.Messages.Connection.Response.InitalizeHostConnectionMessage rs = new Network.Messages.Connection.Response.InitalizeHostConnectionMessage();
                rs.HostSystemId = Manager.Manager.SystemId;
                rs.ClientSystemId = e.ClientSystemId;
                rs.PublicKey = pair.PublicKey;

                Manager.Manager.sendMessage(rs);
            };

            Manager.HostListener.OnClientConnected += (object sender, ClientConnectedEventArgs e) =>
            {
                if (e.PasswordOk)
                {
                    this.lblStatus.Text = "Passwort Ok Verbunden mit: " + e.SystemId;
                }
                else
                {
                    this.lblStatus.Text = "Passwort Falsch Verbindung abgebrochen von: " + e.SystemId;
                }
            };
            Manager.HostListener.OnMouseMove += (object sender, MouseMoveEventArgs e) =>
            {
                Mouse.Move((int)e.X, (int)e.Y);
            };
            Manager.HostListener.OnKey += (object sender, Common.EventArgs.Network.KeyEventArgs e) =>
            {
                if(e.Mode == Network.Messages.Connection.OneWay.KeyMessage.KeyMode.Down)
                {
                    Keyboard.Down(e.Key);
                }
                else
                {
                    Keyboard.Up(e.Key);
                }
                
            };
            Manager.HostListener.OnMouseClick += (object sender, MouseClickEventArgs e) =>
            {
                if(e.DoubleClick)
                {
                    switch (e.Button)
                    {
                        case MouseClickEventArgs.ButtonType.Left:
                            Mouse.DoubleClickLeft((int)e.X, (int)e.Y);
                            break;
                        case MouseClickEventArgs.ButtonType.Middle:
                            Mouse.DoubleClickMiddle((int)e.X, (int)e.Y);
                            break;
                        case MouseClickEventArgs.ButtonType.Right:
                            Mouse.DoubleClickRight((int)e.X, (int)e.Y);
                            break;
                    }

                    return;
                }
                if (e.Down)
                {
                    switch (e.Button)
                    {
                        case MouseClickEventArgs.ButtonType.Left:
                            Mouse.ClickDownLeft((int)e.X, (int)e.Y);
                            break;
                        case MouseClickEventArgs.ButtonType.Middle:
                            Mouse.ClickDownMiddle((int)e.X, (int)e.Y);
                            break;
                        case MouseClickEventArgs.ButtonType.Right:
                            Mouse.ClickDownRight((int)e.X, (int)e.Y);
                            break;
                    }

                    return;
                }
                if (e.Up)
                {
                    switch (e.Button)
                    {
                        case MouseClickEventArgs.ButtonType.Left:
                            Mouse.ClickUpLeft((int)e.X, (int)e.Y);
                            break;
                        case MouseClickEventArgs.ButtonType.Middle:
                            Mouse.ClickUpMiddle((int)e.X, (int)e.Y);
                            break;
                        case MouseClickEventArgs.ButtonType.Right:
                            Mouse.ClickUpRight((int)e.X, (int)e.Y);
                            break;
                    }

                    return;
                }


                switch (e.Button)
                {
                    case MouseClickEventArgs.ButtonType.Left:
                        Mouse.ClickLeft((int)e.X, (int)e.Y);
                        break;
                    case MouseClickEventArgs.ButtonType.Middle:
                        Mouse.ClickMiddle((int)e.X, (int)e.Y);
                        break;
                    case MouseClickEventArgs.ButtonType.Right:
                        Mouse.ClickRight((int)e.X, (int)e.Y);
                        break;
                }

            };
            Manager.HostListener.OnScreenshotRequest += (object sender, ScreenshotRequestEventArgs e) =>
            {
                Display.RequestScreenshot(e, this.Manager.Manager, e.Fullscreen);
            };
            Manager.start();
        }

        protected void SetSystemIdAndPassword(String SystemId, String Password)
        {
            this.txtSystemId.Text = SystemId;
            this.txtPassword.Text = Password;
        }

        void Network_OnConnected(object sender, ConnectedEventArgs e)
        {
            if (this.txtSystemId.InvokeRequired)
            {
                SetSystemIdAndPasswordCallback d = new SetSystemIdAndPasswordCallback(SetSystemIdAndPassword);
                this.Invoke(d, new object[] { e.SystemId, Manager.Manager.Password });
            }
            else
            {
                SetSystemIdAndPassword(e.SystemId, Manager.Manager.Password);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ConfigManager.HostConfig.SystemId = this.txtSystemId.Text;
            ConfigManager.HostConfig.Password = this.txtPassword.Text;
            ConfigManager.saveHostConfig();
            Manager.Manager.Password = this.txtPassword.Text;
        }
    }
}
