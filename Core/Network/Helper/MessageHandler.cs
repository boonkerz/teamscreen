using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using LiteNetLib.Utils;
using Messages;
using Messages.System;
using Network.Manager;

namespace Network.Helper
{
	public class MessageHandler
	{
		protected ManagerModus Modus {get;set;}
        public BaseManager Manager { get; set; }

        public enum ManagerModus {
            Client = 1,
            Broker = 2 ,
            Host = 3
        }

		public MessageHandler(ManagerModus modus, BaseManager manager = null)
		{
			this.Modus = modus;
            this.Manager = manager;
			Discover(Assembly.GetAssembly(typeof(Message)));
		}

		public byte[] encodeMessage(Message message)
		{
			NetDataWriter dw = new NetDataWriter(true);
            message.WriteUncryptedPayload(dw);
            if(this.Modus == ManagerModus.Broker)
            {
                if (message.EncryptedMessage)
                {
                    return message.CopyEncryptedFromTempStorage(dw);
                }
                else
                {
                    message.WritePayload(dw);
                }
                
            }
            else
            {
                message.WritePayload(dw);
                if (message.EncryptedMessage)
                {
                    if(this.Modus == ManagerModus.Host)
                    {
                        return Encrypt(message.StartPointEncryption, Manager.getSymmetricKeyForRemoteId(message.ClientSystemId), dw);
                    }
                    else
                    {
                        return Encrypt(message.StartPointEncryption, Manager.getSymmetricKeyForRemoteId(message.HostSystemId), dw);
                    }
                    
                }
            }
			return dw.Data;
		}

		public Message decodeMessage(NetDataReader incoming)
		{
			ushort messageType = incoming.GetUShort();
            var message = create(messageType);
            message.ReadUncryptedPayload(incoming);
            if (this.Modus == ManagerModus.Broker)
            {
                if (message.EncryptedMessage)
                {
                    message.CopyEncryptedToTempStorage(incoming);
                }
                else
                {
                    message.ReadPayload(incoming);
                }
            }
            else
            {
                if (message.EncryptedMessage)
                {
                    if (this.Modus == ManagerModus.Host)
                    {
                        Decrypt(message.StartPointEncryption, Manager.getSymmetricKeyForRemoteId(message.ClientSystemId), incoming);
                    }
                    else
                    {
                        Decrypt(message.StartPointEncryption, Manager.getSymmetricKeyForRemoteId(message.HostSystemId), incoming);
                    }
                }
                message.ReadPayload(incoming);
            }
            
            return message;
		}

		private readonly ConcurrentDictionary<ushort, Func<Message>> messageCtors = new ConcurrentDictionary<ushort, Func<Message>>();

		private Message create(ushort messageType)
		{
			Func<Message> mCtor;
			if (!messageCtors.TryGetValue(messageType, out mCtor))
					return null;

			return mCtor();
		}

		public void Discover(Assembly assembly)
		{
			if (assembly == null)
				throw new ArgumentNullException("assembly");

			var mtype = typeof(Message);
			RegisterTypes(
				assembly.GetTypes().Where(
					t => !t.IsGenericType && !t.IsGenericTypeDefinition && mtype.IsAssignableFrom(t) && t.GetConstructor(Type.EmptyTypes) != null),
				true);
		}

		public void Discover()
		{
			Discover(Assembly.GetCallingAssembly());
		}

		private void RegisterTypes(IEnumerable<Type> messageTypes, bool ignoreDupes)
		{
			if (messageTypes == null)
				throw new ArgumentNullException("messageTypes");

			var mtype = typeof(Message);

			var types = new Dictionary<Type, Func<Message>>();
			foreach (var t in messageTypes)
			{
				if (!mtype.IsAssignableFrom(t))
					throw new ArgumentException(String.Format("{0} is not an implementation of Message", t.Name), "messageTypes");
				if (mtype.IsGenericType || mtype.IsGenericTypeDefinition)
					throw new ArgumentException(String.Format("{0} is a generic type which is unsupported", t.Name), "messageTypes");

				var plessCtor = t.GetConstructor(Type.EmptyTypes);
				if (plessCtor == null)
					throw new ArgumentException(String.Format("{0} has no parameter-less constructor", t.Name), "messageTypes");

				var dplessCtor = new DynamicMethod("plessCtor", mtype, Type.EmptyTypes);
				var il = dplessCtor.GetILGenerator();
				il.Emit(OpCodes.Newobj, plessCtor);
				il.Emit(OpCodes.Ret);

				types.Add(t, (Func<Message>)dplessCtor.CreateDelegate(typeof(Func<Message>)));
			}

			RegisterTypesWithCtors(types, ignoreDupes);
		}

		private void RegisterTypesWithCtors(IEnumerable<KeyValuePair<Type, Func<Message>>> messageTypes, bool ignoreDupes)
		{
			if (messageTypes == null)
			{
				throw new ArgumentNullException("messageTypes");
			}
			var mtype = typeof(Message);
			foreach (var kvp in messageTypes)
			{
				if (!mtype.IsAssignableFrom(kvp.Key))
					throw new ArgumentException(String.Format("{0} is not an implementation of Message", kvp.Key.Name), "messageTypes");
				if (kvp.Key.IsGenericType || kvp.Key.IsGenericTypeDefinition)
					throw new ArgumentException(String.Format("{0} is a generic type which is unsupported", kvp.Key.Name), "messageTypes");

				var m = kvp.Value();

				if (!this.messageCtors.TryAdd (m.MessageType, kvp.Value))
				{
					if (ignoreDupes)
						continue;

					throw new ArgumentException (String.Format ("A message of type {0} has already been registered.", m.MessageType), "messageTypes");
				}
			}
		}

        public void Decrypt(int StartPointEncryption, String SymmetricKey, NetDataReader message)
        {
            byte[] toDecrypt = new byte[message.Data.Length - StartPointEncryption];

            Array.Copy(message.Data, StartPointEncryption, toDecrypt, 0, message.Data.Length - StartPointEncryption);

            byte[] decrypted = Network.Helper.Cryptography.DecryptBytes(toDecrypt, SymmetricKey);

            byte[] toTransfer = new byte[StartPointEncryption + decrypted.Length];
            Array.Copy(message.Data, 0, toTransfer, 0, StartPointEncryption);
            Array.Copy(decrypted, 0, toTransfer, StartPointEncryption, decrypted.Length);
            message.SetSource(toTransfer, StartPointEncryption);
        }

        public byte[] Encrypt(int StartPointEncryption, String SymmetricKey, NetDataWriter message)
        {
            byte[] toEncrypt = new byte[message.Data.Length - StartPointEncryption];
            Array.Copy(message.Data, StartPointEncryption, toEncrypt, 0, message.Data.Length - StartPointEncryption);

            byte[] encrypted = Network.Helper.Cryptography.EncryptBytes(toEncrypt, SymmetricKey);

            byte[] toTransfer = new byte[StartPointEncryption + encrypted.Length];

            Array.Copy(message.Data, 0, toTransfer, 0, StartPointEncryption);
            Array.Copy(encrypted, 0, toTransfer, StartPointEncryption, encrypted.Length);
            return toTransfer;
        }
    }
}
