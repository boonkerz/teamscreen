using System;
using System.Threading;
using Common;
using Common.EventArgs.Network;
using Gdk;
using Gtk;
using Network;
using Network.Messages.Connection;

public partial class MainWindow : Gtk.Window
{
	public ClientThread Manager { get { return Common.Instance.Client.Instance.Thread; } }

	Gtk.Image Image;
	float ratio;

	public MainWindow() : base(Gtk.WindowType.Toplevel)
	{
		Build();

		this.eventBox.AddEvents((int)
			(EventMask.AllEventsMask));


		this.eventBox.MotionNotifyEvent += OnDrawingAreaMotionNotifyEvent;
		this.eventBox.KeyPressEvent += OnKeyPressEvent;
		this.eventBox.ButtonPressEvent += OnButtonPressEvent;

		Image = new Gtk.Image();
		Image.Show();
		this.eventBox.Add(Image);

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
			ratio = (float)this.eventBox.Allocation.Width / (float)e.ScreenWidth;
			this.eventBox.SetSizeRequest(this.eventBox.Allocation.Width, (int)Math.Round(e.ScreenHeight * ratio));
			pixbuf = pixbuf.ScaleSimple(this.eventBox.Allocation.Width, (int)Math.Round(e.ScreenHeight*ratio), Gdk.InterpType.Bilinear);
			this.Image.Pixbuf = pixbuf;
			Thread.Sleep(50);
			Manager.Manager.sendMessage(new RequestScreenshotMessage { HostSystemId = this.txtHostSystemId.Text, ClientSystemId = Manager.Manager.SystemId });

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

	protected void OnDrawingAreaMotionNotifyEvent(object o, MotionNotifyEventArgs args)
	{
		Manager.Manager.sendMessage(new MouseMoveMessage { ClientSystemId = Manager.Manager.SystemId, HostSystemId = this.txtHostSystemId.Text, X = (args.Event.X/ratio), Y = (args.Event.Y/ratio) });
	}

	protected void OnKeyPressEvent(object o, KeyPressEventArgs args)
	{
		
	}

	protected void OnButtonPressEvent(object o, ButtonPressEventArgs args)
	{
		Manager.Manager.sendMessage(new MouseClickMessage { ClientSystemId = Manager.Manager.SystemId, HostSystemId = this.txtHostSystemId.Text, X = args.Event.X, Y = args.Event.Y });
	}
}
