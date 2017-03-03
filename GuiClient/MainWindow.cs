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



	public MainWindow() : base(Gtk.WindowType.Toplevel)
	{
		Build();


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
		Manager.start();
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


}
