using ByteSizeLib;
using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsGuiClient
{
    public partial class FileManager : Form
    {
        protected ClientThread Manager;

        public FileManager()
        {
            InitializeComponent();

            InitTrees();
        }

        private void InitTrees()
        {
            PopulateTreeViewLocal();
            PopulateTreeViewRemote();
        }

        public void setManager(ClientThread manager)
        {
            this.Manager = manager;
        }

        private void PopulateTreeViewRemote()
        {
            //Manager.Manager.sendMessage(new Network.Messages.Connection.Request.MouseMoveMessage { ClientSystemId = Manager.Manager.SystemId, HostSystemId = this.SystemId, X = (e.X / Ratio), Y = (e.Y / Ratio) });
        }

        private void PopulateTreeViewLocal()
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            populateListLocal(new DirectoryInfo(folder));
        }

        public void populateListLocal(DirectoryInfo info)
        {
            this.listLocal.Items.Clear();
            if (info.Parent != null)
            {
                ListViewItem upper = new ListViewItem("");
                upper.Tag = info.Parent;
                upper.SubItems.Add("..");
                upper.SubItems.Add("0");
                upper.SubItems.Add("Folder");
                upper.SubItems.Add("");
                this.listLocal.Items.Add(upper);
            }

            foreach (var f in info.GetDirectories())
            {
                ListViewItem lvi = new ListViewItem("");
                lvi.Tag = f;
                lvi.SubItems.Add(f.Name);
                lvi.SubItems.Add("0");
                lvi.SubItems.Add("Folder");
                lvi.SubItems.Add(f.LastWriteTime.ToString());

                this.listLocal.Items.Add(lvi);
            }

            foreach (var f in info.GetFiles())
            {
                ListViewItem lvi = new ListViewItem("");
                lvi.Tag = f;
                lvi.SubItems.Add(f.Name);
                lvi.SubItems.Add(ByteSize.FromBytes(f.Length).ToString());
                lvi.SubItems.Add("File");
                lvi.SubItems.Add(f.LastWriteTime.ToString());

                this.listLocal.Items.Add(lvi);
            }
        }

        private void listLocal_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.listLocal.SelectedItems.Count == 1)
            {
                ListView.SelectedListViewItemCollection items = this.listLocal.SelectedItems;

                
                if(items[0].SubItems[3].Text == "Folder")
                {
                    DirectoryInfo folder = (DirectoryInfo)items[0].Tag;
                    populateListLocal(folder);
                }
                

            }
        }
    }
}
