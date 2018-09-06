using System;
using System.Security.Cryptography;
using LiteNetLib;
using Network.Helper;

namespace Network.Manager
{
	public sealed class HostManager : BaseManager
	{

		public HostManager(INetEventListener listener, string connectKey) : base(listener, connectKey)
        {
            _messageHandler = new MessageHandler(MessageHandler.ManagerModus.Host, this);
        }
	}
}
