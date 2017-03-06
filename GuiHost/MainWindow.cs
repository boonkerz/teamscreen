using System;
using System.Diagnostics;
using Common.EventArgs.Network;
using Common.Thread;
using Gdk;
using Gtk;
using Network.Messages.Connection;

public partial class MainWindow : Gtk.Window
{

	public HostThread Manager { get { return Common.Instance.Host.Instance.Thread; } }

	protected Common.Config.Manager ConfigManager;

	public Driver.Interfaces.Mouse Mouse { get { return Driver.Manager.Instance.Mouse; } }
	public Driver.Interfaces.Keyboard Keyboard { get { return Driver.Manager.Instance.Keyboard; } }
	public Driver.Interfaces.Display Display { get { return Driver.Manager.Instance.Display; } }

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
		ConfigManager = new Common.Config.Manager();

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
			Mouse.move((int)e.X, (int)e.Y);
		};
		Manager.HostListener.OnMouseClick += (object sender, MouseClickEventArgs e) =>
		{
			if (e.Button == MouseClickEventArgs.ButtonType.Left)
			{
				Mouse.clickLeft((int)e.X, (int)e.Y);
			}
			if (e.Button == MouseClickEventArgs.ButtonType.Middle)
			{
				Mouse.clickMiddle((int)e.X, (int)e.Y);
			}
			if (e.Button == MouseClickEventArgs.ButtonType.Right)
			{
				Mouse.clickRight((int)e.X, (int)e.Y);
			}

		};
		Manager.HostListener.OnScreenshotRequest += (object sender, ScreenshotRequestEventArgs e) =>
		{
			
			ResponseScreenshotMessage rs = new ResponseScreenshotMessage();
			rs.HostSystemId = e.HostSystemId;
			rs.ClientSystemId = e.ClientSystemId;
			rs.Image = Display.makeScreenshot();
			rs.ScreenWidth = Display.getScreenWidth();
			rs.ScreenHeight = Display.getScreenHeight();

			this.Manager.Manager.sendMessage(rs);
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

	protected void OnBtnSaveClicked(object sender, EventArgs e)
	{
		ConfigManager.HostConfig.Password = this.Password.Text;
		ConfigManager.saveHostConfig();
		Manager.Manager.Password = this.Password.Text;
	}
}
