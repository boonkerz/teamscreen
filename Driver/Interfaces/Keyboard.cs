using System;
namespace Driver.Interfaces
{
	public interface Keyboard
	{
		void Press(uint Key);
		void Release(uint Key);
	}
}
