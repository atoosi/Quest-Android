using System;
using Android.App;
using Android.Content;
using Android.Runtime;
namespace CData.DynamicsCRM.demo
{
  public class MyMessageDialog
  {
    public MyMessageDialog (Context context, string title, string message)
    {
      var builder = new AlertDialog.Builder(context);
      AlertDialog alertDialog = builder.Create();
      alertDialog.SetTitle(title);
      alertDialog.SetMessage(message);
      alertDialog.SetButton("OK", (s, ev) => {});
      alertDialog.Show();
    }
  }
}
