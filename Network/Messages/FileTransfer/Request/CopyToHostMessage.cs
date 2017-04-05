using System;
using Network.Utils;
using Model;
using LiteNetLib.Utils;

namespace Network.Messages.FileTransfer.Request
{
	public class CopyMessage : BaseMessage
	{

		public String Folder { get; set; }
        public byte[] Data { get; set; }

		public CopyMessage()
			: base((ushort)CustomMessageType.RequestCopy)
		{
            Folder = "";
		}

		public override void WritePayload(NetDataWriter message)
		{
			base.WritePayload(message);
            message.Put(Folder);
            message.Put(Data);
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
            Data = message.GetBytesWithLength();
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
