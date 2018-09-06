using System;
using static Messages.Connection.OneWay.KeyMessage;

namespace Messages.EventArgs.Network
{
	public class KeyEventArgs 
	{
        public uint Key { get; set; }
        public Boolean Shift { get; set; }
        public Boolean Control { get; set; }
        public Boolean Alt { get; set; }
        public KeyMode Mode { get; set; }
    }
}
