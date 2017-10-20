using System;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android;
using Android.Content.PM;
using Android.Support.V4.Content;
using Android.Support.V4.App;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using System.Data.CData.DynamicsCRM;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Locations;
using System.Net;
using Newtonsoft.Json;


namespace App9
{


    [Activity(Label = "QuestNet Mobile App")]
    public class FourthActivity : AppCompatActivity
    {

        #region variables

        const int RequestLocationId = 0;
        const string permission = Manifest.Permission.AccessFineLocation;
        const string connectionString = "User=axie;Password=8291;URL=https://qcrm.questinc.com;CRM Version=CRM 2011 IFD;STSURL=https://adfs.questinc.com:444/adfs/services/trust/13/usernamemixed;";
        readonly string[] PermissionsLocation =
                {
               Manifest.Permission.AccessNetworkState,
               Manifest.Permission.Internet,
               Manifest.Permission.WriteExternalStorage,
               Manifest.Permission.AccessCoarseLocation,
               Manifest.Permission.AccessFineLocation,
             
            };

        private Button doneButton;
        private Button puseButton;
        private Button btnNormal;
        private Button btnHybrid;
        private Button btnTerrain;
        private Button btnSatellite;
        private TextView disTextView;
        private GoogleMap mMap;
        private Geocoder geo;
        private Intent ServiceIntent;
        private Bundle mBundle = new Bundle();
        private double latitude = new double();
        private double longitude = new double();
        private LatLng latLngSource;
        private LatLng latLngDestination;
        private bool cameraFlag; 
        View layout;

        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            

            SetContentView(Resource.Layout.Fourth);
            geo = new Geocoder(this);
            layout = FindViewById<LinearLayout>(Resource.Id.main_layout);
            doneButton = FindViewById<Button>(Resource.Id.btnDone);
            puseButton = FindViewById<Button>(Resource.Id.btnPuse);
            btnNormal = FindViewById<Button>(Resource.Id.btnNormal);
            btnHybrid = FindViewById<Button>(Resource.Id.btnHybrid);
            btnSatellite = FindViewById<Button>(Resource.Id.btnSatellite);
            btnTerrain = FindViewById<Button>(Resource.Id.btnTerrain);
            ServiceIntent = new Intent(this, typeof(FourthService));
            disTextView = FindViewById<TextView>(Resource.Id.distanceText);


            doneButton.Click += doneButton_Click;
            puseButton.Click += PuseButton_Click;
            btnHybrid.Click += BtnHybrid_Click;
            btnNormal.Click += BtnNormal_Click;
            btnSatellite.Click += BtnSatellite_Click;
            btnTerrain.Click += BtnTerrain_Click;
            cameraFlag = true;


            if ((int)Build.VERSION.SdkInt >= 23)
            {
                GetLocationPermission();
            }
            else
            {
                SetUpGoogleMap();
            }

              

        }


        #region buttons and service checker


        private void BtnSatellite_Click(object sender, EventArgs e)
        {
            mMap.MapType = GoogleMap.MapTypeSatellite;
        }

        private void BtnTerrain_Click(object sender, EventArgs e)
        {
            mMap.MapType = GoogleMap.MapTypeTerrain;
        }

        private void BtnHybrid_Click(object sender, EventArgs e)
        {
            mMap.MapType = GoogleMap.MapTypeHybrid;
        }

        private void BtnNormal_Click(object sender, EventArgs e)
        {
            mMap.MapType = GoogleMap.MapTypeNormal;
        }

        private void PuseButton_Click(object sender, EventArgs e)
        {
            if (this.puseButton.Text == "PUSE")
            {
                this.puseButton.Text = "RESUME";


                StopService(ServiceIntent);
            }
            else
            {
                this.puseButton.Text = "PUSE";

                ServiceIntent.PutExtra("FeId", Intent.GetStringExtra("FeId"));

                StartService(ServiceIntent);

            }
        }



        private void doneButton_Click(object sender, EventArgs e)
        {
            using (DynamicsCRMConnection connection = new DynamicsCRMConnection(connectionString))
            {
                DynamicsCRMCommand cmd = new DynamicsCRMCommand("UPDATE new_supportrecord SET new_checkout=@checkout WHERE Id=@id", connection);
                cmd.Parameters.Add(new DynamicsCRMParameter("@checkout", System.DateTime.Now));
                cmd.Parameters.Add(new DynamicsCRMParameter("@id", Intent.GetStringExtra("SrId")));
                cmd.ExecuteNonQuery();

            }

            foreach (var ser in GetRunningServices())
            {
                if (ser.Contains("FourthService"))
                {
                    StopService(ServiceIntent);

                }
            }

            Intent intent = new Intent(this, typeof(FifthActivity));
            intent.PutExtra("SrId", Intent.GetStringExtra("SrId"));
            StartActivity(intent);


        }



        private IEnumerable<string> GetRunningServices()
        {
            var manager = (ActivityManager)GetSystemService(ActivityService);
            return manager.GetRunningServices(int.MaxValue).Select(
                service => service.Service.ClassName).ToList();
        }



        #endregion

         async Task test()
        {
            if ((int)Build.VERSION.SdkInt >= 23 && (ContextCompat.CheckSelfPermission(this, permission) != (int)Permission.Granted))
            {
                 RequestPermissions(PermissionsLocation, RequestLocationId);
            }
        }

        void SetUpGoogleMap()
        {
            if (!CommonHelperClass.FnIsConnected(this))
            {
                Toast.MakeText(this, Constants.strNoInternet, ToastLength.Short).Show();
                return;
            }
            var frag = FragmentManager.FindFragmentById<MapFragment>(Resource.Id.mapFragment);
            var mapReadyCallback = new OnMapReadyClass();

            mapReadyCallback.MapReadyAction += delegate (GoogleMap obj)
            {
                mMap = obj;
                mMap.MyLocationEnabled = true;
                mMap.MyLocationChange += MMap_MyLocationChange;


            };


            frag.GetMapAsync(mapReadyCallback);
        }



        #region setup map direct reference



        //private void SetupMap()
        //{
        //    if (mMap == null)
        //    {
        //        FragmentManager.FindFragmentById<MapFragment>(Resource.Id.mapFragment).GetMapAsync(this);
        //    }
        //}

        //public void OnMapReady(GoogleMap googleMap)
        //{

        //    mMap = googleMap;
        //    mMap.MyLocationEnabled = true;

        //    GetLocationPermission();




        //}


        #endregion




        void GetLocationPermission()
        {
            
         
           if ((ContextCompat.CheckSelfPermission(this, permission) == (int)Permission.Granted))
            {
                SetUpGoogleMap();
             

                // return;
            }

            else if (ActivityCompat.ShouldShowRequestPermissionRationale(this, permission))
            {
                //Explain to the user why we need to read the contacts
                Snackbar.Make(layout, "Location access is required to show the map.",
                    Snackbar.LengthIndefinite)
                    .SetAction("OK", v => RequestPermissions(PermissionsLocation, RequestLocationId))
                    .Show();


                // return;
            }

            else
            {
                RequestPermissions(PermissionsLocation, RequestLocationId);
           

            }
        }





        private void MMap_MyLocationChange(object sender, GoogleMap.MyLocationChangeEventArgs e)
        {
            try
            {
                latitude = e.Location.Latitude;
                longitude = e.Location.Longitude;
                if (latitude != 0 && longitude != 0)

                    FnProcessOnMap();
            }
            catch
            {
                Toast.MakeText(this, "Your Location Is Not Clear", ToastLength.Short).Show();
            }
        }


    async void FnProcessOnMap()
        {
            await FnLocationToLatLng();

            if (latitude != 0 && longitude != 0 && latLngDestination != null)
            {
                latLngSource = new LatLng(latitude, longitude);
                if(cameraFlag!=false)
                FnUpdateCameraPosition(latLngSource);

                var address = await geo.GetFromLocationAsync(latitude, longitude, 1);
                string SourceAddress = "";
                for (int i = 0; i < address[0].MaxAddressLineIndex; i++)
                {
                    SourceAddress += address[0].GetAddressLine(i);
                    if (++i != address[0].MaxAddressLineIndex) SourceAddress += ",";
                }

                FnDrawPath(SourceAddress, Intent.GetStringExtra("Address"));
            }
        }




        async Task<bool> FnLocationToLatLng()
        {
            try
            {

                latLngSource = new LatLng(latitude, longitude);

                var destAddress = await geo.GetFromLocationNameAsync(Intent.GetStringExtra("Address"), 1);
                destAddress.ToList().ForEach((addr) =>
                {
                    latLngDestination = new LatLng(addr.Latitude, addr.Longitude);
                });



                return true;
            }
            catch
            {
                return false;
            }


        }


        async void FnDrawPath(string strSource, string strDestination)
        {
            string strFullDirectionURL = string.Format(Constants.strGoogleDirectionUrl, strSource, strDestination);
            string strJSONDirectionResponse = await FnHttpRequest(strFullDirectionURL);
            if (strJSONDirectionResponse != Constants.strException)
            {
                 RunOnUiThread(() =>
                    {
                        if (mMap != null)
                        {
                            mMap.Clear();
                            MarkOnMap(Constants.strTextSource, latLngSource, Resource.Drawable.MarkerSource);
                            MarkOnMap(Constants.strTextDestination, latLngDestination, Resource.Drawable.MarkerDest);
                        }
                    });
                
                    FnSetDirectionQuery(strJSONDirectionResponse);
            }
            else
            {
                RunOnUiThread(() =>
                   Toast.MakeText(this, Constants.strUnableToConnect, ToastLength.Short).Show());
            }

        }





        void FnSetDirectionQuery(string strJSONDirectionResponse)
        {
            var objRoutes = JsonConvert.DeserializeObject<GoogleDirectionClass>(strJSONDirectionResponse);
            //objRoutes.routes.Count  --may be more then one 
            try
            {
                if (objRoutes.routes.Count > 0)
            {
                string encodedPoints = objRoutes.routes[0].overview_polyline.points;

                int duration = 0, ind;
                string temp;
                foreach (var item in objRoutes.routes[0].legs)
                {
                    temp = item.duration.text;
                    ind = temp.IndexOf("min");
                    if (ind != -1)
                    {
                        duration += System.Convert.ToInt32(temp.Remove(ind));
                    }

                }
                disTextView.Text = duration.ToString() + " min   " + objRoutes.routes[0].legs[0].distance.text;
                var lstDecodedPoints = FnDecodePolylinePoints(encodedPoints);
                //convert list of location point to array of latlng type
                var latLngPoints = new LatLng[lstDecodedPoints.Count];
                int index = 0;
                foreach (Location loc in lstDecodedPoints)
                {
                    latLngPoints[index++] = new LatLng(loc.lat, loc.lng);
                }

                var polylineoption = new PolylineOptions();
                polylineoption.InvokeColor(Android.Graphics.Color.Red);
                polylineoption.Geodesic(true);
                polylineoption.Add(latLngPoints);
                RunOnUiThread(() =>
              mMap.AddPolyline(polylineoption));
            }
            }
            catch
            {
                RunOnUiThread(() =>
                 Toast.MakeText(this, "Unabel to Find Path for your location", ToastLength.Short).Show());
            }
        }




        List<Location> FnDecodePolylinePoints(string encodedPoints)
        {
            if (string.IsNullOrEmpty(encodedPoints))
                return null;
            var poly = new List<Location>();
            char[] polylinechars = encodedPoints.ToCharArray();
            int index = 0;

            int currentLat = 0;
            int currentLng = 0;
            int next5bits;
            int sum;
            int shifter;

            try
            {
                while (index < polylinechars.Length)
                {
                    // calculate next latitude
                    sum = 0;
                    shifter = 0;
                    do
                    {
                        next5bits = (int)polylinechars[index++] - 63;
                        sum |= (next5bits & 31) << shifter;
                        shifter += 5;
                    } while (next5bits >= 32 && index < polylinechars.Length);

                    if (index >= polylinechars.Length)
                        break;

                    currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                    //calculate next longitude
                    sum = 0;
                    shifter = 0;
                    do
                    {
                        next5bits = (int)polylinechars[index++] - 63;
                        sum |= (next5bits & 31) << shifter;
                        shifter += 5;
                    } while (next5bits >= 32 && index < polylinechars.Length);

                    if (index >= polylinechars.Length && next5bits >= 32)
                        break;

                    currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);
                    Location p = new Location();
                    p.lat = Convert.ToDouble(currentLat) / 100000.0;
                    p.lng = Convert.ToDouble(currentLng) / 100000.0;
                    poly.Add(p);
                }
            }
            catch
            {
                RunOnUiThread(() =>
                  Toast.MakeText(this, Constants.strPleaseWait, ToastLength.Short).Show());
            }
            return poly;
        }






        void MarkOnMap(string title, LatLng pos, int resourceId)
        {
            RunOnUiThread(() =>
            {
                try
                {
                  
                    var marker = new MarkerOptions();
                    marker.SetTitle(title);
                    marker.SetPosition(pos); //Resource.Drawable.BlueDot
                    marker.SetIcon(BitmapDescriptorFactory.FromResource(resourceId));
                    mMap.AddMarker(marker);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });
        }




        void FnUpdateCameraPosition(LatLng pos)
        {
            try
            {
                cameraFlag = false;
                CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
                builder.Target(pos);
                builder.Zoom(12);
                builder.Bearing(45);
                builder.Tilt(10);
                CameraPosition cameraPosition = builder.Build();
                CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
                mMap.AnimateCamera(cameraUpdate);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
        }





        WebClient webclient;
        async Task<string> FnHttpRequest(string strUri)
        {
            webclient = new WebClient();
            string strResultData;
            try
            {
                strResultData = await webclient.DownloadStringTaskAsync(new Uri(strUri));
                Console.WriteLine(strResultData);
            }
            catch
            {
                strResultData = Constants.strException;
            }
            finally
            {
                if (webclient != null)
                {
                    webclient.Dispose();
                    webclient = null;
                }
            }

            return strResultData;
        }








        string FnHttpRequestOnMainThread(string strUri)
        {
            webclient = new WebClient();
            string strResultData;
            try
            {
                strResultData = webclient.DownloadString(new Uri(strUri));
                Console.WriteLine(strResultData);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                strResultData = Constants.strException;
            }
            finally
            {
                if (webclient != null)
                {
                    webclient.Dispose();
                    webclient = null;
                }
            }

            return strResultData;
        }



    }
}
