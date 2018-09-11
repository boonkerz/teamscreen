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

            display.SetHostManager(hostThread.Manager);

            hostThread.Start();
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
            //Display.RemoveAllClients();
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
            hostThread.Reconnect();
        }
    }
}
