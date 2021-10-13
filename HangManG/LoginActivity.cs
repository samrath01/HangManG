using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using HangManG.Common;
using HangManG.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HangManG
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class LoginActivity : Activity
    {
        Button btnLogin, btnRegister;
        EditText etName, etPassword;
        HangmanConnection connection;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_login);

            connection = new HangmanConnection(this);
            etName = FindViewById<EditText>(Resource.Id.etName);
            etPassword = FindViewById<EditText>(Resource.Id.etPassword);
            btnLogin = FindViewById<Button>(Resource.Id.btnLogin);
            btnRegister = FindViewById<Button>(Resource.Id.btnRegister);

            btnLogin.Click += BtnLogin_Click;
            btnRegister.Click += BtnRegister_Click;


        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            string name = etName.Text.Trim();
            string password = etPassword.Text;
            string message = "";
            if (name.Length == 0 || password.Length == 0)
            {
                message = "Please Fill All Boxes";
            }
            else
            {
                Profile profile = new Profile();
                profile.ProfileName = name;
                profile.Password = password;
                if (connection.SaveProfile(profile))
                {
                    message = "Profile is saved";
                    Intent intent = new Intent(this, typeof(MainActivity));
                    intent.PutExtra("name", name);
                    StartActivity(intent);
                    Finish();
                }
                else
                {
                    message = "Profile is not Saved Due To " + connection.GetError();
                }
            }
            Toast.MakeText(this, message, ToastLength.Long).Show();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string name = etName.Text.Trim();
            string password = etPassword.Text;
            string message = "";
            if (name.Length == 0 || password.Length == 0)
            {
                message = "Please Fill All Boxes";
            }
            else
            {
                if (name.Equals("admin") && password.Equals("admin"))
                {
                    message = "Welcome To Admin!!!";
                    Intent intent = new Intent(this, typeof(AdminActivity));
                    StartActivity(intent);
                    Finish();
                }
                else if (connection.CheckProfile(name, password))
                {
                    message = "Welcome To Hangman";
                    Intent intent = new Intent(this, typeof(MainActivity));
                    intent.PutExtra("name", name);
                    StartActivity(intent);
                    Finish();
                }
                else
                {
                    message = "Invalid Name and Password Given";
                }

            }
            Toast.MakeText(this, message, ToastLength.Long).Show();
        }
    }
}