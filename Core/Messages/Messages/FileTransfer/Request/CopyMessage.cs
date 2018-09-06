using System;
using LiteNetLib.Utils;

namespace Messages.FileTransfer.Request
{
	public class CopyMessage : BaseMessage
	{
		public String Folder { get; set; }
        public String Name { get; set; }
        public byte[] Data { get; set; }

		public CopyMessage()
			: base((ushort)CustomMessageType.RequestCopy)
		{
            
		}

		public override void WritePayload(NetDataWriter message)
		{
			base.WritePayload(message);
            message.Put(Folder);
            message.Put(Name);
            message.Put(Data);

        }

		public override void ReadPayload(NetDataReader message)
		{
			base.ReadPayload(message);
            Folder = message.GetString(250);
            Name = message.GetString(250);
            Data = message.GetRemainingBytes();

		}

	}
}
