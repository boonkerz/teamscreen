using System;
using System.Collections.Generic;

namespace Messages.EventArgs.FileTransfer
{
    public class FileTransferListingEventArgs 
    {
        public String ClientSystemId { get; set; }
        public String HostSystemId { get; set; }
        public String Folder { get; set; }
        public List<Model.Listing> Entrys {get;set;}
        public Boolean Parent { get; set; }
        public String ParentPath { get; set; }
        public String ActFolder { get; set; }
    }
}
