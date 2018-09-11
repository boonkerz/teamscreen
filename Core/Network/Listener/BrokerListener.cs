using System;
using System.Collections.Generic;
using System.Linq;
using LiteNetLib;
using LiteNetLib.Utils;
using Messages;
using Messages.Connection;
using Messages.System;
using Model;
using Network.Listener;
using Network.Helper;
using Utils;

namespace Network.Listener
{
	public class BrokerListener : BaseListener
    {
		public NetManager Server;
		private readonly MessageHandler _messageHandler;
		private readonly Dictionary<string, NetPeer> _hostPeers = new Dictionary<string, NetPeer>();

		private readonly Dictionary<string, NetPeer> _clientPeers = new Dictionary<string, NetPeer>();

		public BrokerListener()
		{
			_messageHandler = new MessageHandler(MessageHandler.ManagerModus.Broker);

		}

        public override void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
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

        public override void OnNetworkReceive(NetPeer peer, NetDataReader reader)
		{
			Message msg = _messageHandler.decodeMessage(reader);

			switch (msg.MessageType)
			{
				case (ushort)Messages.System.CustomMessageType.RequestHostIntroducerRegistration:
					handleRequestHostIntroducerRegistration(peer, (RequestHostIntroducerRegistrationMessage)msg);
					break;
				case (ushort)Messages.System.CustomMessageType.RequestClientIntroducerRegistration:
					handleRequestClientIntroducerRegistration(peer, (RequestClientIntroducerRegistrationMessage)msg);
					break;
				case (ushort)Messages.Connection.CustomMessageType.RequestHostConnection:
					handleRequestHostConnection(peer, (Messages.Connection.Request.HostConnectionMessage)msg);
					break;
				case (ushort)Messages.Connection.CustomMessageType.ResponseHostConnection:
					handleResponseHostConnection(peer, (Messages.Connection.Response.HostConnectionMessage)msg);
					break;
				case (ushort)Messages.Connection.CustomMessageType.RequestScreenshot:
					handleRequestScreenshot(peer, (Messages.Connection.RequestScreenshotMessage)msg);
					break;
				case (ushort)Messages.Connection.CustomMessageType.StartScreenSharing:
					handleStartScreenSharing(peer, (Messages.Connection.StartScreenSharingMessage)msg);
					break;
				case (ushort)Messages.Connection.CustomMessageType.StopScreenSharing:
					handleStopScreenSharing(peer, (Messages.Connection.StopScreenSharingMessage)msg);
					break;
				case (ushort)Messages.Connection.CustomMessageType.ResponseScreenshot:
					handleResponseScreenshot(peer, (Messages.Connection.ResponseScreenshotMessage)msg);
					break;
				case (ushort)Messages.Connection.CustomMessageType.MouseClick:
					handleMouseClickEvent(peer, (Messages.Connection.OneWay.MouseClickMessage)msg);
					break;
				case (ushort)Messages.Connection.CustomMessageType.MouseMove:
					handleMouseMoveEvent(peer, (Messages.Connection.OneWay.MouseMoveMessage)msg);
					break;
				case (ushort)Messages.Connection.CustomMessageType.RequestInitalizeHostConnection:
					handleRequestInitalizeHostConnection(peer, (Messages.Connection.Request.InitalizeHostConnectionMessage)msg);
					break;
				case (ushort)Messages.Connection.CustomMessageType.ResponseInitalizeHostConnection:
					handleResponseInitalizeHostConnection(peer, (Messages.Connection.Response.InitalizeHostConnectionMessage)msg);
					break;
				case (ushort)Messages.Connection.CustomMessageType.Key:
					handleKey(peer, (Messages.Connection.OneWay.KeyMessage)msg);
					break;
				case (ushort)Messages.Connection.CustomMessageType.ResponseEmptyScreenshot:
					handleEmptyResponseScreenshot(peer, (Messages.Connection.ResponseEmptyScreenshotMessage)msg);
					break;
                case (ushort)Messages.Connection.CustomMessageType.RequestCloseHostConnection:
                    handleRequestClientHost(peer, (Messages.Connection.Request.CloseHostConnectionMessage)msg);
                    break;
                case (ushort)Messages.Connection.CustomMessageType.ResponseCloseHostConnection:
                    handleResponseHostClient(peer, (Messages.Connection.Response.CloseHostConnectionMessage)msg);
                    break;
                case (ushort)Messages.Connection.CustomMessageType.RequestCheckOnline:
                    handleCheckOnline(peer, (Messages.Connection.RequestCheckOnlineMessage)msg);
                    break;
                case (ushort)Messages.Connection.CustomMessageType.DisconnectFromIntroducer:
                    handleDisconnect(peer, (Messages.Connection.OneWay.DisconnectFromIntroducerMessage)msg);
                    break;
                case (ushort)Messages.FileTransfer.CustomMessageType.RequestListing:
                    handleRequestClientHost(peer, (Messages.FileTransfer.Request.ListingMessage)msg);
                    break;
                case (ushort)Messages.FileTransfer.CustomMessageType.ResponseListing:
                    handleResponseHostClient(peer, (Messages.FileTransfer.Response.ListingMessage)msg);
                    break;
                case (ushort)Messages.FileTransfer.CustomMessageType.RequestCopy:
                    handleRequestClientHost(peer, (Messages.FileTransfer.Request.CopyMessage)msg);
                    break;
            }
        }


        public void handleDisconnect(NetPeer peer, Messages.Connection.OneWay.DisconnectFromIntroducerMessage message)
        {
            foreach (var item in _hostPeers.Where(kvp => kvp.Key == message.SystemId).ToList())
            {
                _hostPeers.Remove(item.Key);
            }

            foreach (var item in _clientPeers.Where(kvp => kvp.Key == message.SystemId).ToList())
            {
                _hostPeers.Remove(item.Key);
            }
        }

        public void handleCheckOnline(NetPeer peer, Messages.Connection.RequestCheckOnlineMessage message)
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

        public void handleRequestClientHost(NetPeer peer, BaseMessage message)
        {
            NetPeer wpeer;
            if (_hostPeers.TryGetValue(message.HostSystemId, out wpeer))
            {
                wpeer.Send(_messageHandler.encodeMessage(message), SendOptions.ReliableOrdered);
            }
        }

        public void handleResponseHostClient(NetPeer peer, BaseMessage message)
        {
            NetPeer wpeer;
            if (_clientPeers.TryGetValue(message.ClientSystemId, out wpeer))
            {
                wpeer.Send(_messageHandler.encodeMessage(message), SendOptions.ReliableOrdered);
            }
        }

        public void handleKey(NetPeer peer, Messages.Connection.OneWay.KeyMessage message)
		{
			NetPeer wpeer;
			if (_hostPeers.TryGetValue(message.HostSystemId, out wpeer))
			{
				wpeer.Send(_messageHandler.encodeMessage(message), SendOptions.Unreliable);
			}
		}

		public void handleResponseInitalizeHostConnection(NetPeer peer, Messages.Connection.Response.InitalizeHostConnectionMessage message)
		{
			NetPeer wpeer;
			if (_clientPeers.TryGetValue(message.ClientSystemId, out wpeer))
			{
				wpeer.Send(_messageHandler.encodeMessage(message), SendOptions.Unreliable);
			}
		}

		public void handleRequestInitalizeHostConnection(NetPeer peer, Messages.Connection.Request.InitalizeHostConnectionMessage message)
		{
			NetPeer wpeer;
			if (_hostPeers.TryGetValue(message.HostSystemId, out wpeer))
			{
				wpeer.Send(_messageHandler.encodeMessage(message), SendOptions.Unreliable);
			}
			else
			{
				Messages.Connection.Response.InitalizeHostConnectionMessage messageNew = new Messages.Connection.Response.InitalizeHostConnectionMessage();
				messageNew.HostFound = false;
			}
		}

		public void handleMouseClickEvent(NetPeer peer, Messages.Connection.OneWay.MouseClickMessage message)
		{
			NetPeer wpeer;
			if (_hostPeers.TryGetValue(message.HostSystemId, out wpeer))
			{
				wpeer.Send(_messageHandler.encodeMessage(message), SendOptions.Unreliable);
			}
		}

		public void handleMouseMoveEvent(NetPeer peer, Messages.Connection.OneWay.MouseMoveMessage message)
		{
			NetPeer wpeer;
			if (_hostPeers.TryGetValue(message.HostSystemId, out wpeer))
			{
				wpeer.Send(_messageHandler.encodeMessage(message), SendOptions.Unreliable);
			}
		}

		public void handleEmptyResponseScreenshot(NetPeer peer, Messages.Connection.ResponseEmptyScreenshotMessage message)
		{
			NetPeer wpeer;
			if (_clientPeers.TryGetValue(message.ClientSystemId, out wpeer))
			{
				wpeer.Send(_messageHandler.encodeMessage(message), SendOptions.ReliableOrdered);
			}
		}

		public void handleResponseScreenshot(NetPeer peer, Messages.Connection.ResponseScreenshotMessage message)
		{
			NetPeer wpeer;
			if (_clientPeers.TryGetValue(message.ClientSystemId, out wpeer))
			{
				wpeer.Send(_messageHandler.encodeMessage(message), SendOptions.ReliableOrdered);
			}
		}

		public void handleRequestScreenshot(NetPeer peer, Messages.Connection.RequestScreenshotMessage message)
		{
			NetPeer wpeer;
			if (_hostPeers.TryGetValue(message.HostSystemId, out wpeer))
			{
				wpeer.Send(_messageHandler.encodeMessage(message), SendOptions.Unreliable);
			}
		}

		public void handleStartScreenSharing(NetPeer peer, Messages.Connection.StartScreenSharingMessage message)
		{
			NetPeer wpeer;
			if (_hostPeers.TryGetValue(message.HostSystemId, out wpeer))
			{
				wpeer.Send(_messageHandler.encodeMessage(message), SendOptions.Unreliable);
			}
		}

		public void handleStopScreenSharing(NetPeer peer, Messages.Connection.StopScreenSharingMessage message)
		{
			NetPeer wpeer;
			if (_hostPeers.TryGetValue(message.HostSystemId, out wpeer))
			{
				wpeer.Send(_messageHandler.encodeMessage(message), SendOptions.Unreliable);
			}
		}

		public void handleResponseHostConnection(NetPeer peer, Messages.Connection.Response.HostConnectionMessage message)
		{
			NetPeer wpeer;
			if (_clientPeers.TryGetValue(message.ClientSystemId, out wpeer))
			{
				wpeer.Send(_messageHandler.encodeMessage(message), SendOptions.Unreliable);
			}
		}

		public void handleRequestHostConnection(NetPeer peer, Messages.Connection.Request.HostConnectionMessage message)
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

			if (this._clientPeers.ContainsKey(machine.SystemId))
			{
				this._clientPeers.Remove(machine.SystemId);
			}
			this._clientPeers.Add(machine.SystemId, peer);

			ResponseClientIntroducerRegistrationMessage res = new ResponseClientIntroducerRegistrationMessage();
			res.Machine = machine;
			peer.Send(_messageHandler.encodeMessage(res), SendOptions.Unreliable);

		}
	}
}
