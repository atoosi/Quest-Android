using System;
using Foundation;
using UIKit;
using System.CodeDom.Compiler;
using System.Data;
namespace iso_demo
{
	partial class TablesViewController : UITableViewController
	{
    public string[] tables;
		public TablesViewController (IntPtr handle) : base (handle)
		{
      tables = Connection.ListTablesViews ();
      this.Title = "Tables and Views";
		}

    public override void ViewWillAppear (bool animated)
    {
      base.ViewWillAppear (animated);
      if (tables == null || tables.Length == 0) return;
      TableView.Source = new TableTableSource(tables, this);
    }

    public void OpenSubView(string tablename) {
      string viewId = "custom query";
      CustomQueryViewController detail = Storyboard.InstantiateViewController (viewId) as CustomQueryViewController;
      NavigationController.PushViewController (detail, true);
      detail.SetStatement ("SELECT * FROM " + tablename + " LIMIT 20");
    }
	}

  class TableTableSource : UITableViewSource {
    TablesViewController parent;
    string[] tableItems;
    string cellIdentifier = "tableItem";
    public TableTableSource (string[] tables, TablesViewController tvc)
    {
      tableItems = tables;
      parent = tvc;
    }

    public override nint RowsInSection (UITableView tableview, nint section)
    {
      return tableItems.Length;
    }
    public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
    {

      UITableViewCell cell = tableView.DequeueReusableCell (cellIdentifier);   
      cell.TextLabel.Text = tableItems[indexPath.Row];  
      return cell;
    }
    public string GetItem(int id) {
      return tableItems[id];
    }
    public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
    {
      parent.OpenSubView (tableItems[indexPath.Row]);
    }
  }
}