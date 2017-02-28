using System;
using Common;
using Common.EventArgs.Network;
using Common.Instance;
using Common.Thread;
using Gtk;

public partial class MainWindow : Gtk.Window
{

	public HostThread Manager { get { return Common.Instance.Host.Instance.Thread; } }

	public MainWindow() : base(Gtk.WindowType.Toplevel)
	{
		Build();
		Manager.HostListener.OnConnected += new EventHandler<ConnectedEventArgs>(Network_OnConnected);
		Manager.HostListener.OnClientConnected += (object sender, ClientConnectedEventArgs e) =>
		{
			if (e.PasswordOk)
			{
				this.lblStatus.Text = "Passwort Ok Verbunden mit: " + e.SystemId;
			}
			else
			{
				this.lblStatus.Text = "Passwort Falsch Verbindung abgebrochen von: " + e.SystemId;
			}
		};
		Manager.start();
	}

	protected void OnDeleteEvent(object sender, DeleteEventArgs a)
	{
		Application.Quit();
		a.RetVal = true;
	}

	void Network_OnConnected(object sender, ConnectedEventArgs e)
	{
		this.SystemId.Text = e.SystemId;
		this.Password.Text = Manager.Manager.Password;
	}
}
