using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Data;
namespace CData.DynamicsCRM.demo
{
  [Activity (Label = "Query")]      
  public class Query : Activity
  {
    DataTable result;
    string[] items;
    bool canList = false;
    protected override void OnCreate (Bundle bundle)
    {
      base.OnCreate (bundle);
      SetContentView (Resource.Layout.Query);
      Title = "Query";

      ListView view = FindViewById<ListView> (Resource.Id.Result);
      view.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
        if (items != null & items.Length > 0 & canList) {
          var intent = new Intent (this, typeof(Details));
          List<string> nItems = new List<string>();
          List<string> nColumns = new List<string>();
          DataRow row = result.Rows[e.Position];
          for (int i = 0; i < result.Columns.Count; i++) {
            nItems.Add(row[i].ToString());
            nColumns.Add(result.Columns[i].ToString());
          }
          intent.PutStringArrayListExtra("items", nItems);
          intent.PutStringArrayListExtra("columns", nColumns);
          StartActivity (intent);
        }
      };

      EditText editText = FindViewById<EditText> (Resource.Id.QueryText);
      string puttedStatement = Intent.GetStringExtra ("statement");
      if (!string.IsNullOrEmpty (puttedStatement))
        editText.Text = puttedStatement;

      Button execute = FindViewById<Button> (Resource.Id.Execute);
      execute.Click += (object sender, EventArgs e) => {
        string statement = editText.Text;
        if (statement.ToLower().Contains("select")) {
          try {
            result = Connection.Query(statement);
          } catch (Exception ex) {
            new MyMessageDialog(this, "Query", "Error: " + ex.Message);
            return;
          }
          if (result != null) {
            canList = true;
            items = new string[result.Rows.Count];
            for (int i = 0; i < result.Rows.Count; i++) {
              items[i] = result.Columns[0] + ": " + result.Rows[i][0].ToString();
            }
            view.Adapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, items);
          }
        } else {
          int count;
          try {
            canList = false;
            count = Connection.Execute(statement);
            items = new string[1];
            items[0] = "Row effected: " + count;
            view.Adapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, items);
          } catch (Exception ex) {
            new MyMessageDialog(this, "Query", "Error: " + ex.Message);
            return;
          }
        }
      };
    }
  }
}
