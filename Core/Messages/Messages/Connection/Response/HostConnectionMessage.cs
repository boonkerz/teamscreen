using LiteNetLib.Utils;

namespace Messages.Connection.Response
{
	public class HostConnectionMessage : BaseMessage
	{

		public bool PasswordOk { get; set; }

		public HostConnectionMessage()
			: base((ushort)CustomMessageType.ResponseHostConnection)
		{
            EncryptedMessage = false;
		}

		public override void WritePayload(NetDataWriter message)
		{
			base.WritePayload(message);
			message.Put(PasswordOk);
		}

		public override void ReadPayload(NetDataReader message)
		{
			base.ReadPayload(message);
			PasswordOk = message.GetBool();
		}

	}
}
