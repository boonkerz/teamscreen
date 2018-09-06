using System;
using System.Security.Cryptography;
using LiteNetLib;
using Network.Helper;
using Network;

namespace Network.Manager
{
	public sealed class ClientManager : BaseManager
	{

		public ClientManager(INetEventListener listener, string connectKey) : base(listener, connectKey)
		{

            _messageHandler = new MessageHandler(MessageHandler.ManagerModus.Client, this);

        }
	}
}
