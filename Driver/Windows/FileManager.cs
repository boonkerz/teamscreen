using Driver.Windows.DfMirage;
using Network;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Common.EventArgs.Network;
using Model;
using System.Collections.Generic;
using System.IO;

namespace Driver.Windows
{
    public class FileManager : Interfaces.FileManager
    {
        protected DirectoryInfo ActFolder { get; set; }

        public void BrowseTo(string folder)
        {
            if (folder == "")
            {
                folder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            }

            ActFolder = new DirectoryInfo(folder);
        }

        public string GetActFolder()
        {
            return this.ActFolder.ToString();
        }

        public List<DriveInfo> GetAllDrives()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            List<DriveInfo> data = new List<DriveInfo>();
            foreach (DriveInfo d in allDrives)
            {
                if (d.IsReady == true)
                {
                    data.Add(d);
                }
            }

            return data;
        }

        public List<Listing> GetList()
        {
            return populateListLocal(ActFolder);
        }

        public bool GetParent()
        {
            
            if(ActFolder.Parent == null)
            {
                return false;
            }
            return true;
        }

        public String GetParentPath()
        {

            if (ActFolder.Parent == null)
            {
                return "";
            }
            return ActFolder.Parent.FullName;
        }

        protected List<Model.Listing> populateListLocal(DirectoryInfo info)
        {
            List<Model.Listing> data = new List<Listing>();

            foreach (var f in info.GetDirectories())
            {
                Model.Listing lvi = new Model.Listing();
                lvi.Name = f.Name;
                lvi.Size = 0;
                lvi.Directory = true;
                lvi.Modified = f.LastWriteTime.ToString();
                lvi.Path = f.FullName;
                data.Add(lvi);
            }

            foreach (var f in info.GetFiles())
            {
                Model.Listing lvi = new Model.Listing();
                lvi.Name = f.Name;
                lvi.Size = f.Length;
                lvi.Directory = false;
                lvi.Modified = f.LastWriteTime.ToString();
                lvi.Path = f.FullName;
                
                data.Add(lvi);
            }

            return data;
        }
    }
}
