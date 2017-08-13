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

            //if started by a user
            if (Environment.UserInteractive)
            {
#if DEBUG
                //if in debug mode, run the services as a non service application
                WindowsServiceHost.Interactive.Run(ServicesToRun.ToArray());
#else
                //otherwise, install the application

                switch(par)
                {
                    case "--install":
                        ManagedInstallerClass.InstallHelper(new string[] { Assembly.GetExecutingAssembly().Location });
                        break;
                    case "--uninstall":
                        ManagedInstallerClass.InstallHelper(new string[] { "/u", Assembly.GetExecutingAssembly().Location });
                        break;
                    default:
                        Console.Error.WriteLine("Insvalid Paramters!");
                        break;
                }
#endif

            }
            else
            {

                ServiceBase.Run(ServicesToRun.ToArray());
            }
        }
    }
}
