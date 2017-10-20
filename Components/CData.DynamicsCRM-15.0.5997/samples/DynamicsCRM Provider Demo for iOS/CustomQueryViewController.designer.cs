// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using System;
using Foundation;
using UIKit;
using System.CodeDom.Compiler;

namespace iso_demo
{
	[Register ("CustomQueryViewController")]
	partial class CustomQueryViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIButton Execute { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UITableView ResultTable { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UITextField Statement { get; set; }

		[Action ("Execute_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void Execute_TouchUpInside (UIKit.UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (Execute != null) {
				Execute.Dispose ();
				Execute = null;
			}
			if (ResultTable != null) {
				ResultTable.Dispose ();
				ResultTable = null;
			}
			if (Statement != null) {
				Statement.Dispose ();
				Statement = null;
			}
		}
	}
}