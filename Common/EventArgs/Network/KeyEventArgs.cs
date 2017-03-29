using System;
using static Network.Messages.Connection.OneWay.KeyMessage;

namespace Common.EventArgs.Network
{
	public class KeyEventArgs : System.EventArgs
	{
        public uint Key { get; set; }
        public Boolean Shift { get; set; }
        public Boolean Control { get; set; }
        public Boolean Alt { get; set; }
        public KeyMode Mode { get; set; }
    }
}
