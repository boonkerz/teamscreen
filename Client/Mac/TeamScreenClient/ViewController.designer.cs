// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;

namespace TeamScreenClient
{
	[Register("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSTextField lblStatus { get; set; }

		[Outlet]
		AppKit.NSScrollView tblNodes { get; set; }

		[Outlet]
		AppKit.NSSecureTextField txtPasswordfield { get; set; }

		[Outlet]
		AppKit.NSTextField txtUserfield { get; set; }

		[Action("LoginClick:")]
		partial void LoginClick(Foundation.NSObject sender);

		void ReleaseDesignerOutlets()
		{
			if (tblNodes != null)
			{
				tblNodes.Dispose();
				tblNodes = null;
			}

			if (txtPasswordfield != null)
			{
				txtPasswordfield.Dispose();
				txtPasswordfield = null;
			}

			if (txtUserfield != null)
			{
				txtUserfield.Dispose();
				txtUserfield = null;
			}

			if (lblStatus != null)
			{
				lblStatus.Dispose();
				lblStatus = null;
			}
		}
	}
}
