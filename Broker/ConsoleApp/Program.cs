using System;
using LiteNetLib;
using Network.Thread;
using Terminal.Gui;

namespace TeamScreen.Broker.ConsoleApp
{
    class Program
    {
        public static BrokerThread Manager { get { return Network.Instance.Broker.Instance.Thread; } }
        
        static void Main(string[] args)
        {
			var ml = new Label(new Rect(3, 17, 47, 1), "");

			Manager.Events.onPeerConnected += delegate (object sender, EventArgs eventArgs)
			{
				NetPeer peer = (NetPeer)sender;
				ml.Text = "Peer connected: " + peer.EndPoint;
			};

			Manager.Start();


			Application.Init();
			var top = Application.Top;

			// Creates the top-level window to show
			var win = new Window(new Rect(0, 1, top.Frame.Width, top.Frame.Height - 1), "TeamScreen Broker");
			top.Add(win);

			var menu = new MenuBar(new MenuBarItem[] {
			new MenuBarItem ("_File", new MenuItem [] {
				new MenuItem ("_Quit", "", () => { if (Quit ()) top.Running = false; })
				})
			});

			win.Add(ml);

			Application.Run();

        }

		static bool Quit()
		{
			var n = MessageBox.Query(50, 7, "Quit TeamScreen Broker", "Are you sure you want to quit?", "Yes", "No");
			return n == 0;
		}
	}
}