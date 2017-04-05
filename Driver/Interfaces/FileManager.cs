using Common.EventArgs.Network;
using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driver.Interfaces
{
    public interface FileManager
    {
        List<Model.Listing> getList();
        bool getParent();
        void BrowseTo(String folder);
        String getParentPath();
    }
}
