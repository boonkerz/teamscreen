using System;
using System.Collections.Generic;
using LiteNetLib.Utils;

namespace Messages.Connection
{
	public class RequestCheckOnlineMessage : BaseMessage
	{

		public List<Model.Peer> Peers { set; get; }

		public RequestCheckOnlineMessage()
			: base((ushort)CustomMessageType.RequestCheckOnline)
		{
			Peers = new List<Model.Peer>();
            EncryptedMessage = false;
		}

		public override void WritePayload(NetDataWriter message)
		{
			base.WritePayload(message);
			message.Put(Peers.Count);
			foreach(var peer in Peers)
			{
				peer.WritePayload(message);
			}
		}

		public override void ReadPayload(NetDataReader message)
		{
			base.ReadPayload(message);
			int count = message.GetInt();

			for (int i = 0; i < count; i++)
			{
				Model.Peer peer = new Model.Peer();
				peer.ReadPayload(message);
				this.Peers.Add(peer);
			}
		}

	}
}
