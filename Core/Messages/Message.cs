using System;
using LiteNetLib.Utils;

namespace Messages
{
	public abstract class Message
	{
		public ushort MessageType { get; private set; }

        public String SymmetricKey { get; set; }
        public int StartPointEncryption { get; set; }

        public String HostSystemId { get; set; }
        public String ClientSystemId { get; set; }

        public bool EncryptedMessage { get; set; }

        protected Message(ushort messageType)
		{
			MessageType = messageType;
            EncryptedMessage = true;
		}

		public virtual void WritePayload(NetDataWriter message)
		{

		}

		public virtual void ReadPayload(NetDataReader message)
		{

		}

        public virtual void WriteUncryptedPayload(NetDataWriter message)
        {
           
        }

        public virtual void ReadUncryptedPayload(NetDataReader message)
        {
            
        }

        public virtual void CopyEncryptedToTempStorage(NetDataReader message)
        {

        }

        public virtual byte[] CopyEncryptedFromTempStorage(NetDataWriter message)
        {
            return null;
        }
    }
}
