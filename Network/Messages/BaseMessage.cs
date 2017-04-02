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

        protected BaseMessage(ushort type)
			: base(type)
		{
		}


        public override void WritePayload(Network.Utils.NetDataWriter message)
        {
            base.WritePayload(message);
            message.Put(HostSystemId);
            message.Put(ClientSystemId);
            StartPointEncryption = message.Position();
            message.Put(StartPointEncryption + 4);
        }

        public override void ReadPayload(Network.Utils.NetDataReader message)
        {
            base.ReadPayload(message);
            HostSystemId = message.GetString(100);
            ClientSystemId = message.GetString(100);
            StartPointEncryption = message.GetInt();

        }

        public void Encrypt(Network.Utils.NetDataWriter message)
        {

            byte[] toEncrypt = new byte[message.Data.Length-this.StartPointEncryption];

            message.Data.CopyTo(toEncrypt, this.StartPointEncryption);

            using (Rijndael rijAlg = Rijndael.Create())
            {
                rijAlg.Key = Encoding.ASCII.GetBytes("Test");
                rijAlg.IV = Encoding.ASCII.GetBytes("TE");

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(toEncrypt);
                        }
                        msEncrypt.ToArray().CopyTo(message.Data, StartPointEncryption);
                    }
                }
            }

        }
    }
}
