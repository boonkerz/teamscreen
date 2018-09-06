using System;
using Model;
using System.Drawing;
using LiteNetLib.Utils;

namespace Messages.Connection
{
	public class ResponseEmptyScreenshotMessage : BaseMessage
	{

		public ResponseEmptyScreenshotMessage()
			: base((ushort)CustomMessageType.ResponseEmptyScreenshot)
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
