using System;
using Network.Utils;
using Model;
using System.Drawing;

namespace Network.Messages.Connection
{
	public class ResponseEmptyScreenshotMessage : BaseMessage
	{

		public String HostSystemId { get; set; }
		public String ClientSystemId { get; set; }

        public ResponseEmptyScreenshotMessage()
			: base((ushort)CustomMessageType.ResponseEmptyScreenshot)
		{
		}

		public override void WritePayload(NetDataWriter message)
		{
			base.WritePayload(message);
			message.Put(HostSystemId);
			message.Put(ClientSystemId);

		}

		public override void ReadPayload(NetDataReader message)
		{
			base.ReadPayload(message);
			HostSystemId = message.GetString(100);
			ClientSystemId = message.GetString(100);
		}

	}
}
