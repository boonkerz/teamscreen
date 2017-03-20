using System;
using LiteNetLib.Utils;

namespace Model
{
	public class Peer
	{
		public String SystemId { get; set; }
		public String Name { get; set; }
		public String Password { get; set; }
		public Boolean isOnline { get; set; }

		public Peer() {
			isOnline = false;
		}

		public void WritePayload(NetDataWriter writer)
		{
			writer.Put(this.SystemId);
			writer.Put(this.Name);
			writer.Put(this.isOnline);
		}

		public void ReadPayload(NetDataReader reader)
		{
			this.SystemId = reader.GetString(100);
			this.Name = reader.GetString(100);
			this.isOnline = reader.GetBool();
		}
	}


}
