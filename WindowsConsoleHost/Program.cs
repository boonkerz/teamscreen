using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var p = new ScreenCaptureService();
            p.OnStart();
        }
    }
}
