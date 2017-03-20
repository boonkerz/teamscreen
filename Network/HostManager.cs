using System;
using System.Security.Cryptography;
using LiteNetLib;
using Network.Utils;

namespace Network
{
	public sealed class HostManager : BaseManager
	{

		public HostManager(INetEventListener listener, string connectKey) : base(listener, connectKey)
        {
		}
	}
}
