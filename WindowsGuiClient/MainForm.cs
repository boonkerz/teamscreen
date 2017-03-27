using Common;
using Common.EventArgs.Network;
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

namespace WindowsGuiClient
{
    public partial class MainForm : Form
    {
        public ClientThread Manager { get { return Common.Instance.Client.Instance.Thread; } }

        protected Common.Config.Manager ConfigManager;

        public MainForm()
        {
            
            InitializeComponent();

            ConfigManager = new Common.Config.Manager();

            Manager.ClientListener.OnHostInitalizeConnected += (object sender, Common.EventArgs.Network.Client.HostInitalizeConnectedEventArgs e) =>
            {
                Manager.Manager.SaveHostPublicKey(e.HostSystemId, e.HostPublicKey);

                Network.Messages.Connection.Request.HostConnectionMessage ms = new Network.Messages.Connection.Request.HostConnectionMessage();
                ms.HostSystemId = e.HostSystemId;
                ms.ClientSystemId = e.ClientSystemId;
                ms.Password = Manager.Manager.Encode(e.HostSystemId, this.txtPassword.Text);

                Manager.Manager.sendMessage(ms);


            };
            Manager.ClientListener.OnClientConnected += OnClientConnected;

            Manager.start();
        }

        private async void OpenAForm(object sender, EventArgs e)
        {
            RemoteForm rm = new RemoteForm((string)sender);
            rm.setManager(Manager);
            rm.Show();
        }

        private void OnClientConnected(object sender, ClientConnectedEventArgs e)
        {
            if (e.PasswordOk)
            {

                this.Invoke(new EventHandler(OpenAForm), e.SystemId);

                this.lblStatus.Text = "Passwort Ok Verbunden mit: " + e.SystemId;
             
            }
            else
            {
                this.lblStatus.Text = "Passwort Falsch Verbindung abgebrochen von: " + e.SystemId;
            }
        }
        

        private void btnLogin_Click(object sender, EventArgs e)
        {
			var pair = Manager.Manager.CreateNewKeyPairKey(this.txtName.Text);
			Manager.Manager.sendMessage(
				new Network.Messages.Connection.Request.InitalizeHostConnectionMessage
				{
					ClientSystemId = Manager.Manager.SystemId,
					HostSystemId = this.txtName.Text,
					ClientPublicKey = pair.PublicKey
				}
			);
        }
    }
}
