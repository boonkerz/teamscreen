using System;

using AppKit;
using Foundation;
using Messages.EventArgs.Network;
using Network.Thread;

namespace TeamScreenHost
{
	public partial class ViewController : NSViewController
	{
		public HostThread Manager { get { return Network.Instance.Host.Instance.Thread; } }

		protected Driver.Mac.Display display;

		protected Utils.Config.Manager ConfigManager;

		protected System.Timers.Timer connectionStatus;

		public ViewController(IntPtr handle) : base(handle)
		{
			connectionStatus = new System.Timers.Timer(1000);
			connectionStatus.Elapsed += Connection_Elapsed;

			display = new Driver.Mac.Display();
			display.SetHostManager(Manager.Manager);

			ConfigManager = new Utils.Config.Manager();

			Manager.Events.OnConnected += new EventHandler<ConnectedEventArgs>(Network_OnConnected);

			Manager.Events.OnClientInitalizeConnected += (object sender, Messages.EventArgs.Network.Host.ClientInitalizeConnectedEventArgs e) =>
			{
				SetLabelStatus("Initaliziere Verbindung");
			};

			Manager.Events.OnClientConnected += (object sender, ClientConnectedEventArgs e) =>
			{
				if (e.PasswordOk)
				{
					SetLabelStatus("Passwort Ok Verbunden mit: " + e.SystemId);
				}
				else
				{
					SetLabelStatus("Passwort Falsch Verbindung abgebrochen von: " + e.SystemId);
				}
			};

			Manager.Events.onPeerConnected += HostListener_onPeerConnected;
			Manager.Events.onPeerDisconnected += HostListener_onPeerDisconnected;
			Manager.Events.OnStartScreenSharing += Events_OnStartScreenSharing;
			Manager.Events.OnStopScreenSharing += Events_OnStopScreenSharing;;
			Manager.Start();
		}

		protected void SetLabelStatus(String Message)
		{
			if (this.lblStatus != null)
			{
				this.lblStatus.InvokeOnMainThread(() =>
				{
					this.lblStatus.StringValue = Message;
				});
			}
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// Do any additional setup after loading the view.
		}

		public override NSObject RepresentedObject
		{
			get
			{
				return base.RepresentedObject;
			}
			set
			{
				base.RepresentedObject = value;
				// Update the view, if already loaded.
			}
		}

		private void Connection_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			//Display.RemoveAllClients();
			Manager.Reconnect();
		}

		void Network_OnConnected(object sender, ConnectedEventArgs e)
		{
			SetSystemIdAndPassword(e.SystemId, Manager.Manager.Password);
		}

		private void HostListener_onPeerDisconnected(object sender, EventArgs e)
		{
			SetLabelStatus("Disconnected");
			connectionStatus.Start();
		}

		private void HostListener_onPeerConnected(object sender, EventArgs e)
		{
			SetLabelStatus("Connected");
			connectionStatus.Stop();
		}

		void Events_OnStartScreenSharing(object sender, StartScreenSharingEventArgs e)
		{
			display.StartScreenSharing(e.ClientSystemId);
		}

		void Events_OnStopScreenSharing(object sender, StopScreenSharingEventArgs e)
		{
			display.StopScreenSharing(e.ClientSystemId);
		}


		private void HostListener_OnScreenshotRequest(object sender, ScreenshotRequestEventArgs e)
		{

		}

		protected void SetSystemIdAndPassword(String SystemId, String Password)
		{
			if (this.txtSystemId != null)
			{
				this.txtSystemId.InvokeOnMainThread(() =>
				{
					this.txtSystemId.StringValue = SystemId;
				});
			}
			if (this.txtPassword != null)
			{
				this.txtPassword.InvokeOnMainThread(() =>
				{
					this.txtPassword.StringValue = Password;
				});
			}
		}
	}
}
