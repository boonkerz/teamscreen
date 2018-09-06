using System;
using LiteNetLib;
using LiteNetLib.Utils;
using Messages;
using Messages.EventArgs.FileTransfer;
using Messages.EventArgs.Network;
using Messages.EventArgs.Network.Client;
using Network.Helper;
using Network.Manager;

namespace Network.Listener
{
	public class ClientListener : BaseListener
	{
		private readonly MessageHandler _messageHandler;
		public ClientManager _clientManager;

		public event EventHandler<ClientConnectedEventArgs> OnClientConnected;
        public event EventHandler<HostCloseEventArgs> OnHostClose;
        public event EventHandler<ScreenshotReceivedEventArgs> OnScreenshotReceived;
		public event EventHandler<HostInitalizeConnectedEventArgs> OnHostInitalizeConnected;
        public event EventHandler<OnlineCheckReceivedEventArgs> OnOnlineCheckReceived;
        public event EventHandler<FileTransferListingEventArgs> OnFileTransferListing;

        public ClientListener()
		{
			_messageHandler = new MessageHandler(MessageHandler.ManagerModus.Client, _clientManager);
		}

        public void SetManager(ClientManager Manager)
        {
            this._clientManager = Manager;
            _messageHandler.Manager = Manager;
        }

        public override void OnNetworkReceive(NetPeer peer, NetDataReader reader)
		{
            base.OnNetworkReceive(peer, reader);
			Message msg = _messageHandler.decodeMessage(reader);

			switch (msg.MessageType)
			{
				case (ushort)Messages.System.CustomMessageType.ResponseClientIntroducerRegistration:
					handleResponseClientIntroducerRegistration(peer, (Messages.System.ResponseClientIntroducerRegistrationMessage)msg);
					break;
				case (ushort)Messages.Connection.CustomMessageType.ResponseHostConnection:
					handleResponseHostConnection(peer, (Messages.Connection.Response.HostConnectionMessage)msg);
					break;
				case (ushort)Messages.Connection.CustomMessageType.ResponseScreenshot:
					handleResponseScreenshotConnection(peer, (Messages.Connection.ResponseScreenshotMessage)msg);
					break;
				case (ushort)Messages.Connection.CustomMessageType.ResponseInitalizeHostConnection:
					handleResponseInitalizeHostConnection(peer, (Messages.Connection.Response.InitalizeHostConnectionMessage)msg);
					break;
				case (ushort)Messages.Connection.CustomMessageType.ResponseEmptyScreenshot:
					handleEmptyResponseScreenshot(peer, (Messages.Connection.ResponseEmptyScreenshotMessage)msg);
					break;
                case (ushort)Messages.Connection.CustomMessageType.ResponseCloseHostConnection:
                    handleResponseCloseHostConnection(peer, (Messages.Connection.Response.CloseHostConnectionMessage)msg);
                    break;
                case (ushort)Messages.Connection.CustomMessageType.ResponseCheckOnline:
                    handleCheckOnline(peer, (Messages.Connection.ResponseCheckOnlineMessage)msg);
                    break;
                case (ushort)Messages.FileTransfer.CustomMessageType.ResponseListing:
                    handleFileResponseListing(peer, (Messages.FileTransfer.Response.ListingMessage)msg);
                    break;
                case (ushort)Messages.FileTransfer.CustomMessageType.RequestCopy:
                    handleFileCopy(peer, (Messages.FileTransfer.Request.CopyMessage)msg);
                    break;
            }

		}

        public void handleFileCopy(NetPeer peer, Messages.FileTransfer.Request.CopyMessage message)
        {
            
        }

        public void handleFileResponseListing(NetPeer peer, Messages.FileTransfer.Response.ListingMessage message)
        {
            if (OnFileTransferListing != null)
                OnFileTransferListing(this, new FileTransferListingEventArgs() { Parent = message.Parent, ParentPath = message.ParentPath, HostSystemId = message.HostSystemId, ClientSystemId = message.ClientSystemId, Entrys = message.Entrys, ActFolder = message.ActFolder });
        }

        public void handleCheckOnline(NetPeer peer, Messages.Connection.ResponseCheckOnlineMessage message)
        {
            if (OnOnlineCheckReceived != null)
                OnOnlineCheckReceived(this, new OnlineCheckReceivedEventArgs() { Peers = message.Peers });
        }

        public void handleResponseCloseHostConnection(NetPeer peer, Messages.Connection.Response.CloseHostConnectionMessage message)
        {
            if (OnHostClose != null)
                OnHostClose(this, new HostCloseEventArgs() { HostSystemId = message.HostSystemId, ClientSystemId = message.ClientSystemId });
        }

        public void handleEmptyResponseScreenshot(NetPeer peer, Messages.Connection.ResponseEmptyScreenshotMessage message)
		{
			if (OnScreenshotReceived != null)
				OnScreenshotReceived(this, new ScreenshotReceivedEventArgs()
				{
					Nothing = true,
					SystemId = message.HostSystemId
				});
		}

		public void handleResponseInitalizeHostConnection(NetPeer peer, Messages.Connection.Response.InitalizeHostConnectionMessage message)
		{
            var symmetricKey = Guid.NewGuid().ToString().Replace("-", string.Empty);

            this._clientManager.SaveRemotePublicKey(message.HostSystemId, message.HostPublicKey);
            this._clientManager.SaveSymmetricKey(message.HostSystemId, symmetricKey);
            
            if (OnHostInitalizeConnected != null)
				OnHostInitalizeConnected(this, new HostInitalizeConnectedEventArgs() { HostSystemId = message.HostSystemId, ClientSystemId = message.ClientSystemId, HostPublicKey = message.HostPublicKey });
		}

		public void handleResponseScreenshotConnection(NetPeer peer, Messages.Connection.ResponseScreenshotMessage message)
		{
		if (OnScreenshotReceived != null)
			OnScreenshotReceived(this, new ScreenshotReceivedEventArgs()
			{
				Image = message.Image,
				Nothing = false,
				Bounds = message.Bounds,
				SystemId = message.HostSystemId,
                Fullscreen = message.Fullscreen
			});
		}

		public void handleResponseHostConnection(NetPeer peer, Messages.Connection.Response.HostConnectionMessage message)
		{
			if (OnClientConnected != null)
				OnClientConnected(this, new ClientConnectedEventArgs() { SystemId = message.HostSystemId, PasswordOk = message.PasswordOk });
		}

		public void handleResponseClientIntroducerRegistration(NetPeer peer, Messages.System.ResponseClientIntroducerRegistrationMessage message)
		{
			this._clientManager.SystemId = message.Machine.SystemId;
		}

	}
}
