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
                Mouse.move((int)e.X, (int)e.Y);
            };
            Manager.HostListener.OnKeyPress += (object sender, Common.EventArgs.Network.KeyPressEventArgs e) =>
            {
                Keyboard.Press(e.Key);
            };
            Manager.HostListener.OnKeyRelease += (object sender, Common.EventArgs.Network.KeyReleaseEventArgs e) =>
            {
                Keyboard.Release(e.Key);
            };
            Manager.HostListener.OnMouseClick += (object sender, MouseClickEventArgs e) =>
            {
                if (e.Button == MouseClickEventArgs.ButtonType.Left)
                {
                    Mouse.clickLeft((int)e.X, (int)e.Y);
                }
                if (e.Button == MouseClickEventArgs.ButtonType.Middle)
                {
                    Mouse.clickMiddle((int)e.X, (int)e.Y);
                }
                if (e.Button == MouseClickEventArgs.ButtonType.Right)
                {
                    Mouse.clickRight((int)e.X, (int)e.Y);
                }

            };
            Manager.HostListener.OnScreenshotRequest += (object sender, ScreenshotRequestEventArgs e) =>
            {
                Display.RequestScreenshot(e, this.Manager.Manager, e.Fullscreen);
            };
            Manager.start();
        }

        void Network_OnConnected(object sender, ConnectedEventArgs e)
        {
            this.txtSystemId.Text = e.SystemId;
            this.txtPassword.Text = Manager.Manager.Password;
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
