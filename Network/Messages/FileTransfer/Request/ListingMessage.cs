using System;
using Network.Utils;
using Model;
using LiteNetLib.Utils;

namespace Network.Messages.FileTransfer.Request
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
            /*if (this.Introducer)
            {
                this.CopyEncryptedFromTempStorage(message);
            }
            else
            {
                message.Put(Folder);
            }*/
        }

		public override void ReadPayload(NetDataReader message)
		{
			base.ReadPayload(message);
            Folder = message.GetString(250);
            /*if (this.Introducer)
            {
                this.CopyEncryptedToTempStorage(message);
            }
            else
            {
                this.Decrypt(message);
                Folder = message.GetString(250);
            }*/
		}

	}
}
