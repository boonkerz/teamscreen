using System;
using System.Collections.Generic;

namespace Utils.Config
{
	public class Client
	{
		public String ServerName { get; set; }
		public int ServerPort { get; set; }
		public List<Model.Peer> Peers = new List<Model.Peer>();

		public Client()
		{
			this.ServerName = "127.0.0.1";
			this.ServerPort = 9050;
		}
	}
}
