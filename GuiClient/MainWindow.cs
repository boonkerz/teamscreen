using System;
using Common;
using Common.EventArgs.Network;
using Gtk;
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
				Manager.Manager.sendMessage(new RequestScreenshotMessage { HostSystemId = e.SystemId, ClientSystemId = Manager.Manager.SystemId });
			}
			else
			{
				this.lblStatus.Text = "Passwort Falsch Verbindung abgebrochen von: " + e.SystemId;
			}
		};
		Manager.ClientListener.OnScreenshotReceived += (object sender, ScreenshotReceivedEventArgs e) => 
		{ 
		
			var pixbuf = new Gdk.Pixbuf(e.Image);
			pixbuf = pixbuf.ScaleSimple(this.drawingArea.Allocation.Width, this.drawingArea.Allocation.Height, Gdk.InterpType.Bilinear);
			this.drawingArea.Pixbuf = pixbuf;
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
