    using Android.App;
    using Android.OS;
    using Android.Widget;
    using Android.Content;
    using Android.Preferences;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    namespace Project12
    {
        /// <summary>
        /// Activity that displays account details including balance and recent transfers.
        /// Provides UI for initiating transfers, requests, and viewing transfer history.
        /// </summary>
        [Activity(Label = "AccountDashboardActivity")]
        public class MainPage : Activity
        {
            private TextView textViewName;      // TextView to display user name
            private TextView textViewReminder;      // TextView to display account balance
            private Button buttonTransfer;          // Button to initiate transfer
            private Button buttonRequest;          // Button to initiate request
            private ListView listViewTransfers;      // ListView to display transfer items

            private FirebaseManager firebaseManager;  // Custom manager to interact with Firebase Realtime Database
            private Account account;                  // Current user's account instance
            private ISharedPreferences sharedPreferences;  // SharedPreferences are used for persistent key-value storage between sessions

            /// <summary>
            /// Called when the activity is created. Initializes UI and loads account data.
            /// </summary>
            /// <param name="savedInstanceState">If the activity is being re-initialized after previously being shut down then this Bundle contains the data it most recently supplied. Otherwise, it is null.</param>
            protected override async void OnCreate(Bundle savedInstanceState)
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.main_page);

                firebaseManager = new FirebaseManager("https://project12-f950c-default-rtdb.europe-west1.firebasedatabase.app/");

                // Instead of Intent extras, retrieve the account ID from SharedPreferences

                sharedPreferences = this.GetSharedPreferences("details", FileCreationMode.Private);
                string accountId = sharedPreferences.GetString("id", null);

                if (accountId == null)
                {
                    new Android.App.AlertDialog.Builder(this)
                        .SetTitle("Error")
                        .SetMessage("Account ID not found. Please log in again.")
                        .SetPositiveButton("OK", (sender, args) =>
                        {
                            Intent intent = new Intent(this, typeof(SignIn));
                            StartActivity(intent);
                            Finish(); // close current activity
                        })
                        .SetCancelable(false)
                        .Show();

                    return;
                }


                // Linking layout components to fields
                textViewName = FindViewById<TextView>(Resource.Id.textViewName);
                textViewReminder = FindViewById<TextView>(Resource.Id.textViewReminder);
                buttonTransfer = FindViewById<Button>(Resource.Id.buttonTransfer);
                buttonRequest = FindViewById<Button>(Resource.Id.buttonRequest);
                listViewTransfers = FindViewById<ListView>(Resource.Id.listViewTransfers);

                try
                {
                    account = await firebaseManager.GetAccountAsync(accountId);

                    textViewName.Text = "Name: " + account.Name;
                    textViewReminder.Text = "Remainder: " + account.Remainder;

                    if (account.Transfers != null)
                    {

                            listViewTransfers.Adapter = new TransferAdapter(this, account.GetWaitingTransfers(), accountId, "https://project12-f950c-default-rtdb.europe-west1.firebasedatabase.app/");
                    }
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, "Error: " + ex.Message, ToastLength.Long).Show();
                }

                buttonTransfer.Click += delegate
                {
                    Toast.MakeText(this, "Transfer clicked", ToastLength.Short).Show();
                    // TODO: Launch transfer activity
                };

                buttonRequest.Click += delegate
                {
                    Toast.MakeText(this, "Request clicked", ToastLength.Short).Show();
                    // TODO: Launch request activity
                };

                
            }
        }
    }