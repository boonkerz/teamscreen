using System;
using Messages.EventArgs.Network;

namespace Driver.BaseDriver.Interface
{
	public interface IKeyboard
	{
        void Up(uint Key);
        void Down(uint Key);

    }
}
