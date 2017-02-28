using System;
using Common.EventArgs.Network;
using LiteNetLib;
using LiteNetLib.Utils;
using Network;

namespace Common.Listener
{
	public class ClientListener : INetEventListener
	{
		private readonly MessageHandler _messageHandler;
		public ClientManager _clientManager;

		public event EventHandler<ConnectedEventArgs> OnConnected;

		public event EventHandler<ClientConnectedEventArgs> OnClientConnected;

		public event EventHandler<ScreenshotReceivedEventArgs> OnScreenshotReceived;

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
				case (ushort)Network.Messages.Connection.CustomMessageType.ResponseScreenshot:
					handleResponseScreenshotConnection(peer, (Network.Messages.Connection.ResponseScreenshotMessage)msg);
					break;
			}

		}
		public void handleResponseScreenshotConnection(NetPeer peer, Network.Messages.Connection.ResponseScreenshotMessage message)
		{
			if (OnScreenshotReceived != null)
				OnScreenshotReceived(this, new ScreenshotReceivedEventArgs() { Image = message.Image });
		}

		public void handleResponseHostConnection(NetPeer peer, Network.Messages.Connection.ResponseHostConnectionMessage message)
		{
			if (OnClientConnected != null)
				OnClientConnected(this, new ClientConnectedEventArgs() { SystemId = message.HostSystemId, PasswordOk = message.PasswordOk });
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
}
