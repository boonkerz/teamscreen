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
		float Ratio;
		Gdk.Pixbuf Buf = null;
		System.Drawing.Rectangle Bounds;

		String SystemId { get; set; }
		public ClientThread Manager { get { return Common.Instance.Client.Instance.Thread; } }

		public RemoteWindow(String systemId) :
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();

			this.eventBox.AddEvents((int)
			EventMask.AllEventsMask);

			this.SystemId = systemId;
			this.eventBox.MotionNotifyEvent += OnDrawingAreaMotionNotifyEvent;
			this.eventBox.KeyPressEvent += OnKeyPressEvent;
			this.eventBox.KeyReleaseEvent += OnKeyReleaseEvent;
			this.eventBox.ButtonPressEvent += OnButtonPressEvent;

			Image = new Gtk.Image();
			Image.Show();
			this.eventBox.Add(Image);
			this.eventBox.Realize();
			Manager.ClientListener.OnScreenshotReceived += OnScreenshotReceive;
			this.eventBox.GrabFocus();

			Manager.Manager.sendMessage(new RequestScreenshotMessage { HostSystemId = this.SystemId, ClientSystemId = Manager.Manager.SystemId });
		}

		protected void OnScreenshotReceive(object sender, ScreenshotReceivedEventArgs e)
		{
			if (e.SystemId == this.SystemId)
			{
				if (e.Nothing)
				{
					Thread.Sleep(200);
					Manager.Manager.sendMessage(new RequestScreenshotMessage { HostSystemId = this.SystemId, ClientSystemId = Manager.Manager.SystemId });
					return;
				}

				if (Buf == null)
				{
					Ratio = (float)this.eventBox.Allocation.Width / (float)e.Bounds.Width;
					Buf = new Gdk.Pixbuf(e.Image);
					this.eventBox.SetSizeRequest(this.eventBox.Allocation.Width, (int)Math.Round(e.Bounds.Height * Ratio));
					Bounds = e.Bounds;
				}
				else
				{
					Ratio = (float)this.eventBox.Allocation.Width / Bounds.Width;
					this.eventBox.SetSizeRequest(this.eventBox.Allocation.Width, (int)Math.Round(Bounds.Height * Ratio));
					var pixbuf = new Gdk.Pixbuf(e.Image);
					Console.WriteLine("x:" + e.Bounds.X + " y:" + e.Bounds.Y + " width:" + e.Bounds.Width + " height:" + e.Bounds.Height + " capwidth:" + pixbuf.Width);

					pixbuf.CopyArea(0, 0, pixbuf.Width, pixbuf.Height, Buf, e.Bounds.X, e.Bounds.Y);
				}

				this.Image.Pixbuf = Buf.ScaleSimple(this.eventBox.Allocation.Width, (int)Math.Round(Bounds.Height * Ratio), Gdk.InterpType.Bilinear);

				Thread.Sleep(200);
				Manager.Manager.sendMessage(new RequestScreenshotMessage { HostSystemId = this.SystemId, ClientSystemId = Manager.Manager.SystemId });
			}
		}

		protected void OnBtnCloseClicked(object sender, EventArgs e)
		{
			Manager.Manager.sendMessage(new RequestHostDisconnectMessage {  });
			Manager.ClientListener.OnScreenshotReceived -= OnScreenshotReceive;
			this.Destroy();
		}

		protected void OnDrawingAreaMotionNotifyEvent(object o, MotionNotifyEventArgs args)
		{
			Manager.Manager.sendMessage(new MouseMoveMessage { ClientSystemId = Manager.Manager.SystemId, HostSystemId = this.SystemId, X = (args.Event.X / Ratio), Y = (args.Event.Y / Ratio) });
		}

		protected void OnKeyPressEvent(object o, Gtk.KeyPressEventArgs args)
		{
			Manager.Manager.sendMessage(new Network.Messages.Connection.OneWay.KeyPressMessage { Key = args.Event.KeyValue, ClientSystemId = Manager.Manager.SystemId, HostSystemId = this.SystemId });
		}

		protected void OnKeyReleaseEvent(object o, Gtk.KeyReleaseEventArgs args)
		{
			Manager.Manager.sendMessage(new Network.Messages.Connection.OneWay.KeyReleaseMessage { Key = args.Event.KeyValue, ClientSystemId = Manager.Manager.SystemId, HostSystemId = this.SystemId });
		}

		protected void OnButtonPressEvent(object o, ButtonPressEventArgs args)
		{
			this.eventBox.GrabFocus();
			if (args.Event.Button == 1)
			{
				Manager.Manager.sendMessage(new MouseClickMessage { Button = MouseClickMessage.ButtonType.Left, ClientSystemId = Manager.Manager.SystemId, HostSystemId = this.SystemId, X = (args.Event.X / Ratio), Y = (args.Event.Y / Ratio) });
			}
			if (args.Event.Button == 2)
			{
				Manager.Manager.sendMessage(new MouseClickMessage { Button = MouseClickMessage.ButtonType.Middle, ClientSystemId = Manager.Manager.SystemId, HostSystemId = this.SystemId, X = (args.Event.X / Ratio), Y = (args.Event.Y / Ratio) });
			}
			if (args.Event.Button == 3)
			{
				Manager.Manager.sendMessage(new MouseClickMessage { Button = MouseClickMessage.ButtonType.Right, ClientSystemId = Manager.Manager.SystemId, HostSystemId = this.SystemId, X = (args.Event.X / Ratio), Y = (args.Event.Y / Ratio) });
			}
		}
	}
}
