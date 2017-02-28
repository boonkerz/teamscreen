using System;

namespace Common.EventArgs.Network
{
	public class ClientConnectedEventArgs : System.EventArgs
	{
		public String SystemId { get; set; }
		public Boolean PasswordOk { get; set; }
	}
}
