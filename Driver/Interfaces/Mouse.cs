using System;
namespace Driver.Interfaces
{
	public interface Mouse
	{

		void Move(int x, int y);
		void ClickLeft(int x, int y);
		void ClickRight(int x, int y);
		void ClickMiddle(int x, int y);
        void DoubleClickLeft(int x, int y);
        void DoubleClickRight(int x, int y);
        void DoubleClickMiddle(int x, int y);

        void ClickDownLeft(int x, int y);
        void ClickDownRight(int x, int y);
        void ClickDownMiddle(int x, int y);

        void ClickUpLeft(int x, int y);
        void ClickUpRight(int x, int y);
        void ClickUpMiddle(int x, int y);
    }
}
