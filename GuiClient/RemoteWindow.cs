using System;
using System.Threading;
using Common;
using Common.EventArgs.Network;
using Gdk;
using Gtk;
using Network.Messages.Connection;
using Network.Messages.System;

namespace GuiClient
{
	public partial class RemoteWindow : Gtk.Window
	{
		Gtk.Image Image;
		float ratio;

		String SystemId { get; set; }
		public ClientThread Manager { get { return Common.Instance.Client.Instance.Thread; } }

		public RemoteWindow(String systemId) :
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();

			this.eventBox.AddEvents((int)
			(EventMask.AllEventsMask));

			this.SystemId = systemId;
			this.eventBox.MotionNotifyEvent += OnDrawingAreaMotionNotifyEvent;
			this.eventBox.KeyPressEvent += OnKeyPressEvent;
			this.eventBox.ButtonPressEvent += OnButtonPressEvent;

			Image = new Gtk.Image();
			Image.Show();
			this.eventBox.Add(Image);

			Manager.ClientListener.OnScreenshotReceived += OnScreenshotReceive;

			Manager.Manager.sendMessage(new RequestScreenshotMessage { HostSystemId = this.SystemId, ClientSystemId = Manager.Manager.SystemId });
		}

		protected void OnScreenshotReceive(object sender, ScreenshotReceivedEventArgs e)
		{
			if (e.SystemId == this.SystemId)
			{
				var pixbuf = new Gdk.Pixbuf(e.Image);
				ratio = (float)this.eventBox.Allocation.Width / (float)e.ScreenWidth;
				this.eventBox.SetSizeRequest(this.eventBox.Allocation.Width, (int)Math.Round(e.ScreenHeight * ratio));
				pixbuf = pixbuf.ScaleSimple(this.eventBox.Allocation.Width, (int)Math.Round(e.ScreenHeight * ratio), Gdk.InterpType.Bilinear);
				this.Image.Pixbuf = pixbuf;
				Thread.Sleep(50);
				Manager.Manager.sendMessage(new RequestScreenshotMessage { HostSystemId = this.SystemId, ClientSystemId = Manager.Manager.SystemId });
			}
		}

		protected void OnBtnCloseClicked(object sender, EventArgs e)
		{
			Manager.Manager.sendMessage(new RequestHostDisconnectMessage {  });
			Manager.ClientListener.OnScreenshotReceived -= OnScreenshotReceive;
			this.Dispose();
		}

		protected void OnDrawingAreaMotionNotifyEvent(object o, MotionNotifyEventArgs args)
		{
			Manager.Manager.sendMessage(new MouseMoveMessage { ClientSystemId = Manager.Manager.SystemId, HostSystemId = this.SystemId, X = (args.Event.X / ratio), Y = (args.Event.Y / ratio) });
		}

		protected void OnKeyPressEvent(object o, KeyPressEventArgs args)
		{

		}

		protected void OnButtonPressEvent(object o, ButtonPressEventArgs args)
		{
			if (args.Event.Button == 1)
			{
				Manager.Manager.sendMessage(new MouseClickMessage { Button = MouseClickMessage.ButtonType.Left, ClientSystemId = Manager.Manager.SystemId, HostSystemId = this.SystemId, X = (args.Event.X / ratio), Y = (args.Event.Y / ratio) });
			}
			if (args.Event.Button == 2)
			{
				Manager.Manager.sendMessage(new MouseClickMessage { Button = MouseClickMessage.ButtonType.Middle, ClientSystemId = Manager.Manager.SystemId, HostSystemId = this.SystemId, X = (args.Event.X / ratio), Y = (args.Event.Y / ratio) });
			}
			if (args.Event.Button == 3)
			{
				Manager.Manager.sendMessage(new MouseClickMessage { Button = MouseClickMessage.ButtonType.Right, ClientSystemId = Manager.Manager.SystemId, HostSystemId = this.SystemId, X = (args.Event.X / ratio), Y = (args.Event.Y / ratio) });
			}
		}
	}
}
