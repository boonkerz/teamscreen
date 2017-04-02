using System;
using Network.Utils;
using Model;
using System.Drawing;

namespace Network.Messages.Connection
{
	public class ResponseScreenshotMessage : BaseMessage
	{

		public String HostSystemId { get; set; }
		public String ClientSystemId { get; set; }
		public byte[] Image { get; set; } 
		public Rectangle Bounds { get; set; }
        public Boolean Fullscreen { get; set; }

        public ResponseScreenshotMessage()
			: base((ushort)CustomMessageType.ResponseScreenshot)
		{
		}

		public override void WritePayload(NetDataWriter message)
		{
			base.WritePayload(message);
			message.Put(HostSystemId);
			message.Put(ClientSystemId);
			message.Put(Bounds.X);
            message.Put(Bounds.Y);
            message.Put(Bounds.Width);
            message.Put(Bounds.Height);
            message.Put(Fullscreen);
            message.Put(Image);

		}

		public override void ReadPayload(NetDataReader message)
		{
			base.ReadPayload(message);
			HostSystemId = message.GetString(100);
			ClientSystemId = message.GetString(100);
            Bounds = new Rectangle(message.GetInt(), message.GetInt(), message.GetInt(), message.GetInt());
            Fullscreen = message.GetBool();
 			Image = message.GetRemainingBytes();

		}

	}
}
