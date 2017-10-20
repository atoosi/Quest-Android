using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;
using System.Data.CData.DynamicsCRM;
using Plugin.Geolocator;
using System.Threading.Tasks;

namespace App9
{

    [Service]
    public class FourthService : Service
    {
        static readonly string TAG = "X:" + typeof(FourthService).Name;
        static readonly int TimerWait = 60000;
        private  Timer _timer;
        const string connectionString = "User=axie;Password=8291;URL=https://qcrm.questinc.com;CRM Version=CRM 2011 IFD;STSURL=https://adfs.questinc.com:444/adfs/services/trust/13/usernamemixed;";


        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {

            //Log.Debug(TAG, "OnStartCommand called at {2}, flags={0}, startid={1}", flags, startId, DateTime.UtcNow);
            _timer = new Timer(o => {
               // Log.Debug(TAG, "Hello from SimpleService. {0}", DateTime.UtcNow);
                SaveLocation(intent);
            },null,0,TimerWait);

            return StartCommandResult.NotSticky;
        }

        public override IBinder OnBind(Intent intent)
        {
            // This example isn't of a bound service, so we just return NULL.
            return null;
        }
        public override void OnDestroy()
        {
            base.OnDestroy();

            _timer.Dispose();
            _timer = null;

            Log.Debug(TAG, "SimpleService destroyed at {0}.", DateTime.UtcNow);
        }


         void SaveLocation(Intent intent)
        {

            try
            {

                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;
                var position = locator.GetPositionAsync(20000);


                using (DynamicsCRMConnection connection = new DynamicsCRMConnection(connectionString))
                {


                    DynamicsCRMCommand cmd = new DynamicsCRMCommand("INSERT INTO new_felocation (new_latitude,new_longitude,new_fe_Id,new_fe_LogicalName,new_fe_Name) VALUES (@new_latatiude,@new_longitude,@new_fe, @logicalName,@Name)", connection);

                    cmd.Parameters.AddWithValue("@new_latatiude", position.Result.Latitude);
                    cmd.Parameters.AddWithValue("@new_longitude", position.Result.Longitude);
                    cmd.Parameters.AddWithValue("@new_fe", intent.GetStringExtra("FeId"));
                    cmd.Parameters.AddWithValue("@logicalName", "new_fieldengineering");
                    cmd.Parameters.AddWithValue("@Name", "test");


                    cmd.ExecuteNonQuery();

                    cmd = connection.CreateCommand();
                    cmd.CommandText = "SELECT SCOPE_IDENTITY()";
                    Object returnedValues = cmd.ExecuteScalar();
                    String Id = (String)returnedValues;


                }
            }
            catch (Exception ex)
            {

                Log.Debug(this.PackageName,ex.Message);

            }
        }
    }
}