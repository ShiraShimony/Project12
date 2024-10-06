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
    [Activity(Label = "SignUpActivity")]
    public class SignUpActivity : Activity
    {
        EditText etSignUpUsername, etSignUpPassword;
        Button btnSignUpConfirm;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_sign_up);

            etSignUpUsername = FindViewById<EditText>(Resource.Id.etSignUpUsername);
            etSignUpPassword = FindViewById<EditText>(Resource.Id.etSignUpPassword);
            btnSignUpConfirm = FindViewById<Button>(Resource.Id.btnSignUpConfirm);

            btnSignUpConfirm.Click += BtnSignUpConfirm_Click;
        }

        private void BtnSignUpConfirm_Click(object sender, System.EventArgs e)
        {
            string username = etSignUpUsername.Text;
            string password = etSignUpPassword.Text;

            // Add sign-up logic here (e.g., save user data, validation)
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Toast.MakeText(this, "Please fill all fields", ToastLength.Short).Show();
            }
            else
            {
                Toast.MakeText(this, "Sign Up Successful", ToastLength.Short).Show();
                // After successful sign-up, you can navigate back to the login screen
                Finish(); // This will close the current activity and go back to the previous one (Sign In)
            }
        }
    }
}