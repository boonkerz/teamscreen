using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteNetLib.Utils;

namespace Network.Listener
{
    public class BaseListener : INetEventListener
    {

        public event EventHandler onPeerConnected;
        public event EventHandler onPeerDisconnected;
        public event EventHandler onNetworkError;
        public event EventHandler<NetDataReader> OnReceive;

        public virtual void OnPeerConnected(NetPeer peer)
        {
            if (onPeerConnected != null)
                onPeerConnected(peer, System.EventArgs.Empty);
        }

        public virtual void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            if (onPeerDisconnected != null)
                onPeerDisconnected(this, System.EventArgs.Empty);
        }

        public virtual void OnNetworkError(NetEndPoint endPoint, int socketErrorCode)
        {
            if (onNetworkError != null)
                onNetworkError(this, System.EventArgs.Empty);
        }

		public virtual void OnNetworkReceive(NetPeer peer, NetDataReader reader)
        {
            if (OnReceive != null)
                OnReceive(this, reader);
        }

        public virtual void OnNetworkReceiveUnconnected(NetEndPoint remoteEndPoint, NetDataReader reader, UnconnectedMessageType messageType)
        {
            
        }

        public virtual void OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {
            
        }
	}
}
