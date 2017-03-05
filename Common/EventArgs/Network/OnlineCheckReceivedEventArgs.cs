using System;
using System.Collections.Generic;

namespace Common.EventArgs.Network
{
	public class OnlineCheckReceivedEventArgs : System.EventArgs
	{
		public List<Model.Peer> Peers { get; set; }
	}
}
