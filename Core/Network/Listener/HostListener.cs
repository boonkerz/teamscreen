using System;
using LiteNetLib;
using LiteNetLib.Utils;
using Messages;
using Messages.EventArgs.FileTransfer;
using Messages.EventArgs.Network;
using Messages.EventArgs.Network.Host;
using Messages.FileTransfer.Request;
using Network.Helper;
using Network.Manager;

namespace Network.Listener
{
	public class HostListener : BaseListener
    {
		private readonly MessageHandler _messageHandler;
		public HostManager _hostManager;

		public event EventHandler<ConnectedEventArgs> OnConnected;
		public event EventHandler<ClientConnectedEventArgs> OnClientConnected;
		public event EventHandler<ClientInitalizeConnectedEventArgs> OnClientInitalizeConnected;
        public event EventHandler<ClientCloseEventArgs> OnClientClose;
        public event EventHandler<ScreenshotRequestEventArgs> OnScreenshotRequest;
		public event EventHandler<StartScreenSharingEventArgs> OnStartScreenSharing;
        public event EventHandler<ClientAliveEventArgs> OnClientAlive;
        public event EventHandler<StopScreenSharingEventArgs> OnStopScreenSharing;
		public event EventHandler<MouseMoveEventArgs> OnMouseMove;
		public event EventHandler<MouseClickEventArgs> OnMouseClick;
		public event EventHandler<KeyEventArgs> OnKey;
        public event EventHandler<FileTransferListingEventArgs> OnFileTransferListing;
        public event EventHandler<FileTransferCopyEventArgs> OnFileTransferCopy;

        public HostListener()
		{
			_messageHandler = new MessageHandler(MessageHandler.ManagerModus.Host, _hostManager);
		}

        public void SetManager(HostManager Manager)
        {
            this._hostManager = Manager;
            _messageHandler.Manager = Manager;
        }

        public override void OnNetworkReceive(NetPeer peer, NetDataReader reader)
		{
			Message msg = _messageHandler.decodeMessage(reader);
			switch (msg.MessageType)
			{
				case (ushort)Messages.System.CustomMessageType.ResponseHostIntroducerRegistration:
					handleResponseHostIntroducerRegistration(peer, (Messages.System.ResponseHostIntroducerRegistrationMessage)msg);
					break;
				case (ushort)Messages.Connection.CustomMessageType.RequestHostConnection:
					handleRequestHostConnection(peer, (Messages.Connection.Request.HostConnectionMessage)msg);
					break;
				case (ushort)Messages.Connection.CustomMessageType.RequestInitalizeHostConnection:
					handleRequestInitalizeHostConnection(peer, (Messages.Connection.Request.InitalizeHostConnectionMessage)msg);
					break;
				case (ushort)Messages.Connection.CustomMessageType.RequestScreenshot:
					handleRequestScreenshot(peer, (Messages.Connection.RequestScreenshotMessage)msg);
					break;
				case (ushort)Messages.Connection.CustomMessageType.StartScreenSharing:
					handleStartScreenSharing(peer, (Messages.Connection.StartScreenSharingMessage)msg);
					break;
                case (ushort)Messages.Connection.CustomMessageType.ClientAlive:
                    handleClientAlive(peer, (Messages.Connection.ClientAliveMessage)msg);
                    break;
                case (ushort)Messages.Connection.CustomMessageType.StopScreenSharing:
					handleStopScreenSharing(peer, (Messages.Connection.StopScreenSharingMessage)msg);
					break;
				case (ushort)Messages.Connection.CustomMessageType.MouseMove:
					handleMouseMove(peer, (Messages.Connection.OneWay.MouseMoveMessage)msg);
					break;
				case (ushort)Messages.Connection.CustomMessageType.MouseClick:
					handleMouseClick(peer, (Messages.Connection.OneWay.MouseClickMessage)msg);
					break;
				case (ushort)Messages.Connection.CustomMessageType.Key:
					handleKeyPress(peer, (Messages.Connection.OneWay.KeyMessage)msg);
					break;
                case (ushort)Messages.Connection.CustomMessageType.RequestCloseHostConnection:
                    handleRequestClientCloseConnection(peer, (Messages.Connection.Request.CloseHostConnectionMessage)msg);
                    break;
                case (ushort)Messages.FileTransfer.CustomMessageType.RequestListing:
                    handleListing(peer, (Messages.FileTransfer.Request.ListingMessage)msg);
                    break;
                case (ushort)Messages.FileTransfer.CustomMessageType.RequestCopy:
                    handleCopyMessage(peer, (Messages.FileTransfer.Request.CopyMessage)msg);
                    break;

            }

        }

        private void handleCopyMessage(NetPeer peer, CopyMessage msg)
        {
            if (OnFileTransferCopy != null)
                OnFileTransferCopy(this, new FileTransferCopyEventArgs { HostSystemId = msg.HostSystemId, ClientSystemId = msg.ClientSystemId, Folder = msg.Folder, Data = msg.Data, Name = msg.Name });
        }

        public void handleListing(NetPeer peer, Messages.FileTransfer.Request.ListingMessage message)
        {
            if (OnFileTransferListing != null)
                OnFileTransferListing(this, new FileTransferListingEventArgs { HostSystemId = message.HostSystemId, ClientSystemId = message.ClientSystemId, Folder = message.Folder });
        }

        public void handleRequestClientCloseConnection(NetPeer peer, Messages.Connection.Request.CloseHostConnectionMessage message)
        {
            if (OnClientClose != null)
                OnClientClose(this, new ClientCloseEventArgs { HostSystemId = message.HostSystemId, ClientSystemId = message.ClientSystemId });
        }


        public void handleKeyPress(NetPeer peer, Messages.Connection.OneWay.KeyMessage message)
		{
			if (OnKey != null)
				OnKey(this, new KeyEventArgs { Key = message.Key, Alt = message.Alt, Control = message.Control, Shift = message.Shift, Mode = message.Mode });
		}

		public void handleRequestInitalizeHostConnection(NetPeer peer, Messages.Connection.Request.InitalizeHostConnectionMessage message)
		{
            this._hostManager.SaveRemotePublicKey(message.ClientSystemId, message.ClientPublicKey);
            var pair = this._hostManager.CreateNewKeyPairKey(message.ClientSystemId);

            Messages.Connection.Response.InitalizeHostConnectionMessage rs = new Messages.Connection.Response.InitalizeHostConnectionMessage();
            rs.HostSystemId = this._hostManager.SystemId;
            rs.ClientSystemId = message.ClientSystemId;
            rs.HostPublicKey = pair.PublicKey;

            this._hostManager.sendMessage(rs);
            if (OnClientInitalizeConnected != null)
				OnClientInitalizeConnected(this, new ClientInitalizeConnectedEventArgs { HostSystemId = message.HostSystemId, ClientSystemId = message.ClientSystemId, ClientPublicKey = message.ClientPublicKey });
		}

		public void handleMouseMove(NetPeer peer, Messages.Connection.OneWay.MouseMoveMessage message)
		{
			if (OnMouseMove != null)
				OnMouseMove(this, new MouseMoveEventArgs { X = message.X, Y = message.Y });
		}

		public void handleMouseClick(NetPeer peer, Messages.Connection.OneWay.MouseClickMessage message)
		{
			if (OnMouseClick != null)
				OnMouseClick(this, new MouseClickEventArgs { Down = message.Down, Up = message.Up, DoubleClick = message.DoubleClick, X = message.X, Y = message.Y, Button = (MouseClickEventArgs.ButtonType)message.Button });
		}

		public void handleRequestScreenshot(NetPeer peer, Messages.Connection.RequestScreenshotMessage message)
		{
			if (OnScreenshotRequest != null)
				OnScreenshotRequest(this, new ScreenshotRequestEventArgs() { HostSystemId = message.HostSystemId, ClientSystemId = message.ClientSystemId, Fullscreen = message.Fullscreen });

		}

		public void handleStartScreenSharing(NetPeer peer, Messages.Connection.StartScreenSharingMessage message)
		{
			if (OnStartScreenSharing != null)
				OnStartScreenSharing(this, new StartScreenSharingEventArgs() { HostSystemId = message.HostSystemId, ClientSystemId = message.ClientSystemId });

		}

        public void handleClientAlive(NetPeer peer, Messages.Connection.ClientAliveMessage message)
        {
            if (OnClientAlive != null)
                OnClientAlive(this, new ClientAliveEventArgs() { HostSystemId = message.HostSystemId, ClientSystemId = message.ClientSystemId });

        }

        public void handleStopScreenSharing(NetPeer peer, Messages.Connection.StopScreenSharingMessage message)
		{
			if (OnStopScreenSharing != null)
				OnStopScreenSharing(this, new StopScreenSharingEventArgs() { HostSystemId = message.HostSystemId, ClientSystemId = message.ClientSystemId });

		}

		public void handleResponseHostIntroducerRegistration(NetPeer peer, Messages.System.ResponseHostIntroducerRegistrationMessage message)
		{
			this._hostManager.SystemId = message.Machine.SystemId;
			if (OnConnected != null)
				OnConnected(this, new ConnectedEventArgs() { SystemId = message.Machine.SystemId });
		}

		public void handleRequestHostConnection(NetPeer peer, Messages.Connection.Request.HostConnectionMessage message)
		{
			var res = new Messages.Connection.Response.HostConnectionMessage();
			res.HostSystemId = message.HostSystemId;
			res.ClientSystemId = message.ClientSystemId;
			res.PasswordOk = false;
			var decrypt = _hostManager.Decode(message.ClientSystemId, message.Password);
			if (this._hostManager.Password == decrypt)
			{
				res.PasswordOk = true;
				var symmetricKey = _hostManager.Decode(message.ClientSystemId, message.SymmetricKey);
				_hostManager.SaveSymmetricKey(message.ClientSystemId, symmetricKey);
			}
			peer.Send(_messageHandler.encodeMessage(res), SendOptions.Unreliable);
			if (OnClientConnected != null)
				OnClientConnected(this, new ClientConnectedEventArgs() { SystemId = res.ClientSystemId, PasswordOk = res.PasswordOk });
		}

	}
}
