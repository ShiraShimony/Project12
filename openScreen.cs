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
using Xamarin.Essentials;

namespace Project12
{
    [Activity(Label = "openScreen")]
    public class openScreen : Activity
    {
        EditText etUsername;
        EditText etPassword;
        Button btnSignIn;
        FireBaseManager firebase;
        ISharedPreferences sharedPreferences;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.open_screen);

            etUsername = FindViewById<EditText>(Resource.Id.etUsername);
            etPassword = FindViewById<EditText>(Resource.Id.etPassword);
            btnSignIn = FindViewById<Button>(Resource.Id.btnSignIn);
            firebase = new FireBaseManager();
            sharedPreferences = this.GetSharedPreferences("details", FileCreationMode.Private);

            btnSignIn.Click += BtnSignIn_Click;
        }

        async private void BtnSignIn_Click(object sender, System.EventArgs e)
        {
            string username = etUsername.Text;
            string password = etPassword.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                Toast.MakeText(this, "Please enter both username and password", ToastLength.Short).Show();
                return;
            }


            Account account = await firebase.GetAccount(username);
            if (account == null)
            {
                Toast.MakeText(this, "Username does not exist", ToastLength.Short).Show();
            }
            else if (firebase.CheckPassword(username, password).Result)
            {
                ISharedPreferencesEditor editor = sharedPreferences.Edit();

                var intent = new Intent(this, typeof(MainPageActivity));
                StartActivity(intent);
            }
            else
            {
                Toast.MakeText(this, "Incurrect password", ToastLength.Short);
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