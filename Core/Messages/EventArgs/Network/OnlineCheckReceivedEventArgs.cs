using System;
using System.Collections.Generic;

namespace Messages.EventArgs.Network
{
	public class OnlineCheckReceivedEventArgs 
	{
		public List<Model.Peer> Peers { get; set; }
	}
}
