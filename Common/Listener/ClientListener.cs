using System;
using Common.EventArgs.Network;
using Common.EventArgs.Network.Client;
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
		public event EventHandler<OnlineCheckReceivedEventArgs> OnOnlineCheckReceived;
		public event EventHandler<HostInitalizeConnectedEventArgs> OnHostInitalizeConnected;

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
					handleResponseHostConnection(peer, (Network.Messages.Connection.Response.HostConnectionMessage)msg);
					break;
				case (ushort)Network.Messages.Connection.CustomMessageType.ResponseScreenshot:
					handleResponseScreenshotConnection(peer, (Network.Messages.Connection.ResponseScreenshotMessage)msg);
					break;
				case (ushort)Network.Messages.Connection.CustomMessageType.ResponseCheckOnline:
					handleCheckOnline(peer, (Network.Messages.Connection.ResponseCheckOnlineMessage)msg);
					break;
				case (ushort)Network.Messages.Connection.CustomMessageType.ResponseInitalizeHostConnection:
					handleResponseInitalizeHostConnection(peer, (Network.Messages.Connection.Response.InitalizeHostConnectionMessage)msg);
					break;
				case (ushort)Network.Messages.Connection.CustomMessageType.ResponseEmptyScreenshot:
					handleEmptyResponseScreenshot(peer, (Network.Messages.Connection.ResponseEmptyScreenshotMessage)msg);
					break;

			}

		}

		public void handleEmptyResponseScreenshot(NetPeer peer, Network.Messages.Connection.ResponseEmptyScreenshotMessage message)
		{
			if (OnScreenshotReceived != null)
				OnScreenshotReceived(this, new ScreenshotReceivedEventArgs()
				{
					Nothing = true,
					SystemId = message.HostSystemId
				});
		}

		public void handleResponseInitalizeHostConnection(NetPeer peer, Network.Messages.Connection.Response.InitalizeHostConnectionMessage message)
		{
			if (OnHostInitalizeConnected != null)
				OnHostInitalizeConnected(this, new HostInitalizeConnectedEventArgs() { HostSystemId = message.HostSystemId, ClientSystemId = message.ClientSystemId, HostPublicKey = message.PublicKey });
		}

		public void handleCheckOnline(NetPeer peer, Network.Messages.Connection.ResponseCheckOnlineMessage message)
		{
			if (OnOnlineCheckReceived != null)
				OnOnlineCheckReceived(this, new OnlineCheckReceivedEventArgs() { Peers = message.Peers });
		}

		public void handleResponseScreenshotConnection(NetPeer peer, Network.Messages.Connection.ResponseScreenshotMessage message)
		{
		if (OnScreenshotReceived != null)
			OnScreenshotReceived(this, new ScreenshotReceivedEventArgs()
			{
				Image = message.Image,
					Nothing = false,
					Bounds = message.Bounds,
					SystemId = message.HostSystemId
				});
		}

		public void handleResponseHostConnection(NetPeer peer, Network.Messages.Connection.Response.HostConnectionMessage message)
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
