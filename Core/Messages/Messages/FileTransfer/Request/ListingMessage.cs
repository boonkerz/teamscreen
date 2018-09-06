using System;
using LiteNetLib.Utils;

namespace Messages.FileTransfer.Request
{
	public class ListingMessage : BaseMessage
	{

		public String Folder { get; set; }

		public ListingMessage()
			: base((ushort)CustomMessageType.RequestListing)
		{
            Folder = "";
		}

		public override void WritePayload(NetDataWriter message)
		{
			base.WritePayload(message);
            message.Put(Folder);
        }

		public override void ReadPayload(NetDataReader message)
		{
			base.ReadPayload(message);
            Folder = message.GetString(250);
        }
	}
}
