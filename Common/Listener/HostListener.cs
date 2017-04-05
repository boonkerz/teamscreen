using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Common.EventArgs.Network;
using Common.EventArgs.Network.Host;
using LiteNetLib;
using LiteNetLib.Utils;
using Network;
using Network.Messages.Connection;
using Common.EventArgs.FileTransfer;

namespace Common.Listener
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
		public event EventHandler<MouseMoveEventArgs> OnMouseMove;
		public event EventHandler<MouseClickEventArgs> OnMouseClick;
		public event EventHandler<KeyEventArgs> OnKey;
        public event EventHandler<FileTransferListingEventArgs> OnFileTransferListing;


        public HostListener()
		{
			_messageHandler = new MessageHandler();
		}

        public override void OnNetworkReceive(NetPeer peer, NetDataReader reader)
		{
			Message msg = _messageHandler.decodeMessage(reader);
			switch (msg.MessageType)
			{
				case (ushort)Network.Messages.System.CustomMessageType.ResponseHostIntroducerRegistration:
					handleResponseHostIntroducerRegistration(peer, (Network.Messages.System.ResponseHostIntroducerRegistrationMessage)msg);
					break;
				case (ushort)Network.Messages.Connection.CustomMessageType.RequestHostConnection:
					handleRequestHostConnection(peer, (Network.Messages.Connection.Request.HostConnectionMessage)msg);
					break;
				case (ushort)Network.Messages.Connection.CustomMessageType.RequestInitalizeHostConnection:
					handleRequestInitalizeHostConnection(peer, (Network.Messages.Connection.Request.InitalizeHostConnectionMessage)msg);
					break;
				case (ushort)Network.Messages.Connection.CustomMessageType.RequestScreenshot:
					handleRequestScreenshot(peer, (Network.Messages.Connection.RequestScreenshotMessage)msg);
					break;
				case (ushort)Network.Messages.Connection.CustomMessageType.MouseMove:
					handleMouseMove(peer, (Network.Messages.Connection.OneWay.MouseMoveMessage)msg);
					break;
				case (ushort)Network.Messages.Connection.CustomMessageType.MouseClick:
					handleMouseClick(peer, (Network.Messages.Connection.OneWay.MouseClickMessage)msg);
					break;
				case (ushort)Network.Messages.Connection.CustomMessageType.Key:
					handleKeyPress(peer, (Network.Messages.Connection.OneWay.KeyMessage)msg);
					break;
                case (ushort)Network.Messages.Connection.CustomMessageType.RequestCloseHostConnection:
                    handleRequestClientCloseConnection(peer, (Network.Messages.Connection.Request.CloseHostConnectionMessage)msg);
                    break;
                case (ushort)Network.Messages.FileTransfer.CustomMessageType.RequestListing:
                    handleListing(peer, (Network.Messages.FileTransfer.Request.ListingMessage)msg);
                    break;

            }

        }

        public void handleListing(NetPeer peer, Network.Messages.FileTransfer.Request.ListingMessage message)
        {
            if (OnFileTransferListing != null)
                OnFileTransferListing(this, new FileTransferListingEventArgs { HostSystemId = message.HostSystemId, ClientSystemId = message.ClientSystemId, Folder = message.Folder });
        }

        public void handleRequestClientCloseConnection(NetPeer peer, Network.Messages.Connection.Request.CloseHostConnectionMessage message)
        {
            if (OnClientClose != null)
                OnClientClose(this, new ClientCloseEventArgs { HostSystemId = message.HostSystemId, ClientSystemId = message.ClientSystemId });
        }


        public void handleKeyPress(NetPeer peer, Network.Messages.Connection.OneWay.KeyMessage message)
		{
			if (OnKey != null)
				OnKey(this, new KeyEventArgs { Key = message.Key, Alt = message.Alt, Control = message.Control, Shift = message.Shift, Mode = message.Mode });
		}

		public void handleRequestInitalizeHostConnection(NetPeer peer, Network.Messages.Connection.Request.InitalizeHostConnectionMessage message)
		{
			if (OnClientInitalizeConnected != null)
				OnClientInitalizeConnected(this, new ClientInitalizeConnectedEventArgs { HostSystemId = message.HostSystemId, ClientSystemId = message.ClientSystemId, ClientPublicKey = message.ClientPublicKey });
		}

		public void handleMouseMove(NetPeer peer, Network.Messages.Connection.OneWay.MouseMoveMessage message)
		{
			if (OnMouseMove != null)
				OnMouseMove(this, new MouseMoveEventArgs { X = message.X, Y = message.Y });
		}

		public void handleMouseClick(NetPeer peer, Network.Messages.Connection.OneWay.MouseClickMessage message)
		{
			if (OnMouseClick != null)
				OnMouseClick(this, new MouseClickEventArgs { Down = message.Down, Up = message.Up, DoubleClick = message.DoubleClick, X = message.X, Y = message.Y, Button = (MouseClickEventArgs.ButtonType)message.Button });
		}

		public void handleRequestScreenshot(NetPeer peer, Network.Messages.Connection.RequestScreenshotMessage message)
		{
			if (OnScreenshotRequest != null)
				OnScreenshotRequest(this, new ScreenshotRequestEventArgs() { HostSystemId = message.HostSystemId, ClientSystemId = message.ClientSystemId, Fullscreen = message.Fullscreen });

		}

		public void handleResponseHostIntroducerRegistration(NetPeer peer, Network.Messages.System.ResponseHostIntroducerRegistrationMessage message)
		{
			this._hostManager.SystemId = message.Machine.SystemId;
			if (OnConnected != null)
				OnConnected(this, new ConnectedEventArgs() { SystemId = message.Machine.SystemId });
		}

		public void handleRequestHostConnection(NetPeer peer, Network.Messages.Connection.Request.HostConnectionMessage message)
		{
			var res = new Network.Messages.Connection.Response.HostConnectionMessage();
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
