using System;
using System.Collections.Generic;
using LiteNetLib;
using LiteNetLib.Utils;
using Model;
using Network;
using Network.Messages.Connection;
using Network.Messages.System;

namespace Common.Listener
{
	public class IntroducerListener : INetEventListener
	{
		public NetManager Server;
		private readonly MessageHandler _messageHandler;
		private readonly Dictionary<string, NetPeer> _hostPeers = new Dictionary<string, NetPeer>();

		private readonly Dictionary<string, NetPeer> _clientPeers = new Dictionary<string, NetPeer>();

		public IntroducerListener()
		{
			_messageHandler = new MessageHandler();

		}

		public void OnPeerConnected(NetPeer peer)
		{
			Console.WriteLine("[Server] Peer connected: " + peer.EndPoint);
			var peers = Server.GetPeers();
			foreach (var netPeer in peers)
			{
				Console.WriteLine("ConnectedPeersList: id={0}, ep={1}", netPeer.ConnectId, netPeer.EndPoint);
			}
		}

		public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
		{
			Console.WriteLine("[Server] Peer disconnected: " + peer.EndPoint + ", reason: " + disconnectInfo.Reason);
		}

		public void OnNetworkError(NetEndPoint endPoint, int socketErrorCode)
		{
			Console.WriteLine("[Server] error: " + socketErrorCode);
		}

		public void OnNetworkReceive(NetPeer peer, NetDataReader reader)
		{
			Console.WriteLine("[Server] received data. Processing...");
			Message msg = _messageHandler.decodeMessage(reader);

			switch (msg.MessageType)
			{
				case (ushort)Network.Messages.System.CustomMessageType.RequestHostIntroducerRegistration:
					handleRequestHostIntroducerRegistration(peer, (RequestHostIntroducerRegistrationMessage)msg);
					break;
				case (ushort)Network.Messages.System.CustomMessageType.RequestClientIntroducerRegistration:
					handleRequestClientIntroducerRegistration(peer, (RequestClientIntroducerRegistrationMessage)msg);
					break;
				case (ushort)Network.Messages.Connection.CustomMessageType.RequestHostConnection:
					handleRequestHostConnection(peer, (Network.Messages.Connection.RequestHostConnectionMessage)msg);
					break;
				case (ushort)Network.Messages.Connection.CustomMessageType.ResponseHostConnection:
					handleResponseHostConnection(peer, (Network.Messages.Connection.ResponseHostConnectionMessage)msg);
					break;
				case (ushort)Network.Messages.Connection.CustomMessageType.RequestScreenshot:
					handleRequestScreenshot(peer, (Network.Messages.Connection.RequestScreenshotMessage)msg);
					break;
				case (ushort)Network.Messages.Connection.CustomMessageType.ResponseScreenshot:
					handleResponseScreenshot(peer, (Network.Messages.Connection.ResponseScreenshotMessage)msg);
					break;
			}

		}

		public void handleResponseScreenshot(NetPeer peer, Network.Messages.Connection.ResponseScreenshotMessage message)
		{
			NetPeer wpeer;
			if (_clientPeers.TryGetValue(message.ClientSystemId, out wpeer))
			{
				wpeer.Send(_messageHandler.encodeMessage(message), SendOptions.ReliableOrdered);
			}
		}

		public void handleRequestScreenshot(NetPeer peer, Network.Messages.Connection.RequestScreenshotMessage message)
		{
			NetPeer wpeer;
			if (_hostPeers.TryGetValue(message.HostSystemId, out wpeer))
			{
				wpeer.Send(_messageHandler.encodeMessage(message), SendOptions.Unreliable);
			}
		}

		public void handleResponseHostConnection(NetPeer peer, Network.Messages.Connection.ResponseHostConnectionMessage message)
		{
			NetPeer wpeer;
			if (_clientPeers.TryGetValue(message.ClientSystemId, out wpeer))
			{
				wpeer.Send(_messageHandler.encodeMessage(message), SendOptions.Unreliable);
			}
		}

		public void handleRequestHostConnection(NetPeer peer, Network.Messages.Connection.RequestHostConnectionMessage message)
		{
			NetPeer wpeer;
			if (_hostPeers.TryGetValue(message.HostSystemId, out wpeer))
			{
				wpeer.Send(_messageHandler.encodeMessage(message), SendOptions.Unreliable);
			}
			else
			{
				ResponseHostConnectionMessage messageNew = new ResponseHostConnectionMessage();
				messageNew.HostFound = false;
			}
		}

		public void handleRequestHostIntroducerRegistration(NetPeer peer, RequestHostIntroducerRegistrationMessage message)
		{
			Machine machine = new Machine();
			machine.SystemId = new Random().Next(0, 999999999).ToString();

			machine.SystemId = "123456789";

			this._hostPeers.Add(machine.SystemId, peer);

			ResponseHostIntroducerRegistrationMessage res = new ResponseHostIntroducerRegistrationMessage();
			res.Machine = machine;
			peer.Send(_messageHandler.encodeMessage(res), SendOptions.Unreliable);

		}

		public void handleRequestClientIntroducerRegistration(NetPeer peer, RequestClientIntroducerRegistrationMessage message)
		{
			Machine machine = new Machine();
			machine.SystemId = new Random().Next(0, 999999999).ToString();

			machine.SystemId = "987654321";

			this._clientPeers.Add(machine.SystemId, peer);

			ResponseClientIntroducerRegistrationMessage res = new ResponseClientIntroducerRegistrationMessage();
			res.Machine = machine;
			peer.Send(_messageHandler.encodeMessage(res), SendOptions.Unreliable);

		}


		public void OnNetworkReceiveUnconnected(NetEndPoint remoteEndPoint, NetDataReader reader, UnconnectedMessageType messageType)
		{
			Console.WriteLine("[Server] ReceiveUnconnected: {0}", reader.GetString(100));
		}

		public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
		{

		}
	}
}
