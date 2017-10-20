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
using ZXing.Mobile;
using System.Data.CData.DynamicsCRM;
using Android.Support.V7.App;
using Android;
using Android.Support.V4.Content;
using Android.Content.PM;

namespace App9
{
    [Activity(Label = "QuestNet Mobile App")]
    public class FifthActivity : AppCompatActivity
    {
        private Button button1, button2, button3;
        private MobileBarcodeScanner scanner;
        private ZXing.Result result;
        private TextView barcode;
        const string connectionString = "User=axie;Password=8291;URL=https://qcrm.questinc.com;CRM Version=CRM 2011 IFD;STSURL=https://adfs.questinc.com:444/adfs/services/trust/13/usernamemixed;";
        readonly string[] PermissionsLocation =
              {

               Manifest.Permission.Camera
            };
        const string permission = Manifest.Permission.Camera;
        const int RequestLocationId = 0;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
          

            SetContentView(Resource.Layout.Fifth);
            MobileBarcodeScanner.Initialize(Application);
            button1 = FindViewById<Button>(Resource.Id.scanbutton1);
            button2 = FindViewById<Button>(Resource.Id.scanbutton2);
            button3 = FindViewById<Button>(Resource.Id.scanbutton3);
            barcode = FindViewById<TextView>(Resource.Id.barcode);

            scanner = new MobileBarcodeScanner();

            button1.Click += (sender, e) =>
            {
                using (DynamicsCRMConnection connection = new DynamicsCRMConnection(connectionString))
                {
                    DynamicsCRMCommand cmd = new DynamicsCRMCommand("UPDATE new_supportrecord SET new_serialnumber=@serialNumber WHERE Id=@id", connection);
                    cmd.Parameters.Add(new DynamicsCRMParameter("@serialNumber", result.Text));
                    cmd.Parameters.Add(new DynamicsCRMParameter("@id", Intent.GetStringExtra("SrId")));
                    cmd.ExecuteNonQuery();

                }
                Intent intent = new Intent(this, typeof(FirstActivity));
                StartActivity(intent);
            };

            button2.Click += async (sender, e) => {
                if ((ContextCompat.CheckSelfPermission(this, permission) == (int)Permission.Granted))
                {
                    result = await scanner.Scan();

                    if (result != null)
                    {
                        button1.Visibility = ViewStates.Visible;
                        button2.Visibility = ViewStates.Invisible;
                        button3.Visibility = ViewStates.Visible;

                        Console.WriteLine("Scanned Barcode: " + result.Text);
                        barcode.Text = "  " + result.Text;
                    }
                }
                else
                {
                    RequestPermissions(PermissionsLocation, RequestLocationId);
                }
            };

            button3.Click +=async (sender, e) =>
            {
                result = await scanner.Scan();

                if (result != null)
                {
                    Console.WriteLine("Scanned Barcode: " + result.Text);
                    barcode.Text = "  " + result.Text;
                }
            };
          

            
        }
    }
}