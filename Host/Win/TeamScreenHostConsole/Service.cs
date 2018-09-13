using Messages.EventArgs.Network;
using Network.Thread;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamScreenHostConsole
{
    class Service
    {
        public HostThread hostThread { get { return Network.Instance.Host.Instance.Thread; } }

        protected Utils.Config.Manager ConfigManager;

        protected Driver.Win.Mouse mouse = new Driver.Win.Mouse();
        protected Driver.Win.Keyboard keyboard = new Driver.Win.Keyboard();
        protected Driver.Win.Display display = new Driver.Win.Display();

        
        private StreamWriter f = new StreamWriter(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\service.txt", true);

        protected System.Timers.Timer connectionStatus;

        public void OnStart()
        {
            f.AutoFlush = true;
            f.WriteLine("Start");
            

            f.WriteLine("Screen");


            connectionStatus = new System.Timers.Timer(1000);
          //  connectionStatus.Elapsed += Connection_Elapsed;

            ConfigManager = new Utils.Config.Manager();

            f.WriteLine("Server" + ConfigManager.HostConfig.ServerName);
            f.WriteLine("Configpath" + ConfigManager.ConfigPath);

            hostThread.Events.OnConnected += new EventHandler<ConnectedEventArgs>(Network_OnConnected);

            hostThread.Events.OnClientInitalizeConnected += (object sender, Messages.EventArgs.Network.Host.ClientInitalizeConnectedEventArgs e) =>
            {

                var pair = hostThread.Manager.CreateNewKeyPairKey(e.ClientSystemId);

                Messages.Connection.Response.InitalizeHostConnectionMessage rs = new Messages.Connection.Response.InitalizeHostConnectionMessage();
                rs.HostSystemId = hostThread.Manager.SystemId;
                rs.ClientSystemId = e.ClientSystemId;
                rs.HostPublicKey = pair.PublicKey;

                hostThread.Manager.sendMessage(rs);
            };

            hostThread.Events.OnClientConnected += (object sender, ClientConnectedEventArgs e) =>
            {
                if (e.PasswordOk)
                {
                    f.WriteLine("Passwort Ok Verbunden mit: " + e.SystemId);
                }
                else
                {
                    f.WriteLine("Passwort Falsch Verbindung abgebrochen von: " + e.SystemId);
                }
            };
            hostThread.Events.OnMouseMove += (object sender, MouseMoveEventArgs e) =>
            {
                mouse.Move((int)e.X, (int)e.Y);
            };
            hostThread.Events.OnKey += (object sender, Messages.EventArgs.Network.KeyEventArgs e) =>
            {
                if (e.Mode == Messages.Connection.OneWay.KeyMessage.KeyMode.Down)
                {
                    keyboard.Down(e.Key);
                }
                else
                {
                    keyboard.Up(e.Key);
                }

            };
            hostThread.Events.OnMouseClick += (object sender, MouseClickEventArgs e) =>
            {
                if (e.DoubleClick)
                {
                    switch (e.Button)
                    {
                        case MouseClickEventArgs.ButtonType.Left:
                            mouse.DoubleClickLeft((int)e.X, (int)e.Y);
                            break;
                        case MouseClickEventArgs.ButtonType.Middle:
                            mouse.DoubleClickMiddle((int)e.X, (int)e.Y);
                            break;
                        case MouseClickEventArgs.ButtonType.Right:
                            mouse.DoubleClickRight((int)e.X, (int)e.Y);
                            break;
                    }

                    return;
                }
                if (e.Down)
                {
                    switch (e.Button)
                    {
                        case MouseClickEventArgs.ButtonType.Left:
                            mouse.ClickDownLeft((int)e.X, (int)e.Y);
                            break;
                        case MouseClickEventArgs.ButtonType.Middle:
                            mouse.ClickDownMiddle((int)e.X, (int)e.Y);
                            break;
                        case MouseClickEventArgs.ButtonType.Right:
                            mouse.ClickDownRight((int)e.X, (int)e.Y);
                            break;
                    }

                    return;
                }
                if (e.Up)
                {
                    switch (e.Button)
                    {
                        case MouseClickEventArgs.ButtonType.Left:
                            mouse.ClickUpLeft((int)e.X, (int)e.Y);
                            break;
                        case MouseClickEventArgs.ButtonType.Middle:
                            mouse.ClickUpMiddle((int)e.X, (int)e.Y);
                            break;
                        case MouseClickEventArgs.ButtonType.Right:
                            mouse.ClickUpRight((int)e.X, (int)e.Y);
                            break;
                    }

                    return;
                }


                switch (e.Button)
                {
                    case MouseClickEventArgs.ButtonType.Left:
                        mouse.ClickLeft((int)e.X, (int)e.Y);
                        break;
                    case MouseClickEventArgs.ButtonType.Middle:
                        mouse.ClickMiddle((int)e.X, (int)e.Y);
                        break;
                    case MouseClickEventArgs.ButtonType.Right:
                        mouse.ClickRight((int)e.X, (int)e.Y);
                        break;
                }

            };
 
            display.SetHostManager(hostThread.Manager);
            hostThread.Events.onPeerConnected += HostListener_onPeerConnected;
            hostThread.Events.onPeerDisconnected += HostListener_onPeerDisconnected;
            hostThread.Events.onNetworkError += HostListener_onNetworkError;

            hostThread.Events.OnStartScreenSharing += Events_OnStartScreenSharing;
            hostThread.Events.OnStopScreenSharing += Events_OnStopScreenSharing;

            hostThread.Start();
        }

        private void Events_OnStopScreenSharing(object sender, StopScreenSharingEventArgs e)
        {
            display.StopScreenSharing(e.ClientSystemId);
        }

        private void Events_OnStartScreenSharing(object sender, StartScreenSharingEventArgs e)
        {
            display.StartScreenSharing(e.ClientSystemId);
        }

        public void OnStop()
        {
            connectionStatus.Stop();
            hostThread.Stop();
        }

        public bool IsRunning()
        {
            return hostThread.IsRunning();
        }

       
        private void HostListener_onNetworkError(object sender, EventArgs e)
        {
            f.WriteLine("Network Error");
            connectionStatus.Start();
        }

        private void HostListener_onPeerDisconnected(object sender, EventArgs e)
        {
            f.WriteLine("Introducer Disconnected");
            connectionStatus.Start();
        }

        private void HostListener_onPeerConnected(object sender, EventArgs e)
        {
            f.WriteLine("Introducer Connected");
            connectionStatus.Stop();
        }

        void Network_OnConnected(object sender, ConnectedEventArgs e)
        {
            f.WriteLine("SystemId: " + e.SystemId + " Password:" + hostThread.Manager.Password);
        }
    }
}
