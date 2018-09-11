using System;
using LiteNetLib.Utils;
using Messages.Connection;

namespace Messages.Connection
{
	public class StopScreenSharingMessage: BaseMessage
	{

		public StopScreenSharingMessage()
			: base((ushort)CustomMessageType.StopScreenSharing)
		{
     	}

		public override void WritePayload(NetDataWriter message)
		{
			base.WritePayload(message);
        }

		public override void ReadPayload(NetDataReader message)
		{
			base.ReadPayload(message);
		}
        

	}
}
