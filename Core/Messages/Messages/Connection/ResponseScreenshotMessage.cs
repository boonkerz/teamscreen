using System;
using Model;
using System.Drawing;
using LiteNetLib.Utils;

namespace Messages.Connection
{
	public class ResponseScreenshotMessage : BaseMessage
	{

		public byte[] Image { get; set; } 
		public Rectangle Bounds { get; set; }
        public Boolean Fullscreen { get; set; }

        public ResponseScreenshotMessage()
			: base((ushort)CustomMessageType.ResponseScreenshot)
		{
            EncryptedMessage = true;
		}

		public override void WritePayload(NetDataWriter message)
		{
			base.WritePayload(message);
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
			Bounds = new Rectangle(message.GetInt(), message.GetInt(), message.GetInt(), message.GetInt());
            Fullscreen = message.GetBool();
 			Image = message.GetRemainingBytes();

		}

	}
}
