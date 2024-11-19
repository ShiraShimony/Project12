using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project12
{
    [Activity(Label = "openScreen")]
    public class openScreen : Activity
    {
        EditText etUsername;
        EditText etPassword;
        Button btnSignIn;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.open_screen);

            etUsername = FindViewById<EditText>(Resource.Id.etUsername);
            etPassword = FindViewById<EditText>(Resource.Id.etPassword);
            btnSignIn = FindViewById<Button>(Resource.Id.btnSignIn);

            btnSignIn.Click += BtnSignIn_Click;
        }

        private void BtnSignIn_Click(object sender, System.EventArgs e)
        {
            string username = etUsername.Text;
            string password = etPassword.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                Toast.MakeText(this, "Please enter both username and password", ToastLength.Short).Show();
                return;
            }

            // Simulate login validation (You can replace this with actual validation)
            if (username == "admin" && password == "password")
            {
                var intent = new Intent(this, typeof(MainPageActivity));
                StartActivity(intent);
            }
            else
            {
                Toast.MakeText(this, "Invalid credentials", ToastLength.Short).Show();
            }
        }

        private void BtnSignUp_Click(object sender, System.EventArgs e)
        {
            // Navigate to Sign Up activity
            var intent = new Intent(this, typeof(SignUpActivity));
            StartActivity(intent);
        }
    }
}