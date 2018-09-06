using System;
using LiteNetLib.Utils;

namespace Messages.Connection.OneWay
{
	public class MouseClickMessage: BaseMessage
	{
		public enum ButtonType : int
		{
			Left = 1,
			Middle = 2,
			Right = 3
		}

		public int X { get; set; }
		public int Y { get; set; }
		public ButtonType Button { get; set; }
        public Boolean DoubleClick { get; set; }
        public Boolean Down { get; set; }
        public Boolean Up { get; set; }

        public MouseClickMessage()
			: base((ushort)CustomMessageType.MouseClick)
		{
            DoubleClick = false;
            Down = false;
            Up = false;
		}

		public override void WritePayload(NetDataWriter message)
		{
			base.WritePayload(message);
			message.Put(X);
			message.Put(Y);
			message.Put((int)this.Button);
            message.Put(DoubleClick);
            message.Put(Down);
            message.Put(Up);
        }

		public override void ReadPayload(NetDataReader message)
		{
			base.ReadPayload(message);
			X = message.GetInt();
			Y = message.GetInt();
			Button = (ButtonType)message.GetInt();
            DoubleClick = message.GetBool();
            Down = message.GetBool();
            Up = message.GetBool();
		}
	}
}
