using System;
using System.Threading;
using System.Timers;
using LiteNetLib;
using LiteNetLib.Utils;
using Network;
using Network.Messages.Connection;
using Network.Messages.System;

namespace Common
{
	public class Host
	{
		private class HostListener : INetEventListener
		{
			private readonly MessageHandler _messageHandler;
			public HostManager _hostManager;

			public HostListener()
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
				Console.WriteLine("[Host] received data. Processing...");
				Message msg = _messageHandler.decodeMessage(reader);

				switch (msg.MessageType)
				{
					case (ushort)Network.Messages.System.CustomMessageType.ResponseHostIntroducerRegistration:
						handleResponseHostIntroducerRegistration(peer, (Network.Messages.System.ResponseHostIntroducerRegistrationMessage)msg);
						break;
					case (ushort)Network.Messages.Connection.CustomMessageType.RequestHostConnection:
						handleRequestHostConnection(peer, (Network.Messages.Connection.RequestHostConnectionMessage)msg);
						break;
				}

			}

			public void handleResponseHostIntroducerRegistration(NetPeer peer, Network.Messages.System.ResponseHostIntroducerRegistrationMessage message)
			{
				this._hostManager.SystemId = message.Machine.SystemId;
			}

			public void handleRequestHostConnection(NetPeer peer, Network.Messages.Connection.RequestHostConnectionMessage message)
			{
				ResponseHostConnectionMessage res = new ResponseHostConnectionMessage();
				res.HostSystemId = message.HostSystemId;
				res.ClientSystemId = message.ClientSystemId;
				res.PasswordOk = false;
				if (this._hostManager.Password == message.Password)
				{
					res.PasswordOk = false;
				}
				peer.Send(_messageHandler.encodeMessage(res), SendOptions.Unreliable);
			}

			public void OnNetworkReceiveUnconnected(NetEndPoint remoteEndPoint, NetDataReader reader, UnconnectedMessageType messageType)
			{

			}

			public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
			{

			}
		}

		private HostListener _hostListener;

		public void Run()
		{

            _hostListener = new HostListener();

			HostManager host = new HostManager(_hostListener, "myapp1");
			host.Password = "aA";
			_hostListener._hostManager = host;
			host.MergeEnabled = true;
			host.PingInterval = 10000;
			if (!host.Start())
			{
				Console.WriteLine("Client1 start failed");
				return;
			}
			host.Connect("127.0.0.1", 9050);

			host.sendMessage(new RequestHostIntroducerRegistrationMessage());

			while (!Console.KeyAvailable)
			{
				host.PollEvents();
				Thread.Sleep(15);
			}

			host.Stop();
		}
	}
}
