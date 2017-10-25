using System;

using AppKit;
using Common;
using Common.EventArgs.Network;
using Foundation;
using Network.Messages.Connection;

namespace MacGuiClient
{
	public partial class ViewController : NSViewController
	{
		public static ClientThread Manager { get { return Common.Instance.Client.Instance.Thread; } }

		protected Common.Config.Manager ConfigManager;

		protected System.Timers.Timer connectionStatus;

		private String password;

		public delegate void ShowFormCallback(String systemId);

		System.Timers.Timer onlineCheckTimer;

		public ViewController(IntPtr handle) : base(handle)
		{
			connectionStatus = new System.Timers.Timer(1000);
			connectionStatus.Elapsed += Connection_Elapsed;

			onlineCheckTimer = new System.Timers.Timer(5000);
			onlineCheckTimer.Elapsed += OnlineCheckTimer_Elapsed;

			ConfigManager = new Common.Config.Manager();

			Manager.ClientListener.OnHostInitalizeConnected += (object sender, Common.EventArgs.Network.Client.HostInitalizeConnectedEventArgs e) =>
			{
				Network.Messages.Connection.Request.HostConnectionMessage ms = new Network.Messages.Connection.Request.HostConnectionMessage();
				ms.HostSystemId = e.HostSystemId;
				ms.ClientSystemId = e.ClientSystemId;
				ms.Password = Manager.Manager.Encode(e.HostSystemId, this.password);
				ms.SymmetricKey = Manager.Manager.Encode(e.HostSystemId, Manager.Manager.getSymmetricKeyForRemoteId(e.HostSystemId));

				Manager.Manager.sendMessage(ms);


			};
			Manager.ClientListener.OnClientConnected += OnClientConnected;

			Manager.ClientListener.onNetworkError += ClientListener_onNetworkError;
			Manager.ClientListener.onPeerConnected += ClientListener_onPeerConnected;
			Manager.ClientListener.onPeerDisconnected += ClientListener_onPeerDisconnected;
			Manager.ClientListener.OnOnlineCheckReceived += ClientListener_OnOnlineCheckReceived;


		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			Manager.Start();
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

		private void ClientListener_OnOnlineCheckReceived(object sender, OnlineCheckReceivedEventArgs e)
		{
			/*this.listMyHosts.Items.Clear();
			foreach (var peer in e.Peers)
			{
				ListViewItem item = new ListViewItem(peer.Name);
				item.SubItems.Add(peer.SystemId);
				if (peer.isOnline)
				{
					item.SubItems.Add("Online");
					item.BackColor = Color.LightGreen;
				}
				else
				{
					item.SubItems.Add("Offline");
					item.BackColor = Color.White;
				}

				this.listMyHosts.Items.Add(item);
			}*/
		}

		private void OnlineCheckTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			Manager.Manager.sendMessage(new RequestCheckOnlineMessage { Peers = ConfigManager.ClientConfig.Peers });
		}

		private void Connection_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			Manager.Reconnect();
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

		private void ClientListener_onPeerDisconnected(object sender, EventArgs e)
		{

			SetLabelStatus("Introducer Disconnected");
			connectionStatus.Start();
		}


		private void ClientListener_onPeerConnected(object sender, EventArgs e)
		{
			SetLabelStatus("Introducer Connected");
			connectionStatus.Stop();
			//onlineCheckTimer.Start();
		}

		private void ClientListener_onNetworkError(object sender, EventArgs e)
		{
			SetLabelStatus("Network Error");
			connectionStatus.Start();
		}
		/*
		private async void OpenAForm(object sender, EventArgs e)
		{
			RemoteForm rm = new RemoteForm((string)sender, this.txtPassword.Text);

			rm.setManager(Manager);
			rm.Show();
		}

		private void openForm(String systemId)
		{
			RemoteForm rm = new RemoteForm(systemId, this.txtPasswordfield.StringValue);
			rm.setManager(Manager);
			rm.Show();
			rm.Start();
		}*/

		private void OnClientConnected(object sender, ClientConnectedEventArgs e)
		{
			if (e.PasswordOk)
			{

				SetLabelStatus("Passwort Ok Verbunden mit: " + e.SystemId);
				this.InvokeOnMainThread(() => {
					PerformSegue("test", this);
				});

			}
			else
			{
				SetLabelStatus("Passwort Falsch Verbindung abgebrochen von: " + e.SystemId);
			}
		}


		partial void LoginClick(Foundation.NSObject sender)
		{
			var pair = Manager.Manager.CreateNewKeyPairKey(this.txtUserfield.StringValue);
			this.password = this.txtPasswordfield.StringValue;
			Manager.Manager.sendMessage(
				new Network.Messages.Connection.Request.InitalizeHostConnectionMessage
				{
					ClientSystemId = Manager.Manager.SystemId,
					HostSystemId = this.txtUserfield.StringValue,
					ClientPublicKey = pair.PublicKey
				}
			);
		}

		public override void PrepareForSegue(NSStoryboardSegue segue, NSObject sender)
		{
			base.PrepareForSegue(segue, sender);

			// set the View Controller that’s powering the screen we’re
			// transitioning to

			var remoteViewController = segue.DestinationController as RemoteViewController;

			//set the Table View Controller’s list of phone numbers to the
			// list of dialed phone numbers

			if (remoteViewController != null)
			{
				remoteViewController.setManager(Manager);
				remoteViewController.SystemId = this.txtUserfield.StringValue;
			}
		}
	}
}
