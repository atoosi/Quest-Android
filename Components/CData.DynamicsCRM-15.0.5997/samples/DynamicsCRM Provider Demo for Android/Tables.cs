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
  [Activity (Label = "Tables and Views")]
  public class Tables : ListActivity
  {
    string[] items;
    protected override void OnCreate (Bundle bundle)
    {
      base.OnCreate (bundle);
      try {
        items = Connection.ListTablesViews ();
        ListAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, items);
      } catch (Exception e) {
        new MyMessageDialog (this, "Tables", "List table failed, error message: " + e.Message);
      }
    }

    protected override void OnListItemClick(ListView l, View v, int position, long id) {
      string name = items [position];
      var intent = new Intent (this, typeof(Query));
      intent.PutExtra ("statement", "SELECT * FROM " + name + " LIMIT 10");
      StartActivity (intent);
    }
  }
}