using System;
namespace Network
{
	public abstract class Message
	{
		public ushort MessageType { get; private set; }
		/*
		public Protocol Protocol { get; private set; }

		protected Message(Protocol protocol, ushort messageType)
		{
			if (protocol == null)
				throw new ArgumentNullException("protocol");

			Protocol = protocol;
			MessageType = messageType;
		}

		public virtual void WritePayload(NetOutgoingMessage message)
		{
			message.Write(Protocol);
			message.Write(MessageType);
		}

		public virtual void ReadPayload(NetIncomingMessage message)
		{
		}*/
	}
}
