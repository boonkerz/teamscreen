using System;
using LiteNetLib.Utils;
using System.Collections.Generic;

namespace Messages.FileTransfer.Response
{
	public class ListingMessage : BaseMessage
	{

        public Boolean Parent { get; set; }
        public String ParentPath { get; set; }
        public String ActFolder { get; set; }

        public List<Model.Listing> Entrys = new List<Model.Listing>();

		public ListingMessage()
			: base((ushort)CustomMessageType.ResponseListing)
		{
		}

		public override void WritePayload(NetDataWriter message)
		{
			base.WritePayload(message);
            message.Put(Parent);
            message.Put(ParentPath);
            message.Put(ActFolder);
            message.Put(Entrys.Count);
            foreach (var entry in Entrys)
            {
                entry.WritePayload(message);
            }
        }

		public override void ReadPayload(NetDataReader message)
		{
			base.ReadPayload(message);
            Parent = message.GetBool();
            ParentPath = message.GetString(300);
            ActFolder = message.GetString(300);
            int count = message.GetInt();

            for (int i = 0; i < count; i++)
            {
                Model.Listing entry = new Model.Listing();
                entry.ReadPayload(message);
                this.Entrys.Add(entry);
            }
        }

        public void addEntry(Model.Listing entry)
        {
            this.Entrys.Add(entry);
        }

	}
}
