using System;
using System.Collections.Generic;
using System.Threading;
using LiteNetLib;
using Network;

namespace Common
{
	public class IntroducerServer : INatPunchListener
	{
		private const int ServerPort = 50010;
		private static readonly TimeSpan KickTime = new TimeSpan(0, 1, 0);

		private readonly Dictionary<string, Peer> _hostPeers = new Dictionary<string, Peer>();
		private readonly List<string> _peersToRemove = new List<string>();
		private NetManager _puncher;

		void INatPunchListener.OnNatIntroductionRequest(NetEndPoint localEndPoint, NetEndPoint remoteEndPoint, string token)
		{
			Peer wpeer;
			if (_hostPeers.TryGetValue(token, out wpeer))
			{
				if (wpeer.InternalAddr.Equals(localEndPoint) &&
					wpeer.ExternalAddr.Equals(remoteEndPoint))
				{
					wpeer.Refresh();
					return;
				}

				Console.WriteLine("Wait peer found, sending introduction...");

				//found in list - introduce client and host to eachother
				Console.WriteLine(
					"host - i({0}) e({1})\nclient - i({2}) e({3})",
					wpeer.InternalAddr,
					wpeer.ExternalAddr,
					localEndPoint,
					remoteEndPoint);

				_puncher.NatPunchModule.NatIntroduce(
					wpeer.InternalAddr, // host internal
					wpeer.ExternalAddr, // host external
					localEndPoint, // client internal
					remoteEndPoint, // client external
					token // request token
					);

			}
			else
			{
				Console.WriteLine("Wait peer created. i({0}) e({1})", localEndPoint, remoteEndPoint);
				_hostPeers[token] = new Peer(localEndPoint, remoteEndPoint);
			}
		}

		void INatPunchListener.OnNatIntroductionSuccess(NetEndPoint targetEndPoint, string token)
		{
			//Ignore we are server
		}

		void INatPunchListener.OnNatKeepAlive(string token)
		{
			Console.WriteLine("Keepalive." + token);
			Peer wpeer;
			if (_hostPeers.TryGetValue(token, out wpeer))
			{
				wpeer.Refresh();
			}
		}

		public void Run()
		{
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

			_puncher = new NetManager(netListener, "notneed");
			_puncher.Start(ServerPort);
			_puncher.UnsyncedEvents = true;
			_puncher.NatPunchEnabled = true;
			_puncher.NatPunchModule.Init(this);

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
				}

				DateTime nowTime = DateTime.Now;


				_puncher.NatPunchModule.PollEvents();

				//check old peers
				foreach (var waitPeer in _hostPeers)
				{
					if (nowTime - waitPeer.Value.RefreshTime > KickTime)
					{
						_peersToRemove.Add(waitPeer.Key);
					}
				}

				//remove
				for (int i = 0; i < _peersToRemove.Count; i++)
				{
					Console.WriteLine("Kicking peer: " + _peersToRemove[i]);
					_hostPeers.Remove(_peersToRemove[i]);
				}
				_peersToRemove.Clear();

				Thread.Sleep(10);
			}

			_puncher.Stop();
		}
	}
}
