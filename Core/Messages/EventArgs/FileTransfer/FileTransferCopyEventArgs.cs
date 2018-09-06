using System;
using System.Collections.Generic;

namespace Messages.EventArgs.FileTransfer
{
    public class FileTransferCopyEventArgs 
    {
        public String ClientSystemId { get; set; }
        public String HostSystemId { get; set; }
        public String Folder { get; set; }
        public String Name { get; set; }
        public byte[] Data {get;set;}
	}
}
