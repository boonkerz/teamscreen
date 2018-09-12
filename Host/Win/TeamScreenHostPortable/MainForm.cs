using Messages.EventArgs.Network;
using Network.Thread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TeamScreenHostPortable
{
    public partial class MainForm : Form
    {

        public HostThread hostThread { get { return Network.Instance.Host.Instance.Thread; } }

        protected Utils.Config.Manager ConfigManager;

        protected Driver.Win.Mouse mouse = new Driver.Win.Mouse();
        protected Driver.Win.Keyboard keyboard = new Driver.Win.Keyboard();
        protected Driver.Win.Display display = new Driver.Win.Display();

        delegate void SetSystemIdAndPasswordCallback(String SystemId, String Password);

        protected System.Timers.Timer connectionStatus;

        public MainForm()
        {
            InitializeComponent();
            
            connectionStatus = new System.Timers.Timer(1000);

            connectionStatus.Elapsed += Connection_Elapsed;

            ConfigManager = new Utils.Config.Manager();

            hostThread.Events.OnConnected += new EventHandler<ConnectedEventArgs>(Network_OnConnected);
            
            hostThread.Events.OnClientInitalizeConnected += (object sender, Messages.EventArgs.Network.Host.ClientInitalizeConnectedEventArgs e) =>
            {
                this.lblStatus.Text = "Initaliziere Verbindung";
            };
            
            hostThread.Events.OnClientConnected += (object sender, ClientConnectedEventArgs e) =>
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

            this.txtServer.Text = ConfigManager.HostConfig.ServerName;
            this.txtPort.Text = ConfigManager.HostConfig.ServerPort.ToString();
            this.txtSystemId.Text = ConfigManager.HostConfig.SystemId;
            this.txtPassword.Text = ConfigManager.HostConfig.Password;

            hostThread.Events.onPeerConnected += HostListener_onPeerConnected;
            hostThread.Events.onPeerDisconnected += HostListener_onPeerDisconnected;
            hostThread.Events.onNetworkError += HostListener_onNetworkError;
            hostThread.Events.OnStartScreenSharing += Events_OnStartScreenSharing;
            hostThread.Events.OnStopScreenSharing += Events_OnStopScreenSharing;

            hostThread.Events.OnMouseMove += Events_OnMouseMove;
            hostThread.Events.OnMouseClick += Events_OnMouseClick;
            hostThread.Events.OnKey += Events_OnKey;
            hostThread.Events.OnClientAlive += Events_OnClientAlive;

            display.SetHostManager(hostThread.Manager);

            hostThread.Start();
        }

        private void Events_OnClientAlive(object sender, ClientAliveEventArgs e)
        {
            display.AliveScreenSharing(e.ClientSystemId);
        }

        private void Events_OnKey(object sender, Messages.EventArgs.Network.KeyEventArgs e)
        {
            if (e.Mode == Messages.Connection.OneWay.KeyMessage.KeyMode.Down)
            {
                keyboard.Down(e.Key);
            }
            else
            {
                keyboard.Up(e.Key);
            }
        }

        private void Events_OnMouseClick(object sender, MouseClickEventArgs e)
        {
            if (e.DoubleClick)
            {
                switch (e.Button)
                {
                    case MouseClickEventArgs.ButtonType.Left:
                        mouse.DoubleClickLeft((int)e.X, (int)e.Y);
                        break;
                    case MouseClickEventArgs.ButtonType.Middle:
                        mouse.DoubleClickMiddle((int)e.X, (int)e.Y);
                        break;
                    case MouseClickEventArgs.ButtonType.Right:
                        mouse.DoubleClickRight((int)e.X, (int)e.Y);
                        break;
                }

                return;
            }
            if (e.Down)
            {
                switch (e.Button)
                {
                    case MouseClickEventArgs.ButtonType.Left:
                        mouse.ClickDownLeft((int)e.X, (int)e.Y);
                        break;
                    case MouseClickEventArgs.ButtonType.Middle:
                        mouse.ClickDownMiddle((int)e.X, (int)e.Y);
                        break;
                    case MouseClickEventArgs.ButtonType.Right:
                        mouse.ClickDownRight((int)e.X, (int)e.Y);
                        break;
                }

                return;
            }
            if (e.Up)
            {
                switch (e.Button)
                {
                    case MouseClickEventArgs.ButtonType.Left:
                        mouse.ClickUpLeft((int)e.X, (int)e.Y);
                        break;
                    case MouseClickEventArgs.ButtonType.Middle:
                        mouse.ClickUpMiddle((int)e.X, (int)e.Y);
                        break;
                    case MouseClickEventArgs.ButtonType.Right:
                        mouse.ClickUpRight((int)e.X, (int)e.Y);
                        break;
                }

                return;
            }


            switch (e.Button)
            {
                case MouseClickEventArgs.ButtonType.Left:
                    mouse.ClickLeft((int)e.X, (int)e.Y);
                    break;
                case MouseClickEventArgs.ButtonType.Middle:
                    mouse.ClickMiddle((int)e.X, (int)e.Y);
                    break;
                case MouseClickEventArgs.ButtonType.Right:
                    mouse.ClickRight((int)e.X, (int)e.Y);
                    break;
            }
        }

        private void Events_OnMouseMove(object sender, MouseMoveEventArgs e)
        {
            mouse.Move((int)e.X, (int)e.Y);
        }

        private void Events_OnStopScreenSharing(object sender, StopScreenSharingEventArgs e)
        {
            display.StopScreenSharing(e.ClientSystemId);
        }

        private void Events_OnStartScreenSharing(object sender, StartScreenSharingEventArgs e)
        {
            display.StartScreenSharing(e.ClientSystemId);
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Connection_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            hostThread.Reconnect();
        }

        private void HostListener_onNetworkError(object sender, EventArgs e)
        {
            this.lblStatus.Text = "Network Error";
            connectionStatus.Start();
        }
        
        private void HostListener_onPeerDisconnected(object sender, EventArgs e)
        {
            this.lblStatus.Text = "Broker Disconnected";
            connectionStatus.Start();
        }
        
        private void HostListener_onPeerConnected(object sender, EventArgs e)
        {
            this.lblStatus.Text = "Broker Connected";
            connectionStatus.Stop();
        }

        void Network_OnConnected(object sender, ConnectedEventArgs e)
        {
            if (this.txtSystemId.InvokeRequired)
            {
                SetSystemIdAndPasswordCallback d = new SetSystemIdAndPasswordCallback(SetSystemIdAndPassword);
                this.Invoke(d, new object[] { e.SystemId, hostThread.Manager.Password });
            }
            else
            {
                SetSystemIdAndPassword(e.SystemId, hostThread.Manager.Password);
            }
        }

        protected void SetSystemIdAndPassword(String SystemId, String Password)
        {
            this.txtSystemId.Text = SystemId;
            this.txtPassword.Text = Password;
        }

        protected void saveSettingsInConfig()
        {
            if(this.txtSystemId.Text != "")
            {
                ConfigManager.HostConfig.SystemId = this.txtSystemId.Text;
                ConfigManager.HostConfig.Password = this.txtPassword.Text;
            }
            
            ConfigManager.HostConfig.ServerName = this.txtServer.Text;
            ConfigManager.HostConfig.ServerPort = Convert.ToInt32(this.txtPort.Text);
            ConfigManager.saveHostConfig();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveSettingsInConfig();
            hostThread.Manager.SystemId = this.txtSystemId.Text;
            hostThread.Manager.Password = this.txtPassword.Text;
            hostThread.Reconnect();
        }
    }
}
