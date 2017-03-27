using Common;
using Common.EventArgs.Network;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
            Manager.ClientListener.OnClientConnected += (object sender, ClientConnectedEventArgs e) =>
            {
                if (e.PasswordOk)
                {
                    this.lblStatus.Text = "Passwort Ok Verbunden mit: " + e.SystemId;
                    RemoteForm rm = new RemoteForm(e.SystemId);
                    rm.Show();
                }
                else
                {
                    this.lblStatus.Text = "Passwort Falsch Verbindung abgebrochen von: " + e.SystemId;
                }
            };

            Manager.start();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

        }
    }
}
