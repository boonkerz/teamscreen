using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Network.Utils;
using Network.Messages.System;

namespace Network
{
	public class MessageHandler
	{
		public MessageHandler()
		{
			Discover();
		}


		public NetDataWriter encodeMessage(Message message)
		{
			NetDataWriter dw = new NetDataWriter();
			message.WritePayload(dw);
			return dw;
		}

		public Message decodeMessage(NetDataReader incoming)
		{
			
			ushort messageType = incoming.GetUShort();
			var message = create(messageType);
			message.ReadPayload(incoming);

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
	}
}
