using System;
using System.Threading;
using System.Timers;
using LiteNetLib;
using LiteNetLib.Utils;
using Network;

namespace Common
{
	public class Host
	{
		private const int ServerPort = 50010;

		private NetManager _c1;

		private void OnTimedEvent(object source, ElapsedEventArgs e)
		{
			_c1.NatPunchModule.SendNatKeepAlive(new NetEndPoint("::1", ServerPort), "token1");
		}


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

			natPunchListener1.NatIntroductionSuccess += (point, token) =>
			{
				Console.WriteLine("Success C1. Connecting to C2: {0}", point);
				_c1.Connect(point);
			};

			natPunchListener1.NatIntroductionRequest += (localEndPoint, remoteEndPoint, token) =>
			{
				Console.WriteLine("In");
			};


			_c1 = new NetManager(netListener, "gamekey");
			_c1.NatPunchEnabled = true;
			_c1.UnsyncedEvents = true;
			_c1.NatPunchModule.Init(natPunchListener1);
			_c1.Start();


			_c1.NatPunchModule.SendNatIntroduceRequest(new NetEndPoint("::1", ServerPort), "token1");

			Console.WriteLine("Press ESC to quit");

			System.Timers.Timer aTimer = new System.Timers.Timer();
			aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
			aTimer.Interval = 5000;
			aTimer.Enabled = true;


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
						_c1.DisconnectPeer(_c1.GetFirstPeer(), new byte[] { 1, 2, 3, 4 });
						_c1.Stop();
					}
				}

				DateTime nowTime = DateTime.Now;


				_c1.NatPunchModule.PollEvents();

				_c1.PollEvents();

				NetDataWriter data = new NetDataWriter();

				data.Put("1234");

				if (_c1.GetFirstPeer() != null)
				{
					//KeepAliveMessage ka = new KeepAliveMessage();
					//_c1.GetFirstPeer().Send(ka, SendOptions.Unreliable);
				}


				Thread.Sleep(5);
			}

			_c1.Stop();
		}
	}
}
