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
using System.Data.CData.DynamicsCRM;
using System.Data;
using System.Threading;

namespace App9
{

    [Activity(Theme = "@style/CustomActionBar")]
    public class ThirdActivity : Activity
    {
        private string connectionString = @"User=axie;Password=8291;URL=https://qcrm.questinc.com; CRM Version=CRM 2011 IFD;";

        private ListView mListView;
        private List<string> items;
        private Dictionary<string, string> transferData;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ActionBar.SetCustomView(Resource.Layout.action_bar);
            ActionBar.SetDisplayShowCustomEnabled(true);
            SetContentView(Resource.Layout.Third);
            mListView = FindViewById<ListView>(Resource.Id.listView);


            using (DynamicsCRMConnection connection = new DynamicsCRMConnection(connectionString))
            {


                DynamicsCRMCommand command = new DynamicsCRMCommand("SELECT * FROM new_supportrecord where Id = @Id", connection);
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@Id", Intent.GetStringExtra("SrId"));
                DynamicsCRMDataReader rdr = command.ExecuteReader();
                transferData = new Dictionary<string, string>();
                if (rdr.Read())
                {
                    transferData["name"] = rdr["new_name"].ToString();
                    transferData["checkin"] = rdr["new_checkin"].ToString();
                    transferData["checkout"] = rdr["new_checkout"].ToString();
                    transferData["address"] = rdr["new_address"].ToString();
                    transferData["visithours"] = rdr["new_visithours"].ToString();
                    transferData["case_Id"] = rdr["new_case_Id"].ToString();

                }

            }


            Button thirdActivityButton = FindViewById<Button>(Resource.Id.btnStart);
            thirdActivityButton.Click += ThirdActivityButton_Click;



            var detail = FindViewById<LinearLayout>(Resource.Id.linearLayout1);
            detail.FocusChange += Detail_FocusChange;

            var caseData = FindViewById<LinearLayout>(Resource.Id.linearLayout2);
            caseData.FocusChange += CaseData_FocusChange;

            var location = FindViewById<LinearLayout>(Resource.Id.linearLayout3);
            location.FocusChange += Location_FocusChange;


        }

        private void CaseData_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
         
            using (DynamicsCRMConnection connection = new DynamicsCRMConnection(connectionString))
            {


                DynamicsCRMCommand command = new DynamicsCRMCommand("SELECT * FROM incident where Id = @Id", connection);
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@Id", transferData["case_Id"]);
                DynamicsCRMDataReader rdr = command.ExecuteReader();
                items = new List<string>();
                if (rdr.Read())
                {
                        items.Add(" ");
                        items.Add("Case Title:  "+rdr["title"].ToString());
                        items.Add("Serial Number:  " + rdr["productserialnumber"].ToString());
                        items.Add("Case Type Code:  " + rdr["casetypecode"].ToString());

                    this.RunOnUiThread(() =>
                    {

                        ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, items);
                        mListView.Adapter = adapter;
                    });
                   
                }

            }
          
        }

        private void Location_FocusChange(object sender, View.FocusChangeEventArgs e)
        {

            items = new List<string>();


            //new Thread(new ThreadStart(() =>
            //{

            this.RunOnUiThread(() =>
            {


                items.Add(" ");
                items.Add("Location :       " + transferData["address"]);
                ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, items);
                mListView.Adapter = adapter;
            });
            //})).Start();

            //}
        }

        private void Detail_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            items = new List<string>();

            //new Thread(new ThreadStart(() =>
            //{
            this.RunOnUiThread(() =>
            {

                items.Add(" ");
                items.Add("Name :       " + transferData["name"]);
                items.Add("Checkin :    " + transferData["checkin"]);
                items.Add("Checkout :   " + transferData["checkout"]);
                items.Add("Visit hours :" + transferData["visithours"]);
                ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, items);
                mListView.Adapter = adapter;
            });
            //})).Start();


        }
        private void ThirdActivityButton_Click(object sender, EventArgs e)
        {

            using (DynamicsCRMConnection connection = new DynamicsCRMConnection(connectionString))
            {
                DynamicsCRMCommand cmd = new DynamicsCRMCommand("UPDATE new_supportrecord SET new_checkin=@checkin WHERE Id=@id", connection);
                cmd.Parameters.Add(new DynamicsCRMParameter("@checkin", System.DateTime.Now));
                cmd.Parameters.Add(new DynamicsCRMParameter("@id", Intent.GetStringExtra("SrId")));
                cmd.ExecuteNonQuery();
            }

            var intent = new Intent(this, typeof(FourthActivity));
            intent.PutExtra("SrId", Intent.GetStringExtra("SrId"));
            intent.PutExtra("FeId", Intent.GetStringExtra("FeId"));
            intent.PutExtra("Address", transferData["address"]);
            StartActivity(intent);

        }

    }
}
