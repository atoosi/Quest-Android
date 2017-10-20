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
  [Activity (Label = "Details")]      
  public class Details : ListActivity
  {
    string[] items;
    protected override void OnCreate (Bundle bundle)
    {
      base.OnCreate (bundle);
      IList<string> nItems = Intent.GetStringArrayListExtra ("items");
      IList<string> columns = Intent.GetStringArrayListExtra ("columns");
      try {
        items = new string[columns.Count];
        for (int i = 0; i < columns.Count; i++) {
          items[i] = columns[i] + ": " + nItems[i];
        }
        ListAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, items);
      } catch (Exception e) {
        new MyMessageDialog (this, "Details", "Show details failed, error message: " + e.Message);
      }
    }
  }
}
