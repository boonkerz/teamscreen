using System;
using LiteNetLib.Utils;
using Model;

namespace Network.Messages.Connection
{
	public class ResponseScreenshotMessage : BaseMessage
	{

		public String HostSystemId { get; set; }
		public String ClientSystemId { get; set; }
		public byte[] Image { get; set; } 

		public ResponseScreenshotMessage()
			: base((ushort)CustomMessageType.ResponseScreenshot)
		{
		}

		public override void WritePayload(NetDataWriter message)
		{
			base.WritePayload(message);
			message.Put(HostSystemId);
			message.Put(ClientSystemId);
			message.Put(Image);
		}

		public override void ReadPayload(NetDataReader message)
		{
			base.ReadPayload(message);
			HostSystemId = message.GetString(100);
			ClientSystemId = message.GetString(100);
			Image = message.GetBytes();
		}

	}
}
