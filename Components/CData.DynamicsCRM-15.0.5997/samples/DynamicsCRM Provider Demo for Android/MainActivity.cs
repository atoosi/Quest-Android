using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System.Collections;
using System.Data.CData.DynamicsCRM;

namespace CData.DynamicsCRM.demo
{
  [Activity (Label = "DynamicsCRM Provider Demo", MainLauncher = true)]
  public class MainActivity : Activity
  {
    public Hashtable connectionSettings = new Hashtable();
    private string lastConnectionString;
    private static List<string> items;
    protected override void OnCreate (Bundle bundle)
    {
      base.OnCreate (bundle);
      SetContentView (Resource.Layout.Main);
      ListView view = FindViewById<ListView> (Resource.Id.ConnectionProperties);
      view.Adapter = new ConnectionItemAdapter (this, items);
      Button getTables = FindViewById<Button> (Resource.Id.GetTables);
      getTables.Enabled = false;
      getTables.Click += (object sender, EventArgs e) => {
        var intent = new Intent (this, typeof(Tables));
        StartActivity (intent);
      };

      Button testConnection = FindViewById<Button> (Resource.Id.TestButton);
      testConnection.Click += (object sender, EventArgs e) => {
        string connectionString = buildConnectionString();
        try {
          Connection.TestConnection(connectionString);
          Connection.SetupConnection(connectionString);
          lastConnectionString = connectionString;
          new MyMessageDialog(this, "Test Connection", "Connected successfully!");
          getTables.Enabled = true;
        } catch (Exception ex) {
          new MyMessageDialog(this, "Test Connection", "Failed, error message: " + ex.Message);       
        }
      };
    }
      
    static MainActivity () {
      items = new List<string> ();
      items.Add ("CRMVersion");
      items.Add ("Url");
      items.Add ("User");
      items.Add ("Password");

    }

    private string buildConnectionString() {
      string connectionString = "";
      IDictionaryEnumerator keys = connectionSettings.GetEnumerator ();
      while (keys.MoveNext()) {
        string value = keys.Value.ToString();
        if (!string.IsNullOrEmpty (value)) {
          connectionString += keys.Key + "=" + value + ";";
        }
      }
      DynamicsCRMConnectionStringBuilder builder = new DynamicsCRMConnectionStringBuilder();
      foreach (string key in builder.Keys) {
        if (key.Equals("SSL Cert")) {
          if (connectionString.IndexOf ("SSLCert") == -1) connectionString += "SSLCert=*;";
        }
      }
      return connectionString;
    }

    public void CheckConnectionStringChanged() {
      string connectionString = buildConnectionString ();
      if (!connectionString.Equals (lastConnectionString)) {
        Button getTables = FindViewById<Button> (Resource.Id.GetTables);
        getTables.Enabled = false;
      }
    }
  }

  class ConnectionItemAdapter : BaseAdapter {
    MainActivity context;
    List<string> items;
    public ConnectionItemAdapter(MainActivity context, List<string> items)
      : base()
    {
      this.context = context;
      this.items = items;
    }
    public override long GetItemId(int position)
    {
      return position;
    }
    public override Java.Lang.Object GetItem (int position) {
      return null;
    }
    public override int Count
    {
      get { return items.Count; }
    }
    public override View GetView(int position, View convertView, ViewGroup parent)
    {
      View view = convertView;
      if (view == null) view = context.LayoutInflater.Inflate(Resource.Layout.ConnectionItem, null);
      string item = items[position];
      TextView textView = view.FindViewById<TextView> (Resource.Id.Text1);
      textView.Text = item + ":";
      EditText editText = view.FindViewById<EditText> (Resource.Id.Input1);
      if (item.ToLower ().Equals ("password")) {
        editText.InputType = Android.Text.InputTypes.ClassText | Android.Text.InputTypes.TextVariationPassword;
      }
      editText.Tag = item;
      editText.Text = "";
      if (context.connectionSettings.ContainsKey (item)) {
        editText.Text = (string) context.connectionSettings[item].ToString();
      }
      editText.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {
        if (String.IsNullOrEmpty(e.Text.ToString())) return;
        context.connectionSettings[((EditText) sender).Tag] = e.Text.ToString();
        context.CheckConnectionStringChanged();
      };
      return view;
    }
  }
}