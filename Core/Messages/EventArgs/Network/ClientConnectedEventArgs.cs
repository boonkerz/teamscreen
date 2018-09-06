using System;

namespace Messages.EventArgs.Network
{
	public class ClientConnectedEventArgs 
	{
		public String SystemId { get; set; }
		public Boolean PasswordOk { get; set; }
	}
}
