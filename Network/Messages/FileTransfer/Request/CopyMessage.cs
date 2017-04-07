using System;
using Network.Utils;
using Model;
using LiteNetLib.Utils;

namespace Network.Messages.FileTransfer.Request
{
	public class CopyMessage : BaseMessage
	{
		public String Folder { get; set; }
        public int Fragement { get; set; }
        public int TotalFragments { get; set; }
        public String Hash { get; set; }
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
            message.Put(Fragement);
            message.Put(TotalFragments);
            message.Put(Hash);
            message.Put(Data);

        }

		public override void ReadPayload(NetDataReader message)
		{
			base.ReadPayload(message);
            Folder = message.GetString(250);
            Fragement = message.GetInt();
            TotalFragments = message.GetInt();
            Hash = message.GetString(50);
            Data = message.GetBytesWithLength();

		}

	}
}
