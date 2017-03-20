using System;
using System.Security.Cryptography;
using LiteNetLib;
using Network.Utils;

namespace Network
{
	public sealed class ClientManager : BaseManager
	{

		public ClientManager(INetEventListener listener, string connectKey) : base(listener, connectKey)
		{

		}
	}
}
