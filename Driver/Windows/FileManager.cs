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
        public List<Listing> getList(string folder)
        {
            if (folder == "")
            {
                folder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            }      
            return populateListLocal(new DirectoryInfo(folder));
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

                data.Add(lvi);
            }

            foreach (var f in info.GetFiles())
            {
                Model.Listing lvi = new Model.Listing();
                lvi.Name = f.Name;
                lvi.Size = f.Length;
                lvi.Directory = false;
                lvi.Modified = f.LastWriteTime;
                
                data.Add(lvi);
            }

            return data;
        }
    }
}
