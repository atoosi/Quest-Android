using System;
using Foundation;
using UIKit;
using System.CodeDom.Compiler;
using System.Data;
using System.Collections.Generic;

namespace iso_demo
{
	partial class CustomQueryViewController : UIViewController
	{
    DataTable result;
    bool withStatement = false;
    string statement;
		public CustomQueryViewController (IntPtr handle) : base (handle)
		{
		}

    partial void Execute_TouchUpInside (UIButton sender)
    {
      ExecuteStatement();
    }

    public override void ViewDidLoad()
    {
      base.ViewDidLoad ();
      if (withStatement) {
        Statement.Text = statement;
        ExecuteStatement ();
      }
    }

    public void SetStatement(string text) {
      this.statement = text;
      this.withStatement = true;
    }

    public void ExecuteStatement() {
      string statement = Statement.Text;
      if (string.IsNullOrEmpty(statement)) {
        new UIAlertView ("Query", "Error: the statement is null or empty.", null, "OK", null).Show ();
      } else {
        List<string> firstColumns = new List<string>();
        if (statement.ToLower().Contains("select")) {
          result = Connection.Query(statement);
          if (result != null) {
            foreach(DataRow row in result.Rows) {
              firstColumns.Add(result.Columns[0].ColumnName + ": " + row[0].ToString());
            }
            ResultTable.Source = new ResultTableSource(firstColumns.ToArray(), true, this);
          }
        } else {
          int count = Connection.Execute(statement);
          firstColumns.Add("Row effected: " + count);
          ResultTable.Source = new ResultTableSource(firstColumns.ToArray(), false, this);
        }
      }
      ResultTable.ReloadData();
    }

    public void OpenSubView(int index) {
      string viewId = "detailview";
      DetailViewController detail = Storyboard.InstantiateViewController (viewId) as DetailViewController;
      detail.setData(result.Rows [index], result.Columns);
      NavigationController.PushViewController (detail, true);
    }
	}

  class ResultTableSource : UITableViewSource
  {
    CustomQueryViewController parent;
    string[] tableItems;
    string cellIdentifier = "resultItem";
    bool canList = false;
    public ResultTableSource (string[] items, bool list, CustomQueryViewController mvc)
    {
      tableItems = items;
      parent = mvc;
      canList = list;
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
      if (canList) {
        parent.OpenSubView (indexPath.Row);
      }
    }
  }
}