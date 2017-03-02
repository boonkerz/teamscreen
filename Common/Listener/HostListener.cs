using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Common.EventArgs.Network;
using LiteNetLib;
using LiteNetLib.Utils;
using Network;
using Network.Messages.Connection;

namespace Common.Listener
{
	public class HostListener : INetEventListener
	{
		private readonly MessageHandler _messageHandler;
		public HostManager _hostManager;

		public event EventHandler<ConnectedEventArgs> OnConnected;
		public event EventHandler<ClientConnectedEventArgs> OnClientConnected;
		public event EventHandler<ScreenshotRequestEventArgs> OnScreenshotRequest;
		public event EventHandler<MouseMoveEventArgs> OnMouseMove;
		public event EventHandler<MouseClickEventArgs> OnMouseClick;

		public HostListener()
		{
			_messageHandler = new MessageHandler();
		}

		public void OnPeerConnected(NetPeer peer)
		{
		}

		public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
		{
		}

		public void OnNetworkError(NetEndPoint endPoint, int socketErrorCode)
		{
		}

		public void OnNetworkReceive(NetPeer peer, NetDataReader reader)
		{
			Message msg = _messageHandler.decodeMessage(reader);
			switch (msg.MessageType)
			{
				case (ushort)Network.Messages.System.CustomMessageType.ResponseHostIntroducerRegistration:
					handleResponseHostIntroducerRegistration(peer, (Network.Messages.System.ResponseHostIntroducerRegistrationMessage)msg);
					break;
				case (ushort)Network.Messages.Connection.CustomMessageType.RequestHostConnection:
					handleRequestHostConnection(peer, (Network.Messages.Connection.RequestHostConnectionMessage)msg);
					break;
				case (ushort)Network.Messages.Connection.CustomMessageType.RequestScreenshot:
					handleRequestScreenshot(peer, (Network.Messages.Connection.RequestScreenshotMessage)msg);
					break;
				case (ushort)Network.Messages.Connection.CustomMessageType.MouseMove:
					handleMouseMove(peer, (Network.Messages.Connection.MouseMoveMessage)msg);
					break;
				case (ushort)Network.Messages.Connection.CustomMessageType.MouseClick:
					handleMouseClick(peer, (Network.Messages.Connection.MouseClickMessage)msg);
					break;
			}

		}
		public void handleMouseMove(NetPeer peer, Network.Messages.Connection.MouseMoveMessage message)
		{
			if (OnMouseMove != null)
				OnMouseMove(this, new MouseMoveEventArgs { X = message.X, Y = message.Y });
		}

		public void handleMouseClick(NetPeer peer, Network.Messages.Connection.MouseClickMessage message)
		{
			if (OnMouseClick != null)
				OnMouseClick(this, new MouseClickEventArgs { X = message.X, Y = message.Y });
		}

		public void handleRequestScreenshot(NetPeer peer, Network.Messages.Connection.RequestScreenshotMessage message)
		{
			if (OnScreenshotRequest != null)
				OnScreenshotRequest(this, new ScreenshotRequestEventArgs());

			Gdk.Window window = Gdk.Global.DefaultRootWindow;
			if (window != null)
			{
				Gdk.Pixbuf pixBuf = new Gdk.Pixbuf(Gdk.Colorspace.Rgb, false, 8,
									   window.Screen.Width, window.Screen.Height);
				pixBuf.GetFromDrawable(window, Gdk.Colormap.System, 0, 0, 0, 0,
									   window.Screen.Width, window.Screen.Height);
				
				ResponseScreenshotMessage rs = new ResponseScreenshotMessage();
				rs.HostSystemId = message.HostSystemId;
				rs.ClientSystemId = message.ClientSystemId;
				rs.Image = pixBuf.SaveToBuffer("png");
				rs.ScreenWidth = window.Screen.Width;
				rs.ScreenHeight = window.Screen.Height;

				this._hostManager.sendMessage(rs);
			}

		}

		public void handleResponseHostIntroducerRegistration(NetPeer peer, Network.Messages.System.ResponseHostIntroducerRegistrationMessage message)
		{
			this._hostManager.SystemId = message.Machine.SystemId;
			if (OnConnected != null)
				OnConnected(this, new ConnectedEventArgs() { SystemId = message.Machine.SystemId });
		}

		public void handleRequestHostConnection(NetPeer peer, Network.Messages.Connection.RequestHostConnectionMessage message)
		{
			ResponseHostConnectionMessage res = new ResponseHostConnectionMessage();
			res.HostSystemId = message.HostSystemId;
			res.ClientSystemId = message.ClientSystemId;
			res.PasswordOk = false;
			if (this._hostManager.Password == message.Password)
			{
				res.PasswordOk = true;
			}
			peer.Send(_messageHandler.encodeMessage(res), SendOptions.Unreliable);
			if (OnClientConnected != null)
				OnClientConnected(this, new ClientConnectedEventArgs() { SystemId = res.ClientSystemId, PasswordOk = res.PasswordOk });
		}

		public void OnNetworkReceiveUnconnected(NetEndPoint remoteEndPoint, NetDataReader reader, UnconnectedMessageType messageType)
		{

		}

		public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
		{

		}
	}
}
