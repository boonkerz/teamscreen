using System;
using Network.Utils;
using Model;
using LiteNetLib.Utils;

namespace Network.Messages.FileTransfer.Request
{
	public class ListingMessage : BaseMessage
	{

		public String Subdir { get; set; }

		public ListingMessage()
			: base((ushort)CustomMessageType.RequestListing)
		{
		}

		public override void WritePayload(NetDataWriter message)
		{
			base.WritePayload(message);
			message.Put(Subdir);
		}

		public override void ReadPayload(NetDataReader message)
		{
			base.ReadPayload(message);
			Subdir = message.GetString(250);
		}

	}
}
