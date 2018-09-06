using System;
namespace Driver.BaseDriver
{
	public class ClientListItem : IEquatable<ClientListItem>
	{
		public ClientListItem(string systemId)
		{
			SystemId = systemId;
			LastSeen = DateTime.Now;
		}

		public string SystemId { get; set; }

		public DateTime LastSeen { get; set; }

		public override string ToString()
		{
			return "ID: " + SystemId + "   LastSeen: " + LastSeen.ToString();
		}
		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			ClientListItem objAsPart = obj as ClientListItem;
			if (objAsPart == null) return false;
			else return Equals(objAsPart);
		}
		public override int GetHashCode()
		{
			return SystemId.GetHashCode();
		}
		public bool Equals(ClientListItem other)
		{
			if (other == null) return false;
			return (this.SystemId.Equals(other.SystemId));
		}

	}
}
