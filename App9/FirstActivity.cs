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
using System.Collections;
using System.Data;
using System.IO;
using System.Threading;

namespace App9
{

    [Activity(Label = "QuestNet Mobile App", MainLauncher = true)]
    public class FirstActivity : Activity
    {
        private EditText mTxtEmail;
        private EditText mTxtPassword;
        private Button mBtnSignUp;
        private ProgressBar progress;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.First);
            mTxtEmail = FindViewById<EditText>(Resource.Id.txtUserName);
            mTxtPassword = FindViewById<EditText>(Resource.Id.txtPass);
            mBtnSignUp = FindViewById<Button>(Resource.Id.btnLogin);


            string connectionString = "User=axie;Password=8291;URL=https://qcrm.questinc.com;"

                                       + "CRM Version=CRM 2011 IFD;STSURL=https://adfs.questinc.com:444/adfs/services/trust/13/usernamemixed;";

            // +"Logfile=C:/Users/atoosi/Desktop/CRMLog.txt;Verbosity=3;";

            mBtnSignUp.Click += (sender, e) =>
            {
                progress = FindViewById<ProgressBar>(Resource.Id.ProgressBar);
                progress.Visibility = ViewStates.Visible;
                //var progressDialog = ProgressDialog.Show(this, "Please wait...", "Checking account info...", true);
                //new Thread(new ThreadStart(delegate
                //{
                //    LOAD METHOD TO GET ACCOUNT INFO
                //    RunOnUiThread(() => Toast.MakeText(this, "Toast within progress dialog.", ToastLength.Long).Show());
                //    HIDE PROGRESS DIALOG
                //    RunOnUiThread(() => progressDialog.Hide());
                //})).Start();




                //new Thread(new ThreadStart(() =>
                //{
                //    for (int i = 0; i <= 100; i++)
                //    {
                //        this.RunOnUiThread(() =>
                //        {
                //            progress.Progress = i;
                //        });
                //        Thread.Sleep(200);
                //    }


                //})).Start();
                RunOnUiThread(() =>
                {
                    mTxtEmail.Enabled = false;
                    mTxtPassword.Enabled = false;
                });

                new Thread(new ThreadStart(() =>
                {

                    using (DynamicsCRMConnection connection = new DynamicsCRMConnection(connectionString))
                    {


                        DynamicsCRMCommand command = new DynamicsCRMCommand("SELECT Id,new_email,new_password FROM new_fieldengineering where new_email = @email and new_password=@password", connection);

                        command.CommandType = CommandType.Text;

                        command.Parameters.AddWithValue("@email", mTxtEmail.Text.ToString());
                        command.Parameters.AddWithValue("@password", mTxtPassword.Text.ToString());

                        DynamicsCRMDataReader rdr = command.ExecuteReader();
                        rdr = command.ExecuteReader();
                      
                        if (rdr.Read())
                        {
                     
                            var intent = new Intent(this, typeof(SecondActivity));
                            intent.PutExtra("FeId", rdr["Id"].ToString());
                            StartActivity(intent);

                        }
                        else RunOnUiThread(() =>
                        {
                            mTxtEmail.Enabled = true;
                            mTxtPassword.Enabled = true;
                            progress.Visibility = ViewStates.Invisible;
                            Toast.MakeText(this, "Your Password Is Incorrect.", ToastLength.Long).Show();

                        });
                    }

                })).Start();
            };
        }
    }
}