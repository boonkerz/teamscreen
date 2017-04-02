using System;
using LiteNetLib.Utils;

namespace Network
{
	public abstract class Message
	{
		public ushort MessageType { get; private set; }


		protected Message(ushort messageType)
		{
			MessageType = messageType;
		}

		public virtual void WritePayload(Network.Utils.NetDataWriter message)
		{
			message.Put(MessageType);
		}

		public virtual void ReadPayload(Network.Utils.NetDataReader message)
		{
		}
	}
}
