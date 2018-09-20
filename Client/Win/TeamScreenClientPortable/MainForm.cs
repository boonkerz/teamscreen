using Messages.Connection.OneWay;
using Messages.Connection.Request;
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

namespace TeamScreenClientPortable
{
    public partial class MainForm : Form
    {

        public ClientThread clientThread { get { return Network.Instance.Client.Instance.Thread; } }

        protected Utils.Config.Manager ConfigManager;

        protected System.Timers.Timer connectionStatus;

        System.Timers.Timer onlineCheckTimer;

        public delegate void ShowFormCallback(String systemId);

        public MainForm()
        {
            InitializeComponent();

            connectionStatus = new System.Timers.Timer(1000);

            connectionStatus.Elapsed += Connection_Elapsed;

            ConfigManager = new Utils.Config.Manager();

            clientThread.Events.OnHostInitalizeConnected += Events_OnHostInitalizeConnected;
            clientThread.Events.OnClientConnected += ClientListener_OnClientConnected;
            clientThread.Events.onNetworkError += ClientListener_onNetworkError;
            clientThread.Events.onPeerConnected += ClientListener_onPeerConnected;
            clientThread.Events.onPeerDisconnected += ClientListener_onPeerDisconnected;
            //clientThread.Events.OnOnlineCheckReceived += ClientListener_OnOnlineCheckReceived;

            this.txtServer.Text = ConfigManager.ClientConfig.ServerName;
            this.txtServerPort.Text = ConfigManager.ClientConfig.ServerPort.ToString();

            clientThread.Start();
        }

        private void Events_OnHostInitalizeConnected(object sender, Messages.EventArgs.Network.Client.HostInitalizeConnectedEventArgs e)
        {
            Messages.Connection.Request.HostConnectionMessage ms = new Messages.Connection.Request.HostConnectionMessage();
            ms.HostSystemId = e.HostSystemId;
            ms.ClientSystemId = e.ClientSystemId;
            ms.Password = clientThread.Manager.Encode(e.HostSystemId, this.txtPassword.Text);
            ms.SymmetricKey = clientThread.Manager.Encode(e.HostSystemId, clientThread.Manager.getSymmetricKeyForRemoteId(e.HostSystemId));

            clientThread.Manager.sendMessage(ms);
        }

        private void ClientListener_onPeerDisconnected(object sender, EventArgs e)
        {
            this.lblStatus.Text = "Broker Disconnected";
            connectionStatus.Start();
        }

        private void ClientListener_onPeerConnected(object sender, EventArgs e)
        {
            this.lblStatus.Text = "Broker Connected";
            connectionStatus.Stop();
            //onlineCheckTimer.Start();
        }

        private void ClientListener_onNetworkError(object sender, EventArgs e)
        {
            this.lblStatus.Text = "Network Error";
            connectionStatus.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ConfigManager.ClientConfig.ServerName = this.txtServer.Text;
            ConfigManager.ClientConfig.ServerPort = Convert.ToInt32(this.txtServerPort.Text);
            ConfigManager.saveClientConfig();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            var pair = clientThread.Manager.CreateNewKeyPairKey(this.txtSystemId.Text);
            clientThread.Manager.sendMessage(
                new Messages.Connection.Request.InitalizeHostConnectionMessage
                {
                    ClientSystemId = clientThread.Manager.SystemId,
                    HostSystemId = this.txtSystemId.Text,
                    ClientPublicKey = pair.PublicKey
                }
            );
        }

        private void openForm(String systemId)
        {
            RemoteForm rm = new RemoteForm(systemId, this.txtPassword.Text);
            rm.Name = "Host: " + systemId;
            rm.setThread(clientThread);
            rm.Show();
            rm.Start();
        }

        private void Connection_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            clientThread.Reconnect();
        }

        private void ClientListener_OnClientConnected(object sender, ClientConnectedEventArgs e)
        {
            if (e.PasswordOk)
            {

                if (this.InvokeRequired)
                {
                    ShowFormCallback d = new ShowFormCallback(openForm);
                    this.Invoke(d, new object[] { e.SystemId });
                }
                else
                {
                    RemoteForm rm = new RemoteForm(e.SystemId, this.txtPassword.Text);
                    rm.Name = "Host: " + e.SystemId;
                    rm.setThread(clientThread);
                    rm.Show();
                    rm.Start();
                }

                this.lblStatus.Text = "Passwort Ok Verbunden mit: " + e.SystemId;

            }
            else
            {
                this.lblStatus.Text = "Passwort Falsch Verbindung abgebrochen von: " + e.SystemId;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            clientThread.Manager.sendMessage(new DisconnectFromIntroducerMessage() { SystemId = clientThread.Manager.SystemId});
        }
    }
}
