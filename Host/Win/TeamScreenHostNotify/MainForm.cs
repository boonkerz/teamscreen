using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZetaIpc.Runtime.Client;

namespace TeamScreenHostNotify
{
    public partial class MainForm : Form
    {

        private System.ServiceProcess.ServiceController WSController;

        protected Utils.Config.Manager ConfigManager;

        protected IpcClient c = new IpcClient();

        public MainForm()
        {
            InitializeComponent();
            c.Initialize(12345);
            this.WSController = new System.ServiceProcess.ServiceController();

            ConfigManager = new Utils.Config.Manager();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ServiceController[] AvailableServices = ServiceController.GetServices(".");

            foreach (ServiceController AvailableService in AvailableServices)
            {
                //Check the service name for IIS.
                if (AvailableService.ServiceName == "TeamScreen Host")
                {
                    WSController.ServiceName = "TeamScreen Host";
                    ShowStatus();
                    return;
                }
            }

            MessageBox.Show("The TeamScreen Service is not installed on this Machine", "TeamScreen Service is not available");
            this.Close();
            Application.Exit();

        }

        private void ShowStatus()
        {
            var rep = c.Send("getServiceStatus");
            TeamScreenCommon.IPC.Status m = JsonConvert.DeserializeObject<TeamScreenCommon.IPC.Status>(rep);
            if (m.ServiceRunning)
            {
                this.lblStatus.Text = "Running";
            }
            else
            {
                this.lblStatus.Text = "Stopped";
            }

            rep = c.Send("getHostConfig");
            Utils.Config.Host conf = JsonConvert.DeserializeObject<Utils.Config.Host>(rep);

            this.txtServer.Text = conf.ServerName;
            this.txtPort.Text = conf.ServerPort.ToString();
            this.txtSystemId.Text = conf.SystemId;
            this.txtPassword.Text = conf.Password;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Utils.Config.Host conf = new Utils.Config.Host();
            conf.ServerName = this.txtServer.Text;
            conf.ServerPort = Convert.ToInt32(this.txtPort.Text);
            conf.SystemId = this.txtSystemId.Text;
            conf.Password = this.txtPassword.Text;

            ConfigManager.HostConfig = conf;
            ConfigManager.saveHostConfig();

            c.Send("restartService");

            Thread.Sleep(300);

            ShowStatus();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                ShowIcon = false;
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(1000);
                this.Hide();
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ShowInTaskbar = true;
            notifyIcon1.Visible = false;
            this.Show();
            WindowState = FormWindowState.Normal;
        }
    }
}
