using System;
using LiteNetLib.Utils;
using Network.Messages.Connection;

namespace Network.Messages.Connection
{
	public class RequestScreenshotMessage: BaseMessage
	{

		public String HostSystemId { get; set; }
		public String ClientSystemId { get; set; }
        public Boolean Fullscreen { get; set; }

		public RequestScreenshotMessage()
			: base((ushort)CustomMessageType.RequestScreenshot)
		{
            Fullscreen = false;
		}

		public override void WritePayload(NetDataWriter message)
		{
			base.WritePayload(message);
			message.Put(HostSystemId);
			message.Put(ClientSystemId);
            message.Put(Fullscreen);
        }

		public override void ReadPayload(NetDataReader message)
		{
			base.ReadPayload(message);
			HostSystemId = message.GetString(100);
			ClientSystemId = message.GetString(100);
            Fullscreen = message.GetBool();
		}

	}
}
