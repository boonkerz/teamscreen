using System;
using LiteNetLib.Utils;

namespace Model
{
	public class Machine
	{
		/// <summary>
		/// Gets or sets the hardware ID of this machine.
		/// </summary>
		public string Identity { get; set; }

		/// <summary>
		/// Gets or sets the NovaId, which identifies machines to the end-user.
		/// </summary>
		/// <remarks>The NovaId is what the client types in to connect to a server. The NovaId identifies the server.</remarks>
		public string SystemId { get; set; }

		/// <summary>
		/// Gets or sets the server's password.
		/// </summary>
		/// <remarks>This field is never sent over the transport. A local Machine object is instead referenecd with this field.</remarks>
		public string Password { get; set; }

		/// <summary>
		/// Gets or sets the hash of the server's password.
		/// </summary>
		/// <remarks>The server generates a password and sends it to the Introducer as a hash. The client then types in the
		/// password as part of the authentication procedure and sends the hash of what was typed. The hashes are then compared.</remarks>
		public string PasswordHash { get; set; }

		public Machine()
		{
		}

		/// <summary>
		/// Writes the contents of this type to the transport.
		/// </summary>
		public void WritePayload(NetDataWriter message)
		{
			message.Put(Identity);
			message.Put(SystemId);
			message.Put(PasswordHash);
		}

		/// <summary>
		/// Reads the transport into the contents of this type.
		/// </summary>
		public void ReadPayload(NetDataReader message)
		{
			Identity = message.GetString(400);
			SystemId = message.GetString(400);
			PasswordHash = message.GetString(400);
		}
	}

}
