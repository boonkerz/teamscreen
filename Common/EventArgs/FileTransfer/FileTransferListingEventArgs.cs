using System;
using System.Collections.Generic;

namespace Common.EventArgs.FileTransfer
{
    public class FileTransferListingEventArgs : System.EventArgs
    {
        public String ClientSystemId { get; set; }
        public String HostSystemId { get; set; }
        public String Folder { get; set; }
        public List<Model.Listing> Entrys {get;set;}
	}
}
