using Driver.Win.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Driver.Win
{
    public class Mouse
    {
        protected ProcessStartInfo info;
        
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]

        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;
        private const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        private const int MOUSEEVENTF_MIDDLEUP = 0x0040;
        
        public Mouse()
        {
        }

        public void SwitchToInputDesktop()
        {
            var s = PInvoke.OpenInputDesktop(0, false, PInvoke.ACCESS_MASK.MAXIMUM_ALLOWED);
            bool success = PInvoke.SetThreadDesktop(s);
            PInvoke.CloseDesktop(s);
        }

        public void ClickLeft(int x, int y)
        {
            SwitchToInputDesktop();
            System.Windows.Forms.Cursor.Position = new System.Drawing.Point(x, y);
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)x, (uint)y, 0, 0);
        }
        
        public void ClickMiddle(int x, int y)
        {
            SwitchToInputDesktop();
            System.Windows.Forms.Cursor.Position = new System.Drawing.Point(x, y);
            mouse_event(MOUSEEVENTF_MIDDLEDOWN | MOUSEEVENTF_MIDDLEUP, (uint)x, (uint)y, 0, 0);
        }

        public void ClickRight(int x, int y)
        {
            SwitchToInputDesktop();
            System.Windows.Forms.Cursor.Position = new System.Drawing.Point(x, y);
            mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, (uint)x, (uint)y, 0, 0);
        }

        public void Move(int x, int y)
        {
            SwitchToInputDesktop();
            System.Windows.Forms.Cursor.Position = new System.Drawing.Point(x, y);
        }

        public void DoubleClickLeft(int x, int y)
        {
            SwitchToInputDesktop();
            System.Windows.Forms.Cursor.Position = new System.Drawing.Point(x, y);
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)x, (uint)y, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)x, (uint)y, 0, 0);
        }

        public void DoubleClickRight(int x, int y)
        {
            SwitchToInputDesktop();
            System.Windows.Forms.Cursor.Position = new System.Drawing.Point(x, y);
            mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, (uint)x, (uint)y, 0, 0);
            mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, (uint)x, (uint)y, 0, 0);
        }

        public void DoubleClickMiddle(int x, int y)
        {
            SwitchToInputDesktop();
            System.Windows.Forms.Cursor.Position = new System.Drawing.Point(x, y);
            mouse_event(MOUSEEVENTF_MIDDLEDOWN | MOUSEEVENTF_MIDDLEUP, (uint)x, (uint)y, 0, 0);
            mouse_event(MOUSEEVENTF_MIDDLEDOWN | MOUSEEVENTF_MIDDLEUP, (uint)x, (uint)y, 0, 0);
        }

        public void ClickDownLeft(int x, int y)
        {
            SwitchToInputDesktop();
            System.Windows.Forms.Cursor.Position = new System.Drawing.Point(x, y);
            mouse_event(MOUSEEVENTF_LEFTDOWN, (uint)x, (uint)y, 0, 0);
        }

        public void ClickDownRight(int x, int y)
        {
            SwitchToInputDesktop();
            System.Windows.Forms.Cursor.Position = new System.Drawing.Point(x, y);
            mouse_event(MOUSEEVENTF_RIGHTDOWN, (uint)x, (uint)y, 0, 0);
        }
        
        public void ClickDownMiddle(int x, int y)
        {
            SwitchToInputDesktop();
            System.Windows.Forms.Cursor.Position = new System.Drawing.Point(x, y);
            mouse_event(MOUSEEVENTF_MIDDLEDOWN, (uint)x, (uint)y, 0, 0);
        }

        public void ClickUpLeft(int x, int y)
        {
            SwitchToInputDesktop();
            mouse_event(MOUSEEVENTF_LEFTUP, (uint)x, (uint)y, 0, 0);
        }

        public void ClickUpRight(int x, int y)
        {
            SwitchToInputDesktop();
            mouse_event(MOUSEEVENTF_RIGHTUP, (uint)x, (uint)y, 0, 0);
        }
        
        public void ClickUpMiddle(int x, int y)
        {
            SwitchToInputDesktop();
            mouse_event(MOUSEEVENTF_MIDDLEUP, (uint)x, (uint)y, 0, 0);
        }
    }
}
