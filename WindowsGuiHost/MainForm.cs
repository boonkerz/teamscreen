using Common.EventArgs.Network;
using Common.Thread;
using Network.Messages.Connection.OneWay;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
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

        protected System.Timers.Timer connectionStatus;


        public MainForm()
        {
            InitializeComponent();

            connectionStatus = new System.Timers.Timer(1000);
            connectionStatus.Elapsed += Connection_Elapsed;

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
            Manager.HostListener.OnScreenshotRequest += HostListener_OnScreenshotRequest;
            Manager.HostListener.OnClientClose += HostListener_OnClientClose;
            Display.SetManager(Manager.Manager);
            Manager.HostListener.onPeerConnected += HostListener_onPeerConnected;
            Manager.HostListener.onPeerDisconnected += HostListener_onPeerDisconnected;
            Manager.HostListener.onNetworkError += HostListener_onNetworkError;

            Manager.Start();
        }

        private void HostListener_OnScreenshotRequest(object sender, ScreenshotRequestEventArgs e)
        {
            Display.RequestScreenshot(e, e.Fullscreen);
        }

        private void HostListener_OnClientClose(object sender, Common.EventArgs.Network.Host.ClientCloseEventArgs e)
        {
            Display.RemoveClient(e.ClientSystemId);
            Network.Messages.Connection.Response.CloseHostConnectionMessage rs = new Network.Messages.Connection.Response.CloseHostConnectionMessage();
            rs.HostSystemId = Manager.Manager.SystemId;
            rs.ClientSystemId = e.ClientSystemId;

            Manager.Manager.sendMessage(rs);
        }

        private void Connection_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Display.RemoveAllClients();
            Manager.Reconnect();
        }

        private void HostListener_onNetworkError(object sender, EventArgs e)
        {
            this.lblStatus.Text = "Network Error";
            connectionStatus.Start();
        }

        private void HostListener_onPeerDisconnected(object sender, EventArgs e)
        {
            this.lblStatus.Text = "Introducer Disconnected";
            connectionStatus.Start();
        }

        private void HostListener_onPeerConnected(object sender, EventArgs e)
        {
            this.lblStatus.Text = "Introducer Connected";
            connectionStatus.Stop();
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
            Manager.Manager.SystemId = this.txtSystemId.Text;
            Manager.Manager.Password = this.txtPassword.Text;
            Manager.Manager.sendMessage(new DisconnectFromIntroducerMessage() { SystemId = Manager.Manager.SystemId });
            Thread.Sleep(100);
            Manager.Stop();
            Manager.Start();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Manager.Manager.sendMessage(new DisconnectFromIntroducerMessage() { SystemId = Manager.Manager.SystemId }) ;
            Thread.Sleep(100);
        }
    }
}
