using System;
namespace Utils.Config
{
	public class Host
	{
		public String SystemId { get; set; }
		public String Password { get; set; }
		public String ServerName { get; set; }
		public int ServerPort { get; set; }

		public Host()
		{
			this.ServerName = "127.0.0.1";
			this.ServerPort = 9050;
		}
	}
}
