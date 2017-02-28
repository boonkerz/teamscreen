using System;
using LiteNetLib;

namespace Network
{
	public sealed class ClientManager : NetManager
	{

		MessageHandler _messageHandler;
		public String SystemId { get; set; }
		public String Password { get; set; }

		public ClientManager(INetEventListener listener, string connectKey) : base(listener, 1, connectKey)
		{
			_messageHandler = new MessageHandler();
		}

		public void sendMessage(Message message)
		{
			base.SendToAll(_messageHandler.encodeMessage(message), SendOptions.Unreliable);
		}
	}
}
