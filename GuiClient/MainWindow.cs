using System;
using System.Threading;
using Common;
using Common.EventArgs.Network;
using Gdk;
using Gtk;
using GuiClient;
using Network;
using Network.Messages.Connection;

public partial class MainWindow : Gtk.Window
{
	public ClientThread Manager { get { return Common.Instance.Client.Instance.Thread; } }

	protected Common.Config.Manager ConfigManager;

	protected Timer OnlineTimer;

	Gtk.TreeIter iterOnline;
	Gtk.TreeIter iterOffline;
	Gtk.TreeStore peerStore;

	public MainWindow() : base(Gtk.WindowType.Toplevel)
	{
		Build();

		OnlineTimer = new Timer(CheckOnlineStatus, null, 5000, 10000);

		ConfigManager = new Common.Config.Manager();

		Manager.ClientListener.OnClientConnected += (object sender, ClientConnectedEventArgs e) =>
		{
			if (e.PasswordOk)
			{
				this.lblStatus.Text = "Passwort Ok Verbunden mit: " + e.SystemId;
				RemoteWindow rm = new RemoteWindow(e.SystemId);
				rm.Show();
			}
			else
			{
				this.lblStatus.Text = "Passwort Falsch Verbindung abgebrochen von: " + e.SystemId;
			}
		};
		Manager.ClientListener.OnOnlineCheckReceived += onlineCheckReceived;
		Manager.start();

		buildTreeView();
	}

	protected void OnDeleteEvent(object sender, DeleteEventArgs a)
	{
		Application.Quit();
		a.RetVal = true;
	}

	protected void OnBtnLoginClicked(object sender, EventArgs e)
	{
		Manager.Manager.sendMessage(new RequestHostConnectionMessage { ClientSystemId = Manager.Manager.SystemId, HostSystemId = this.txtHostSystemId.Text, Password = this.txtHostPassword.Text });
	}

	private void CheckOnlineStatus(System.Object o)
	{
		Manager.Manager.sendMessage(new RequestCheckOnlineMessage { Peers = ConfigManager.ClientConfig.Peers });
	}

	private void onlineCheckReceived(object sender, OnlineCheckReceivedEventArgs e)
	{
		
		buildStore();
		foreach (var peer in e.Peers)
		{
			if (peer.isOnline)
			{
				this.peerStore.AppendValues(iterOnline, peer.SystemId, peer.Name);
			}
			else
			{
				this.peerStore.AppendValues(iterOffline, peer.SystemId, peer.Name);
			}
		}

		treePeers.Model = peerStore;
	}

	protected void buildTreeView()
	{
		Gtk.TreeViewColumn idColumn = new Gtk.TreeViewColumn();
		idColumn.Title = "Id";

		Gtk.CellRendererText idColumnCell = new Gtk.CellRendererText();

		idColumn.PackStart(idColumnCell, true);

		Gtk.TreeViewColumn nameColumn = new Gtk.TreeViewColumn();
		nameColumn.Title = "Name";

		Gtk.CellRendererText nameColumnCell = new Gtk.CellRendererText();
		nameColumn.PackStart(nameColumnCell, true);

		treePeers.AppendColumn(idColumn);
		treePeers.AppendColumn(nameColumn);

		idColumn.AddAttribute(idColumnCell, "text", 0);
		nameColumn.AddAttribute(nameColumnCell, "text", 1);

		peerStore = new Gtk.TreeStore(typeof(string), typeof(string));

		buildStore();

		treePeers.Model = peerStore;
	}

	protected void buildStore()
	{
		this.peerStore.Clear();
		iterOnline = peerStore.AppendValues("Online");
		iterOffline = peerStore.AppendValues("Offline");
	}

	protected void OnBtnAddClicked(object sender, EventArgs e)
	{
		ConfigManager.ClientConfig.Peers.Add(new Model.Peer { SystemId = this.txtId.Text, Name = this.txtName.Text });
		ConfigManager.saveClientConfig();
	}
}
