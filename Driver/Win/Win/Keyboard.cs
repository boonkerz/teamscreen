using Driver.Win.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Driver.Win
{
    public class Keyboard : Driver.BaseDriver.Interface.IKeyboard
    {
        public void Down(uint Key)
        {
            SwitchToInputDesktop();
            Robot.keyDown((int)Key);
        }

        public void Up(uint Key)
        {
            SwitchToInputDesktop();
            Robot.keyUp((int)Key);
        }

        public void SwitchToInputDesktop()
        {
            var s = PInvoke.OpenInputDesktop(0, false, PInvoke.ACCESS_MASK.MAXIMUM_ALLOWED);
            bool success = PInvoke.SetThreadDesktop(s);
            PInvoke.CloseDesktop(s);
        }

        public string KeyCodeToUnicode(uint key)
        {
            byte[] keyboardState = new byte[255];
            bool keyboardStateStatus = GetKeyboardState(keyboardState);

            if (!keyboardStateStatus)
            {
                return "";
            }

            uint virtualKeyCode = (uint)key;
            uint scanCode = MapVirtualKey(virtualKeyCode, 0);
            IntPtr inputLocaleIdentifier = GetKeyboardLayout(0);

            StringBuilder result = new StringBuilder();
            ToUnicodeEx(virtualKeyCode, scanCode, keyboardState, result, (int)5, (uint)0, inputLocaleIdentifier);

            return result.ToString();
        }

        [DllImport("user32.dll")]
        static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        static extern uint MapVirtualKey(uint uCode, uint uMapType);

        [DllImport("user32.dll")]
        static extern IntPtr GetKeyboardLayout(uint idThread);

        [DllImport("user32.dll")]
        static extern int ToUnicodeEx(uint wVirtKey, uint wScanCode, byte[] lpKeyState, [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pwszBuff, int cchBuff, uint wFlags, IntPtr dwhkl);
    }
}
