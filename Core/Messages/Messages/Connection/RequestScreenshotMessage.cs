using System;
using LiteNetLib.Utils;
using Messages.Connection;

namespace Messages.Connection
{
	public class RequestScreenshotMessage: BaseMessage
	{

		public Boolean Fullscreen { get; set; }

		public RequestScreenshotMessage()
			: base((ushort)CustomMessageType.RequestScreenshot)
		{
            Fullscreen = false;
		}

		public override void WritePayload(NetDataWriter message)
		{
			base.WritePayload(message);
			message.Put(Fullscreen);
        }

		public override void ReadPayload(NetDataReader message)
		{
			base.ReadPayload(message);
            Fullscreen = message.GetBool();
		}
        

	}
}
