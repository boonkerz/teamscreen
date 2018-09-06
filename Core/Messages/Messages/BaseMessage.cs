using System;
using System.Collections.Generic;
using Messages.System;
using LiteNetLib.Utils;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Messages
{
	public abstract class BaseMessage : Message
	{
        
        protected byte[] EncryptedTempStorage { get; set; }

        protected BaseMessage(ushort type)
			: base(type)
		{
            StartPointEncryption = 0;
		}

		public override void CopyEncryptedToTempStorage(NetDataReader message)
		{
			this.EncryptedTempStorage = new byte[message.Data.Length - StartPointEncryption];
			Array.Copy(message.Data, StartPointEncryption, this.EncryptedTempStorage, 0, message.Data.Length - StartPointEncryption);
		}

		public override byte[] CopyEncryptedFromTempStorage(NetDataWriter message)
		{
			byte[] toTransfer = new byte[StartPointEncryption + this.EncryptedTempStorage.Length];

			Array.Copy(message.Data, 0, toTransfer, 0, StartPointEncryption);
			Array.Copy(this.EncryptedTempStorage, 0, toTransfer, StartPointEncryption, this.EncryptedTempStorage.Length);

            return toTransfer;
            //message.PutBytesWithLength(this.EncryptedTempStorage, 0, this.EncryptedTempStorage.Length);
            //message.Data = toTransfer;
            //message.Length = toTransfer.Length;
        }

        public override void WritePayload(NetDataWriter message)
        {
            base.WritePayload(message);   
        }

        public override void ReadPayload(NetDataReader message)
        {
			base.ReadPayload(message);
        }

        public override void WriteUncryptedPayload(NetDataWriter message)
        {
            message.Put(MessageType);
            message.Put(HostSystemId);
            message.Put(ClientSystemId);
            StartPointEncryption = message.Length + 4;
            message.Put(StartPointEncryption);
        }

        public override void ReadUncryptedPayload(NetDataReader message)
        {
            HostSystemId = message.GetString(100);
            ClientSystemId = message.GetString(100);
            StartPointEncryption = message.GetInt();
        }

        public void PrintByteArray(byte[] bytes)
		{
			var sb = new StringBuilder("new byte[] { ");
			foreach (var b in bytes)
			{
				sb.Append(b + ", ");
			}
			sb.Append("}");
			Console.WriteLine(sb.ToString());
		}
    }
}
