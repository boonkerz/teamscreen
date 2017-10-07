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

namespace WindowsNotifyHost
{
    public partial class Form1 : Form
    {

        private System.ServiceProcess.ServiceController WSController;

        protected IpcClient c = new IpcClient();

        public Form1()
        {
            InitializeComponent();
            this.WSController = new System.ServiceProcess.ServiceController();

            Thread.Sleep(3000);
            c.Initialize(12345);
        }

        private void Form1_Resize(object sender, EventArgs e)
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

        private void Form1_Load(object sender, EventArgs e)
        {
            ServiceController[] AvailableServices = ServiceController.GetServices(".");

            foreach (ServiceController AvailableService in AvailableServices)
            {
                //Check the service name for IIS.
                if (AvailableService.ServiceName == "teamscreen")
                {
                    WSController.ServiceName = "teamscreen";
                    SetButtonStatus();
                    return;
                }
            }

            MessageBox.Show("The TeamScreen Service is not installed on this Machine", "TeamScreen Service is not available");
            this.Close();
            Application.Exit();

            notifyIcon1.BalloonTipTitle = "TeamScreen";
            notifyIcon1.BalloonTipText = "Running";
        }

        private void SetButtonStatus()
        {
            var rep = c.Send("serviceStatus");
            WindowsCommon.IPC.Status m = JsonConvert.DeserializeObject<WindowsCommon.IPC.Status>(rep);
            if(m.ServiceRunning)
            {
                ButtonStop.Enabled = true;
                ButtonStart.Enabled = false;
            }
            else
            {
                ButtonStop.Enabled = false;
                ButtonStart.Enabled = true;
            }
        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            var rep = c.Send("serviceStart");
            SetButtonStatus();
        }

        private void ButtonStop_Click(object sender, EventArgs e)
        {
            var rep = c.Send("serviceStop");
            SetButtonStatus();
        }
    }
}
