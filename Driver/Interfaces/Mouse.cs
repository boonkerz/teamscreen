using System;
namespace Driver.Interfaces
{
	public interface Mouse
	{

		void move(int x, int y);
		void clickLeft(int x, int y);
		void clickRight(int x, int y);
		void clickMiddle(int x, int y);
	}
}
