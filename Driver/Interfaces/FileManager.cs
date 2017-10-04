using Common.EventArgs.Network;
using Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driver.Interfaces
{
    public interface FileManager
    {
        List<Model.Listing> GetList();
        bool GetParent();
        void BrowseTo(String folder);
        String GetParentPath();
        String GetActFolder();
        List<DriveInfo> GetAllDrives();
    }
}
