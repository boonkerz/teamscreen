using System;
using System.Collections.Generic;
using Network.Messages.System;
using LiteNetLib.Utils;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Network
{
	public abstract class BaseMessage : Message
	{
        public String HostSystemId { get; set; }
        public String ClientSystemId { get; set; }
        public int StartPointEncryption { get; set; }

		protected byte[] EncryptedTempStorage { get; set; }

        protected BaseMessage(ushort type)
			: base(type)
		{
		}

		public void CopyEncryptedToTempStorage(NetDataReader message)
		{
			this.EncryptedTempStorage = new byte[message.Data.Length - StartPointEncryption];
			Array.Copy(message.Data, StartPointEncryption, this.EncryptedTempStorage, 0, message.Data.Length - StartPointEncryption);
		}

		public void CopyEncryptedFromTempStorage(NetDataWriter message)
		{
			byte[] toTransfer = new byte[StartPointEncryption + this.EncryptedTempStorage.Length];

			Array.Copy(message.Data, 0, toTransfer, 0, StartPointEncryption);
			Array.Copy(this.EncryptedTempStorage, 0, toTransfer, StartPointEncryption, this.EncryptedTempStorage.Length);
            message.PutBytesWithLength(toTransfer, 0, toTransfer.Length);
            //message.Data = toTransfer;
            //message.Length = toTransfer.Length;
        }

        public override void WritePayload(NetDataWriter message)
        {
            base.WritePayload(message);
            message.Put(HostSystemId);
            message.Put(ClientSystemId);
			StartPointEncryption = message.Length + 4;
            message.Put(StartPointEncryption);
        }

        public override void ReadPayload(NetDataReader message)
        {
			base.ReadPayload(message);
            HostSystemId = message.GetString(100);
            ClientSystemId = message.GetString(100);
            StartPointEncryption = message.GetInt();

        }

		public void Decrypt(NetDataReader message)
		{
			byte[] toDecrypt = new byte[message.Data.Length - StartPointEncryption];

			Array.Copy(message.Data, StartPointEncryption, toDecrypt, 0, message.Data.Length - StartPointEncryption);

			byte[] decrypted = Network.Utils.Cryptography.DecryptBytes(toDecrypt, this.SymmetricKey);

            byte[] toTransfer = new byte[StartPointEncryption + decrypted.Length];
			Array.Copy(message.Data, 0, toTransfer, 0, StartPointEncryption);
			Array.Copy(decrypted, 0, toTransfer, StartPointEncryption, decrypted.Length);
			message.SetSource(toTransfer, StartPointEncryption);
		}

        public void Encrypt(NetDataWriter message)
        {
			byte[] toEncrypt = new byte[message.Data.Length-StartPointEncryption];
			Array.Copy(message.Data, StartPointEncryption , toEncrypt, 0, message.Data.Length - StartPointEncryption);

			byte[] encrypted = Network.Utils.Cryptography.EncryptBytes(toEncrypt, this.SymmetricKey);

			byte[] toTransfer = new byte[StartPointEncryption + encrypted.Length];

			Array.Copy(message.Data, 0, toTransfer, 0, StartPointEncryption);
			Array.Copy(encrypted, 0, toTransfer, StartPointEncryption, encrypted.Length);
            message.PutBytesWithLength(toTransfer, 0, toTransfer.Length);
			//message.Data = toTransfer;
			//message.Length = toTransfer.Length;
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
