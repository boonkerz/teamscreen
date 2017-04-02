using System;
using LiteNetLib.Utils;
using Network.Messages.Connection;

namespace Network.Messages.Connection
{
	public class RequestScreenshotMessage: BaseMessage
	{

		public Boolean Fullscreen { get; set; }

		public RequestScreenshotMessage()
			: base((ushort)CustomMessageType.RequestScreenshot)
		{
            Fullscreen = false;
		}

		public override void WritePayload(Network.Utils.NetDataWriter message)
		{
			base.WritePayload(message);
			message.Put(Fullscreen);

            this.Encrypt(message);
        }

		public override void ReadPayload(Network.Utils.NetDataReader message)
		{
			base.ReadPayload(message);
            Fullscreen = message.GetBool();
		}
        

	}
}
