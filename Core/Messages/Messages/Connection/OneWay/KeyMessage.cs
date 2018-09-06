using System;
using LiteNetLib.Utils;

namespace Messages.Connection.OneWay
{
	public class KeyMessage: BaseMessage
	{
        public enum KeyMode : int
        {
            Down = 1,
            Up = 2
        }

        public uint Key { get; set; }
        public Boolean Shift { get; set; }
        public Boolean Control { get; set; }
        public Boolean Alt { get; set; }
        public KeyMode Mode { get; set; }
        

        public KeyMessage()
			: base((ushort)CustomMessageType.Key)
		{
		}

		public override void WritePayload(NetDataWriter message)
		{
			base.WritePayload(message);
			message.Put(Key);
			message.Put(Shift);
			message.Put(Control);
			message.Put(Alt);
			message.Put((int)Mode);
		}

		public override void ReadPayload(NetDataReader message)
		{
			base.ReadPayload(message);
			Key = message.GetUInt();
			Shift = message.GetBool();
			Control = message.GetBool();
			Alt = message.GetBool();
			Mode = (KeyMode)message.GetInt();
		}
	}
}
