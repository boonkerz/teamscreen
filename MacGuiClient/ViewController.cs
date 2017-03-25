using System;

using AppKit;
using Common;
using Foundation;

namespace MacGuiClient
{
	public partial class ViewController : NSViewController
	{
		public ClientThread Manager { get { return Common.Instance.Client.Instance.Thread; } }

		protected Common.Config.Manager ConfigManager;

		public ViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();


		}

		public override NSObject RepresentedObject
		{
			get
			{
				return base.RepresentedObject;
			}
			set
			{
				base.RepresentedObject = value;
				// Update the view, if already loaded.
			}
		}
	}
}
