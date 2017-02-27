using System;
using System.Threading;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Common
{
	public class Client
	{
		private const int ServerPort = 50010;

		private NetManager _c2;

		public void Run()
		{
			EventBasedNatPunchListener natPunchListener1 = new EventBasedNatPunchListener();

			EventBasedNetListener netListener = new EventBasedNetListener();

			netListener.PeerConnectedEvent += peer =>
			{
				Console.WriteLine("PeerConnected: " + peer.EndPoint.ToString());
			};

			netListener.PeerDisconnectedEvent += (peer, disconnectInfo) =>
			{
				Console.WriteLine("PeerDisconnected: " + disconnectInfo.Reason);
				if (disconnectInfo.AdditionalData.AvailableBytes > 0)
				{
					Console.WriteLine("Disconnect data: " + disconnectInfo.AdditionalData.GetInt());
				}
			};

			netListener.NetworkReceiveEvent += (peer, reader) =>
			{

				Console.WriteLine("Recive Data" + reader.GetString(100));
			};

			natPunchListener1.NatIntroductionSuccess += (point, token) =>
			{
				Console.WriteLine("Success C2. Connecting to C1: {0}", point);
				_c2.Connect(point);
			};


			_c2 = new NetManager(netListener, "gamekey");
			_c2.NatPunchEnabled = true;
			_c2.UnsyncedEvents = true;
			_c2.NatPunchModule.Init(natPunchListener1);
			_c2.Start();


			_c2.NatPunchModule.SendNatIntroduceRequest(new NetEndPoint("::1", ServerPort), "token1");

			// keep going until ESCAPE is pressed
			Console.WriteLine("Press ESC to quit");

			while (true)
			{
				if (Console.KeyAvailable)
				{
					var key = Console.ReadKey(true).Key;
					if (key == ConsoleKey.Escape)
					{
						break;
					}
					if (key == ConsoleKey.A)
					{
						Console.WriteLine("C1 stopped");
						_c2.DisconnectPeer(_c2.GetFirstPeer(), new byte[] { 1, 2, 3, 4 });
						_c2.Stop();
					}
				}

				DateTime nowTime = DateTime.Now;


				_c2.NatPunchModule.PollEvents();

				_c2.PollEvents();

				Thread.Sleep(5);
			}

			_c2.Stop();
		}
	}
}
