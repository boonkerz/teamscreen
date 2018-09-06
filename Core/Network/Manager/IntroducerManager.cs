using System;
using LiteNetLib;
using Messages;
using Network.Manager;
using Network.Helper;

namespace Network
{
	public sealed class IntroducerManager
	{

		NetManager _netManager;
		MessageHandler _messageHandler;
		public String SystemId { get; set; }
		public String Password { get; set; }


		public IntroducerManager(INetEventListener listener, string connectKey)
        {
			_netManager = new NetManager(listener, 12000, connectKey);
			_netManager.UnsyncedEvents = true;
			_netManager.PingInterval = 10000;
			_netManager.DisconnectTimeout = 20000;
			_messageHandler = new MessageHandler(MessageHandler.ManagerModus.Broker);
		}

		public void sendMessage(Message message)
		{
			_netManager.SendToAll(_messageHandler.encodeMessage(message), SendOptions.ReliableOrdered);
		}

		public bool Start(int port)
		{
			return _netManager.Start(port);
		}

		public void Stop()
		{
			_netManager.Stop();
		}

		public void Connect(String host, int port)
		{
			_netManager.Connect(host, port);
		}

		public NetManager getNetmanager()
		{
			return _netManager;
		}
	}
}
