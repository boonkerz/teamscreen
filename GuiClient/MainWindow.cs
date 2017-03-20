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

	Gtk.ListStore peerStore;

	public MainWindow() : base(Gtk.WindowType.Toplevel)
	{
		Build();

		OnlineTimer = new Timer(CheckOnlineStatus, null, 5000, 10000);

		ConfigManager = new Common.Config.Manager();

		Manager.ClientListener.OnHostInitalizeConnected += (object sender, Common.EventArgs.Network.Client.HostInitalizeConnectedEventArgs e) =>
		{
			Manager.Manager.SaveHostPublicKey(e.HostSystemId, e.HostPublicKey);

			Network.Messages.Connection.Request.HostConnectionMessage ms = new Network.Messages.Connection.Request.HostConnectionMessage();
			ms.HostSystemId = e.HostSystemId;
			ms.ClientSystemId = e.ClientSystemId;
			ms.Password = Manager.Manager.Encode(e.HostSystemId, this.txtHostPassword.Text);

			Manager.Manager.sendMessage(ms);


		};
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
		var pair = Manager.Manager.CreateNewKeyPairKey(this.txtHostSystemId.Text);
		Manager.Manager.sendMessage(
			new Network.Messages.Connection.Request.InitalizeHostConnectionMessage 
					{ 	ClientSystemId = Manager.Manager.SystemId, 
						HostSystemId = this.txtHostSystemId.Text,
						ClientPublicKey = pair.PublicKey }
		);
	}

	private void CheckOnlineStatus(System.Object o)
	{
		Manager.Manager.sendMessage(new RequestCheckOnlineMessage { Peers = ConfigManager.ClientConfig.Peers });
	}

	private void onlineCheckReceived(object sender, OnlineCheckReceivedEventArgs e)
	{
		Model.Peer peerObj;
		bool found;
		foreach (var peer in e.Peers)
		{
			found = false;
			peerStore.Foreach((TreeModel model, TreePath path, TreeIter iter) =>
			{
				peerObj = (Model.Peer)model.GetValue(iter, 0);
				if (peerObj.SystemId == peer.SystemId)
				{
					peerObj.isOnline = peer.isOnline;
					found = true;
				}
				return found;
			});
			if (!found)
			{
				peerStore.AppendValues(peer);
			}
		}
		peerNodes.QueueDraw();
	}

	protected void buildTreeView()
	{
		Gtk.TreeViewColumn idColumn = new Gtk.TreeViewColumn();
		idColumn.Title = "SystemId";

		Gtk.CellRendererText idColumnCell = new Gtk.CellRendererText();
		idColumn.PackStart(idColumnCell, true);

		Gtk.TreeViewColumn nameColumn = new Gtk.TreeViewColumn();
		nameColumn.Title = "Name";

		Gtk.CellRendererText nameColumnCell = new Gtk.CellRendererText();
		nameColumn.PackStart(nameColumnCell, true);

		idColumn.SetCellDataFunc(idColumnCell, new Gtk.TreeCellDataFunc(RenderIdColumn));
		nameColumn.SetCellDataFunc(nameColumnCell, new Gtk.TreeCellDataFunc(RenderNameColumn));

		peerNodes.AppendColumn(idColumn);
		peerNodes.AppendColumn(nameColumn);


		peerStore = new Gtk.ListStore(typeof(Model.Peer));
		var configPeers = ConfigManager.ClientConfig.Peers;
		foreach (Model.Peer pe in configPeers)
		{
			peerStore.AppendValues(pe);
		}

		peerNodes.Model = peerStore;

		peerNodes.NodeSelection.Changed += (sender, e) =>
		{
			Gtk.TreeIter selected;
			peerNodes.Selection.GetSelected(out selected);
			Model.Peer peer = (Model.Peer)peerStore.GetValue(selected, 0);
			this.txtHostSystemId.Text = peer.SystemId;
			this.txtHostPassword.Text = peer.Password;
		};

	}

	private void RenderIdColumn(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
	{
		Model.Peer peer = (Model.Peer)model.GetValue(iter, 0);
		if (peer.isOnline)
		{
			(cell as Gtk.CellRendererText).Foreground = "darkgreen";
		}
		else
		{
			(cell as Gtk.CellRendererText).Foreground = "red";
		}
		(cell as Gtk.CellRendererText).Text = peer.SystemId;
	}

	private void RenderNameColumn(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
	{
		Model.Peer peer = (Model.Peer)model.GetValue(iter, 0);
		if (peer.isOnline)
		{
			(cell as Gtk.CellRendererText).Foreground = "darkgreen";
		}
		else
		{
			(cell as Gtk.CellRendererText).Foreground = "red";
		}
		(cell as Gtk.CellRendererText).Text = peer.Name;
	}

	protected void OnBtnAddClicked(object sender, EventArgs e)
	{
		ConfigManager.ClientConfig.Peers.Add(new Model.Peer { SystemId = this.txtId.Text, Name = this.txtName.Text, Password = this.txtPassword.Text });
		ConfigManager.saveClientConfig();

		var configPeers = ConfigManager.ClientConfig.Peers;
		peerStore.Clear();
		foreach (Model.Peer pe in configPeers)
		{
			peerStore.AppendValues(pe);
		}
	}
}
