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
	[Register ("ConnectionViewController")]
	partial class ConnectionViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UITableView ConnectionProperties { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIButton save { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIButton test { get; set; }

		[Action ("getTables_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void getTables_TouchUpInside (UIButton sender);

		[Action ("test_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void test_TouchUpInside (UIKit.UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (ConnectionProperties != null) {
				ConnectionProperties.Dispose ();
				ConnectionProperties = null;
			}
			if (save != null) {
				save.Dispose ();
				save = null;
			}
			if (test != null) {
				test.Dispose ();
				test = null;
			}
		}
	}
}