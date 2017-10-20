using System;
using Foundation;
using UIKit;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data.CData.DynamicsCRM;

namespace iso_demo
{
	partial class ConnectionViewController : UIViewController
	{
    public static List<ConnectionItem> items = new List<ConnectionItem> ();
    ConnectionTableViewSource viewSource;
		public ConnectionViewController (IntPtr handle) : base (handle)
		{
      Title = "DynamicsCRM Provider - Demo";
      if (items.Count == 0) {
        items.Add (new ConnectionItem ("CRMVersion", ""));
        items.Add (new ConnectionItem ("Url", ""));
        items.Add (new ConnectionItem ("User", ""));
        items.Add (new ConnectionItem ("Password", ""));

      }
    }

    public override void ViewDidLoad()
    {
      base.ViewDidLoad ();
      ConnectionItem[] array = new ConnectionItem[items.Count];
      for (int i = 0; i < array.Length; i++) {
        array [i] = new ConnectionItem (items [i].name, items [i].value);
      }
      viewSource = new ConnectionTableViewSource (array);
      ConnectionProperties.Source = viewSource;
      save.Enabled = false;
    }

    partial void getTables_TouchUpInside(UIButton sender) {
      items.Clear();
      items.AddRange(viewSource.GetItems());
      Connection.SetupConnection(BuildConnectionString(items.ToArray()));
      string tableViewId = "tables";
      TablesViewController tableView = Storyboard.InstantiateViewController(tableViewId) as TablesViewController;
      NavigationController.PushViewController(tableView, true);
    }

    partial void test_TouchUpInside (UIButton sender)
    {
      try {
        Connection.TestConnection(BuildConnectionString(viewSource.GetItems()));
        new UIAlertView("Test Connection", "Successfully.", null, "OK", null).Show();
        save.Enabled = true;
      } catch (Exception e) {
        new UIAlertView("Test Connection Failed", e.Message, null, "OK", null).Show();
      }
    }

    public string BuildConnectionString(ConnectionItem[] item) {
      string connectionString = "";
      for (int i = 0; i < item.Length; i++) {
        string value = item[i].value;
        if (!string.IsNullOrEmpty(value))
          connectionString += item[i].name + "=" + value + ";";
      }
      DynamicsCRMConnectionStringBuilder builder = new DynamicsCRMConnectionStringBuilder();
      foreach (string key in builder.Keys) {
        if (key.Equals("SSL Cert")) {
          if (connectionString.IndexOf ("SSLCert") == -1) connectionString += "SSLCert=*;";
        }
      }
      return connectionString;
    }
	}

  class ConnectionTableViewSource : UITableViewSource {
    public ConnectionItem[] tableItems;
    string cellIdentifier = "connectionItem";
    public ConnectionTableViewSource (ConnectionItem[] items)
    {
      tableItems = items;
    }
    public override nint RowsInSection (UITableView tableview, nint section)
    {
      return tableItems.Length;
    }
    public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
    {

      TextFieldCell cell = tableView.DequeueReusableCell (cellIdentifier) as TextFieldCell;
      if (cell == null) {
        cell = new TextFieldCell (cellIdentifier);
      }
      if (tableItems[indexPath.Row].name.ToLower().Equals("password")) {
        cell.textField.SecureTextEntry = true;
      }
      cell.UpdateCell (tableItems [indexPath.Row].name, tableItems [indexPath.Row].value);
      cell.textField.EditingChanged += (object sender, EventArgs e) => {
        for (int i = 0; i < tableItems.Length; i++) {
          if (tableItems[i].name.Equals(cell.name)) {
            tableItems[i].value = cell.textField.Text;
          }
        }
      };
      return cell;
    }
    public ConnectionItem[] GetItems() {
      return (ConnectionItem[])tableItems.Clone ();
    }
  }

  class ConnectionItem {
    public string name { get; set;}
    public string value { get; set;}

    public ConnectionItem(string n) {
      name = n;
    }

    public ConnectionItem(string n, string v) {
      name = n;
      value = v;
    }
  }
}