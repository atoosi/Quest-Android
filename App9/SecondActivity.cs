using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using System.Data.CData.DynamicsCRM;
using System.Collections.Generic;
using System.Data;
using System.Threading;

namespace App9
{
    [Activity(Label = "QuestNet Mobile App")]
    public class SecondActivity : Activity
    {

        private ListView mListView;
        private List<ListViewData> items = new List<ListViewData>();
        private DynamicsCRMConnection connection = new DynamicsCRMConnection(@"User=axie;Password=8291;URL=https://qcrm.questinc.com; CRM Version=CRM 2011 IFD;");

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Second);


            new Thread(new ThreadStart(() =>
            {
            DynamicsCRMCommand command = new DynamicsCRMCommand("SELECT Id,new_name,new_fieldengineering_Id FROM new_supportrecord  where new_fieldengineering_Id =@fe_Id", connection);

            command.CommandType = CommandType.Text;

            command.Parameters.AddWithValue("@fe_Id", Intent.GetStringExtra("FeId"));

            DynamicsCRMDataReader rdr = command.ExecuteReader();
            rdr = command.ExecuteReader();
            
            while (rdr.Read())
            {
               
                items.Add(new ListViewData { Id= rdr["Id"].ToString(), Name= rdr["new_name"].ToString() });

            }

            mListView = FindViewById<ListView>(Resource.Id.listView1);

                // ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, items);
                RunOnUiThread(() =>
                {
                    SecondActivityListView adapter = new SecondActivityListView(this, Resource.Layout.Second_row, items);
                    mListView.Adapter = adapter;
                });

            mListView.ItemClick += mListView_ItemClick;
            })).Start();

           


        }
        void mListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //Get our item from the list adapter
            var item = this.mListView.GetItemAtPosition(e.Position);

            var intent = new Intent(this, typeof(ThirdActivity));
            intent.PutExtra("SrId", item.ToString());
            intent.PutExtra("FeId", Intent.GetStringExtra("FeId"));

            StartActivity(intent);

        }

    }
}
