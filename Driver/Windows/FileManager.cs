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

            var ActFolder = new DirectoryInfo(folder);
        }

        public List<Listing> getList()
        {
            return populateListLocal(ActFolder);
        }

        public bool getParent()
        {
            
            if(ActFolder.Parent == null)
            {
                return false;
            }
            return true;
        }

        public String getParentPath()
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
                lvi.Modified = f.LastWriteTime;
                lvi.Path = f.FullName;
                data.Add(lvi);
            }

            foreach (var f in info.GetFiles())
            {
                Model.Listing lvi = new Model.Listing();
                lvi.Name = f.Name;
                lvi.Size = f.Length;
                lvi.Directory = false;
                lvi.Modified = f.LastWriteTime;
                lvi.Path = f.FullName;
                
                data.Add(lvi);
            }

            return data;
        }
    }
}
