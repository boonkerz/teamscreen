using System;
using System.Threading;
using LiteNetLib;
using LiteNetLib.Utils;
using Network;
using Network.Messages.Connection;
using Network.Messages.System;

namespace Common
{
	public class Client
	{
		private class ClientListener : INetEventListener
		{
			private readonly MessageHandler _messageHandler;
			public ClientManager _clientManager;

			public ClientListener()
			{
				_messageHandler = new MessageHandler();
			}

			public void OnPeerConnected(NetPeer peer)
			{
				Console.WriteLine("[Client] connected to: {0}:{1}", peer.EndPoint.Host, peer.EndPoint.Port);

			}

			public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
			{
				Console.WriteLine("[Client] disconnected: " + disconnectInfo.Reason);
			}

			public void OnNetworkError(NetEndPoint endPoint, int socketErrorCode)
			{
				Console.WriteLine("[Client] error! " + socketErrorCode);
			}

			public void OnNetworkReceive(NetPeer peer, NetDataReader reader)
			{
				Console.WriteLine("[Client] received data. Processing...");
				Message msg = _messageHandler.decodeMessage(reader);

				switch (msg.MessageType)
				{
					case (ushort)Network.Messages.System.CustomMessageType.ResponseClientIntroducerRegistration:
						handleResponseClientIntroducerRegistration(peer, (Network.Messages.System.ResponseClientIntroducerRegistrationMessage)msg);
						break;
					case (ushort)Network.Messages.Connection.CustomMessageType.ResponseHostConnection:
						handleResponseHostConnection(peer, (Network.Messages.Connection.ResponseHostConnectionMessage)msg);
						break;
				}

			}
			public void handleResponseHostConnection(NetPeer peer, Network.Messages.Connection.ResponseHostConnectionMessage message)
			{
				Console.WriteLine("[Client] error! " + message.PasswordOk);
				Console.WriteLine("[Client] error! " + message.HostFound);
			}

			public void handleResponseClientIntroducerRegistration(NetPeer peer, Network.Messages.System.ResponseClientIntroducerRegistrationMessage message)
			{
				this._clientManager.SystemId = message.Machine.SystemId;
			}

			public void OnNetworkReceiveUnconnected(NetEndPoint remoteEndPoint, NetDataReader reader, UnconnectedMessageType messageType)
			{

			}

			public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
			{

			}
		}

		private ClientListener _clientListener;

		public void Run()
		{

			_clientListener = new ClientListener();

			ClientManager client = new ClientManager(_clientListener, "myapp1");
			_clientListener._clientManager = client;
			client.MergeEnabled = true;
			client.PingInterval = 10000;
			if (!client.Start())
			{
				Console.WriteLine("Client1 start failed");
				return;
			}
			client.Connect("127.0.0.1", 9050);
			client.sendMessage(new RequestClientIntroducerRegistrationMessage());


			while (true)
			{
				if (Console.KeyAvailable)
				{
					var key = Console.ReadKey(true).Key;
					if (key == ConsoleKey.Escape)
					{
						break;
					}
					if (key == ConsoleKey.A)
					{
						client.sendMessage(new RequestHostConnectionMessage { ClientSystemId = client.SystemId, HostSystemId = "123456789", Password = "123456789" });
					}
				}
				client.PollEvents();
				Thread.Sleep(15);
			}

			client.Stop();
		}
	}
}
