using Common;using Common.EventArgs.Network;using Network.Messages.Connection;
using System;using System.Collections.Generic;using System.ComponentModel;using System.Data;using System.Drawing;using System.Linq;using System.Text;using System.Threading;using System.Threading.Tasks;using System.Windows.Forms;namespace WindowsGuiClient{    public partial class MainForm : Form    {        public ClientThread Manager;        protected Common.Config.Manager ConfigManager;        protected System.Timers.Timer connectionStatus;        public delegate void ShowFormCallback(String systemId);        System.Timers.Timer onlineCheckTimer;        public MainForm(ClientThread manager)        {            this.Manager = manager;            InitializeComponent();            connectionStatus = new System.Timers.Timer(1000);            connectionStatus.Elapsed += Connection_Elapsed;            onlineCheckTimer = new System.Timers.Timer(5000);            onlineCheckTimer.Elapsed += OnlineCheckTimer_Elapsed;            ConfigManager = new Common.Config.Manager();            Manager.ClientListener.OnHostInitalizeConnected += (object sender, Common.EventArgs.Network.Client.HostInitalizeConnectedEventArgs e) =>            {                Manager.Manager.SaveHostPublicKey(e.HostSystemId, e.HostPublicKey);                Network.Messages.Connection.Request.HostConnectionMessage ms = new Network.Messages.Connection.Request.HostConnectionMessage();                ms.HostSystemId = e.HostSystemId;                ms.ClientSystemId = e.ClientSystemId;                ms.Password = Manager.Manager.Encode(e.HostSystemId, this.txtPassword.Text);                Manager.Manager.sendMessage(ms);            };            Manager.ClientListener.OnClientConnected += OnClientConnected;            Manager.ClientListener.onNetworkError += ClientListener_onNetworkError;            Manager.ClientListener.onPeerConnected += ClientListener_onPeerConnected;            Manager.ClientListener.onPeerDisconnected += ClientListener_onPeerDisconnected;            Manager.ClientListener.OnOnlineCheckReceived += ClientListener_OnOnlineCheckReceived;
            Manager.Start();        }

        private void ClientListener_OnOnlineCheckReceived(object sender, OnlineCheckReceivedEventArgs e)
        {
            this.listMyHosts.Items.Clear();
            foreach (var peer in e.Peers)
            {
                ListViewItem item = new ListViewItem(peer.Name);
                item.SubItems.Add(peer.SystemId);
                if(peer.isOnline)
                {
                    item.SubItems.Add("Online");
                    item.BackColor = Color.LightGreen;
                }
                else
                {
                    item.SubItems.Add("Offline");
                    item.BackColor = Color.White;
                }
                
                this.listMyHosts.Items.Add(item);
            }
        }

        private void OnlineCheckTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Manager.Manager.sendMessage(new RequestCheckOnlineMessage { Peers = ConfigManager.ClientConfig.Peers });
        }

        private void Connection_Elapsed(object sender, System.Timers.ElapsedEventArgs e)        {            Manager.Reconnect();        }        private void ClientListener_onPeerDisconnected(object sender, EventArgs e)        {            this.lblStatus.Text = "Introducer Disconnected";            connectionStatus.Start();        }        private void ClientListener_onPeerConnected(object sender, EventArgs e)        {            this.lblStatus.Text = "Introducer Connected";            connectionStatus.Stop();            onlineCheckTimer.Start();        }        private void ClientListener_onNetworkError(object sender, EventArgs e)        {            this.lblStatus.Text = "Network Error";            connectionStatus.Start();        }        private async void OpenAForm(object sender, EventArgs e)        {            RemoteForm rm = new RemoteForm((string)sender, this.txtPassword.Text);                        rm.setManager(Manager);            rm.Show();        }        private void openForm(String systemId)        {            RemoteForm rm = new RemoteForm(systemId, this.txtPassword.Text);            rm.setManager(Manager);            rm.Show();            rm.Start();        }        private void OnClientConnected(object sender, ClientConnectedEventArgs e)        {            if (e.PasswordOk)            {                                if (this.InvokeRequired)                {                    ShowFormCallback d = new ShowFormCallback(openForm);                    this.Invoke(d, new object[] { e.SystemId });                }                else                {                    RemoteForm rm = new RemoteForm(e.SystemId, this.txtPassword.Text);                    rm.setManager(Manager);                    rm.Show();                }                                this.lblStatus.Text = "Passwort Ok Verbunden mit: " + e.SystemId;                         }            else            {                this.lblStatus.Text = "Passwort Falsch Verbindung abgebrochen von: " + e.SystemId;            }        }                private void btnLogin_Click(object sender, EventArgs e)        {			var pair = Manager.Manager.CreateNewKeyPairKey(this.txtName.Text);			Manager.Manager.sendMessage(				new Network.Messages.Connection.Request.InitalizeHostConnectionMessage				{					ClientSystemId = Manager.Manager.SystemId,					HostSystemId = this.txtName.Text,					ClientPublicKey = pair.PublicKey				}			);        }    }}