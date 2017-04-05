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

        protected String SystemId;

        delegate void fillListRemoteCallback(List<Model.Listing> entrys);

        public FileManager(String systemId)
        {
            SystemId = systemId;
            InitializeComponent();

            
        }

        protected void fillListRemote(List<Model.Listing> entrys)
        {
            this.listRemote.Items.Clear();
            ListViewItem upper = new ListViewItem("");
            upper.SubItems.Add("..");
            upper.SubItems.Add("0");
            upper.SubItems.Add("Folder");
            upper.SubItems.Add("");
            this.listLocal.Items.Add(upper);

            foreach (var entry in entrys)
            {
                upper = new ListViewItem("");
                upper.SubItems.Add(entry.Name);
                upper.SubItems.Add(entry.Size.ToString());
                if (entry.Directory)
                {
                    upper.SubItems.Add("Folder");
                }
                else
                {
                    upper.SubItems.Add("File");
                }
                upper.SubItems.Add("");
                this.listRemote.Items.Add(upper);
            }
        }

        private void ClientListener_OnFileTransferListing(object sender, Common.EventArgs.FileTransfer.FileTransferListingEventArgs e)
        {
            

            if (!this.IsDisposed && !this.Disposing)
            {
                if (this.InvokeRequired)
                {
                    fillListRemoteCallback d = new fillListRemoteCallback(fillListRemote);
                    this.Invoke(d, new object[] { e.Entrys });
                }
                else
                {
                    fillListRemote(e.Entrys);
                }
            }
        }

        private void InitTrees()
        {
            PopulateTreeViewLocal();
            PopulateTreeViewRemote();
        }

        public void setManager(ClientThread manager)
        {
            this.Manager = manager;
            Manager.ClientListener.OnFileTransferListing += ClientListener_OnFileTransferListing;

            InitTrees();
        }

        private void PopulateTreeViewRemote()
        {
            Manager.Manager.sendMessage(new Network.Messages.FileTransfer.Request.ListingMessage { ClientSystemId = Manager.Manager.SystemId, HostSystemId = this.SystemId });
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
