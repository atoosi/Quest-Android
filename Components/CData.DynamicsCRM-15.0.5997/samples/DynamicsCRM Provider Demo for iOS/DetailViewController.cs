using System;
using Foundation;
using UIKit;
using System.CodeDom.Compiler;
using System.Data;
namespace iso_demo
{
	partial class DetailViewController : UITableViewController
	{
    DataRow row;
    DataColumnCollection columns;
		public DetailViewController (IntPtr handle) : base (handle)
		{
		}

    public void setData(DataRow input, DataColumnCollection cols) {
      row = input;
      columns = cols;
    }

    public override void ViewWillAppear (bool animated)
    {
      base.ViewWillAppear (animated);
      TableView.Source = new DetailTableSource(row, columns);
    }
	}

  class DetailTableSource : UITableViewSource {
    DataRow row;
    DataColumnCollection columns;
    string cellIdentifier = "detailItem";
    public DetailTableSource (DataRow input, DataColumnCollection cols)
    {
      row = input;
      columns = cols;
    }

    public override nint RowsInSection (UITableView tableview, nint section)
    {
      return columns.Count;
    }
    public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
    {

      UITableViewCell cell = tableView.DequeueReusableCell (cellIdentifier);
      cell.TextLabel.Text = columns [indexPath.Row] + ": " + row [indexPath.Row];
      return cell;
    }
    public string GetItem(int id) {
      return row[id].ToString();
    }
  }
}