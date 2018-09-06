// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace TeamScreenHost
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSTextField lblStatus { get; set; }

		[Outlet]
		AppKit.NSTextField txtPassword { get; set; }

		[Outlet]
		AppKit.NSTextField txtSystemId { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (txtSystemId != null) {
				txtSystemId.Dispose ();
				txtSystemId = null;
			}

			if (txtPassword != null) {
				txtPassword.Dispose ();
				txtPassword = null;
			}

			if (lblStatus != null) {
				lblStatus.Dispose ();
				lblStatus = null;
			}
		}
	}
}
