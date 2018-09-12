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

    }
}
