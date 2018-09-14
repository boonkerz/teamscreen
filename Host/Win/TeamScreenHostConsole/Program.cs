using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZetaIpc.Runtime.Server;

namespace TeamScreenHostConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var p = new Service();

            var s = new IpcServer();
            s.Start(12345); // Passing no port selects a free port automatically.
            s.ReceivedRequest += (sender, argss) =>
            {
                Console.WriteLine("Got: " + argss.Request);

                if (argss.Request == "serviceStart")
                {
                    p.OnStart();
                    argss.Response = "1";
                }
                if (argss.Request == "getServiceStatus")
                {
                    TeamScreenCommon.IPC.Status status = new TeamScreenCommon.IPC.Status();
                    status.ServiceRunning = p.IsRunning();
                    argss.Response = JsonConvert.SerializeObject(status);
                }
                if (argss.Request == "getHostConfig")
                {
                    argss.Response = JsonConvert.SerializeObject(p.hostThread.ConfigManager.HostConfig);
                }
                if (argss.Request == "restartService")
                {
                    p.hostThread.Reconnect();
                }
                if (argss.Request == "serviceStop")
                {
                    p.OnStop();
                    argss.Response = "1";
                }


                argss.Handled = true;
            };
            p.OnStart();
            while (true)
            {
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
