using System;
using System.Collections.Generic;
using System.Linq;
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
			foreach (var item in _hostPeers.Where(kvp => kvp.Value == peer).ToList())
			{
				_hostPeers.Remove(item.Key);
			}

			foreach (var item in _clientPeers.Where(kvp => kvp.Value == peer).ToList())
			{
				_hostPeers.Remove(item.Key);
			}
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
					handleRequestHostConnection(peer, (Network.Messages.Connection.Request.HostConnectionMessage)msg);
					break;
				case (ushort)Network.Messages.Connection.CustomMessageType.ResponseHostConnection:
					handleResponseHostConnection(peer, (Network.Messages.Connection.Response.HostConnectionMessage)msg);
					break;
				case (ushort)Network.Messages.Connection.CustomMessageType.RequestScreenshot:
					handleRequestScreenshot(peer, (Network.Messages.Connection.RequestScreenshotMessage)msg);
					break;
				case (ushort)Network.Messages.Connection.CustomMessageType.ResponseScreenshot:
					handleResponseScreenshot(peer, (Network.Messages.Connection.ResponseScreenshotMessage)msg);
					break;
				case (ushort)Network.Messages.Connection.CustomMessageType.MouseClick:
					handleMouseClickEvent(peer, (Network.Messages.Connection.MouseClickMessage)msg);
					break;
				case (ushort)Network.Messages.Connection.CustomMessageType.MouseMove:
					handleMouseMoveEvent(peer, (Network.Messages.Connection.MouseMoveMessage)msg);
					break;
				case (ushort)Network.Messages.Connection.CustomMessageType.RequestCheckOnline:
					handleCheckOnline(peer, (Network.Messages.Connection.RequestCheckOnlineMessage)msg);
					break;
				case (ushort)Network.Messages.Connection.CustomMessageType.RequestInitalizeHostConnection:
					handleRequestInitalizeHostConnection(peer, (Network.Messages.Connection.Request.InitalizeHostConnectionMessage)msg);
					break;
				case (ushort)Network.Messages.Connection.CustomMessageType.ResponseInitalizeHostConnection:
					handleResponseInitalizeHostConnection(peer, (Network.Messages.Connection.Response.InitalizeHostConnectionMessage)msg);
					break;
				case (ushort)Network.Messages.Connection.CustomMessageType.KeyPress:
					handleKeyPress(peer, (Network.Messages.Connection.OneWay.KeyPressMessage)msg);
					break;
				case (ushort)Network.Messages.Connection.CustomMessageType.KeyRelease:
					handleKeyRelease(peer, (Network.Messages.Connection.OneWay.KeyReleaseMessage)msg);
					break;
			}

		}

		public void handleKeyRelease(NetPeer peer, Network.Messages.Connection.OneWay.KeyReleaseMessage message)
		{
			NetPeer wpeer;
			if (_hostPeers.TryGetValue(message.HostSystemId, out wpeer))
			{
				wpeer.Send(_messageHandler.encodeMessage(message), SendOptions.Unreliable);
			}
		}

		public void handleKeyPress(NetPeer peer, Network.Messages.Connection.OneWay.KeyPressMessage message)
		{
			NetPeer wpeer;
			if (_hostPeers.TryGetValue(message.HostSystemId, out wpeer))
			{
				wpeer.Send(_messageHandler.encodeMessage(message), SendOptions.Unreliable);
			}
		}

		public void handleResponseInitalizeHostConnection(NetPeer peer, Network.Messages.Connection.Response.InitalizeHostConnectionMessage message)
		{
			NetPeer wpeer;
			if (_clientPeers.TryGetValue(message.ClientSystemId, out wpeer))
			{
				wpeer.Send(_messageHandler.encodeMessage(message), SendOptions.Unreliable);
			}
		}

		public void handleRequestInitalizeHostConnection(NetPeer peer, Network.Messages.Connection.Request.InitalizeHostConnectionMessage message)
		{
			NetPeer wpeer;
			if (_hostPeers.TryGetValue(message.HostSystemId, out wpeer))
			{
				wpeer.Send(_messageHandler.encodeMessage(message), SendOptions.Unreliable);
			}
			else
			{
				Network.Messages.Connection.Response.InitalizeHostConnectionMessage messageNew = new Network.Messages.Connection.Response.InitalizeHostConnectionMessage();
				messageNew.HostFound = false;
			}
		}

		public void handleCheckOnline(NetPeer peer, Network.Messages.Connection.RequestCheckOnlineMessage message)
		{
			List<Model.Peer> peers = message.Peers;
			foreach (Model.Peer pr in peers)
			{
				if (_hostPeers.ContainsKey(pr.SystemId))
				{
					pr.isOnline = true;
				}
			}

			peer.Send(_messageHandler.encodeMessage(new ResponseCheckOnlineMessage { Peers = peers }), SendOptions.Unreliable);
		}

		public void handleMouseClickEvent(NetPeer peer, Network.Messages.Connection.MouseClickMessage message)
		{
			NetPeer wpeer;
			if (_hostPeers.TryGetValue(message.HostSystemId, out wpeer))
			{
				wpeer.Send(_messageHandler.encodeMessage(message), SendOptions.Unreliable);
			}
		}

		public void handleMouseMoveEvent(NetPeer peer, Network.Messages.Connection.MouseMoveMessage message)
		{
			NetPeer wpeer;
			if (_hostPeers.TryGetValue(message.HostSystemId, out wpeer))
			{
				wpeer.Send(_messageHandler.encodeMessage(message), SendOptions.Unreliable);
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

		public void handleResponseHostConnection(NetPeer peer, Network.Messages.Connection.Response.HostConnectionMessage message)
		{
			NetPeer wpeer;
			if (_clientPeers.TryGetValue(message.ClientSystemId, out wpeer))
			{
				wpeer.Send(_messageHandler.encodeMessage(message), SendOptions.Unreliable);
			}
		}

		public void handleRequestHostConnection(NetPeer peer, Network.Messages.Connection.Request.HostConnectionMessage message)
		{
			NetPeer wpeer;
			if (_hostPeers.TryGetValue(message.HostSystemId, out wpeer))
			{
				wpeer.Send(_messageHandler.encodeMessage(message), SendOptions.Unreliable);
			}

		}

		public void handleRequestHostIntroducerRegistration(NetPeer peer, RequestHostIntroducerRegistrationMessage message)
		{
			Machine machine = new Machine();
			if (message.SystemId != null && message.SystemId != "")
			{
				machine.SystemId = message.SystemId;
			}
			else
			{
				machine.SystemId = new Random().Next(0, 999999999).ToString();
			}

			if (this._hostPeers.ContainsKey(machine.SystemId))
			{
				this._hostPeers.Remove(machine.SystemId);
			}
			this._hostPeers.Add(machine.SystemId, peer);

			ResponseHostIntroducerRegistrationMessage res = new ResponseHostIntroducerRegistrationMessage();
			res.Machine = machine;
			peer.Send(_messageHandler.encodeMessage(res), SendOptions.Unreliable);

		}

		public void handleRequestClientIntroducerRegistration(NetPeer peer, RequestClientIntroducerRegistrationMessage message)
		{
			Machine machine = new Machine();
			machine.SystemId = new Random().Next(0, 999999999).ToString();

			if (this._hostPeers.ContainsKey(machine.SystemId))
			{
				this._clientPeers.Remove(machine.SystemId);
			}
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
