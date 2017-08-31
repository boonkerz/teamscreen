using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceHost
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        static void Main(string[] args)
        {
            var ServicesToRun = new List<ServiceBase>();
            ServicesToRun.Add(new WindowsServiceHost.Service1());

            var par = string.Concat(args);

            ServiceBase.Run(ServicesToRun.ToArray());
        }
    }
}
