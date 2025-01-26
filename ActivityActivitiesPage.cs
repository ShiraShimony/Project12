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
using Android.Graphics;
using Javax.Security.Auth;


namespace Project12
{
    [Activity(Label = "ActivityActivitiesPage")]
    public class ActivityActivitiesPage : Activity
    {
        Button btnScreen1, btnScreen2, btnScreen3, btnTrans, btnRec, btnSend;
        EditText etDest, etAmmount;
        FireBaseManager firebase;
        ISharedPreferences sharedPreferences;
        Account thisAccount;
        bool Trans, Rec;

        async protected override void OnCreate(Bundle savedInstanceState)
        {
            Trans = Rec = false;
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activityies);

            btnScreen1 = FindViewById<Button>(Resource.Id.btnMainScreen);
            btnScreen2 = FindViewById<Button>(Resource.Id.btnScreenActivities);
            btnScreen3 = FindViewById<Button>(Resource.Id.btnScreenFlow);
            btnTrans = FindViewById<Button>(Resource.Id.btnTransfer);
            btnRec = FindViewById<Button>(Resource.Id.btnRequest);
            btnSend = FindViewById<Button>(Resource.Id.btnSend);
            etDest = FindViewById<EditText>(Resource.Id.etDest);
            etAmmount = FindViewById<EditText>(Resource.Id.etAmmount);


            sharedPreferences = this.GetSharedPreferences("details", FileCreationMode.Private);
            firebase = new FireBaseManager();
            thisAccount = await firebase.GetAccount(sharedPreferences.GetString("id", null));


            // Set click events to navigate between screens
            btnScreen2.Click += (s, e) =>
            {
                // You are already on Screen 1, so no intent here
                Toast.MakeText(this, "Already on Screen Activities", ToastLength.Short).Show();
            };

            btnScreen1.Click += (s, e) =>
            {
                var intent = new Intent(this, typeof(MainPageActivity));
                StartActivity(intent);
            };

            btnScreen3.Click += (s, e) =>
            {
                var intent = new Intent(this, typeof(FlowPageActivity));
                StartActivity(intent);
            };

            btnTrans.Click += (s, e) =>
            {
                Trans = true;
                Rec = false;
                btnTrans.SetBackgroundColor(Color.ParseColor("#FFFFFF")); ;
                btnRec.SetBackgroundColor(Color.ParseColor("#9956ab"));
                btnTrans.SetTextColor(Color.ParseColor("#9956ab"));
                btnRec.SetTextColor(Color.ParseColor("#FFFFFF"));
                makeVisable();
            };

            btnRec.Click += (s, e) =>
            {
                Trans = false;
                Rec = true;
                btnRec.SetBackgroundColor(Color.ParseColor("#FFFFFF")); ;
                btnTrans.SetBackgroundColor(Color.ParseColor("#9956ab"));
                btnRec.SetTextColor(Color.ParseColor("#9956ab"));
                btnTrans.SetTextColor(Color.ParseColor("#FFFFFF"));
                makeVisable();
            };

            btnSend.Click += BtnSend_Click;


        }

        async private void BtnSend_Click(object sender, System.EventArgs e)
        {
            Account account = await firebase.GetAccount(etDest.Text);
            if (account == null)
            {
                Toast.MakeText(this, "Destenation does not exist", ToastLength.Short).Show();
            }
            int n;
            try
            {
                n = Int32.Parse(etAmmount.Text);
            }
            catch (FormatException ex)
            {
                Toast.MakeText(this, "Invalid Ammount", ToastLength.Short).Show();
                return;
            }

            if (Trans)
            {
                account.Remainder = account.Remainder + n;
                thisAccount.Remainder = thisAccount.Remainder -n;
                await firebase.DeleteAccount(account.Name);
                await firebase.DeleteAccount(thisAccount.Name);
                await firebase.AddAccount(account);
                await firebase.AddAccount(thisAccount);


                DateTime today = DateTime.Now;
                string date = today.ToString("MM/dd/yyyy");

                /* List<Transfer> newTransThis = thisAccount.Transfers;
                if(newTransThis == null) Toast.MakeText(this, "Invalid Ammount", ToastLength.Short).Show();
                newTransThis.Add(new Transfer(date, account.Name, -n));
                thisAccount.Transfers = newTransThis;

                List<Transfer> newTransDest = account.Transfers;
                newTransDest.Add(new Transfer(date, thisAccount.Name, n));
                account.Transfers = newTransDest; */

            }
            else
            {
                Toast.MakeText(this, "not handled", ToastLength.Short).Show();
            }
            Toast.MakeText(this, "Done", ToastLength.Short).Show();
            return;

        }
        private void makeVisable()
        {
            btnSend.Visibility = ViewStates.Visible;
            etAmmount.Visibility = ViewStates.Visible;
            etDest.Visibility = ViewStates.Visible;
        }
    }
}