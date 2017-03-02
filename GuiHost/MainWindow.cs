using System;
using Common.EventArgs.Network;
using Common.Thread;
using Gdk;
using Gtk;

public partial class MainWindow : Gtk.Window
{

	public HostThread Manager { get { return Common.Instance.Host.Instance.Thread; } }

	struct NativeStruct
	{
		EventType type;
		IntPtr window;
		sbyte send_event;
		public uint time;
		public double x;
		public double y;
		public IntPtr axes;
		public uint state;
		public uint button;
		public IntPtr device;
		public double x_root;
		public double y_root;
	}

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
		Manager.HostListener.OnMouseMove += (object sender, MouseMoveEventArgs e) =>
		{
			//System.Windows.Forms.Cursor.Position = new System.Drawing.Point((int)e.X, (int)e.Y);
		};
		Manager.HostListener.OnMouseClick += (object sender, MouseClickEventArgs e) =>
		{
			Gdk.Display.Default.WarpPointer(Gdk.Display.Default.DefaultScreen, (int)e.X, (int)e.Y);
	
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
