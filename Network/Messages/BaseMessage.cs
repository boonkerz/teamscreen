using System;
using System.Collections.Generic;
using Network.Messages.System;
using LiteNetLib.Utils;

namespace Network
{
	public abstract class BaseMessage : Message
	{
        public String HostSystemId { get; set; }
        public String ClientSystemId { get; set; }

        protected BaseMessage(ushort type)
			: base(type)
		{
		}


        public override void WritePayload(NetDataWriter message)
        {
            base.WritePayload(message);
            message.Put(HostSystemId);
            message.Put(ClientSystemId);
        }

        public override void ReadPayload(NetDataReader message)
        {
            base.ReadPayload(message);
            HostSystemId = message.GetString(100);
            ClientSystemId = message.GetString(100);
        }
    }
}
