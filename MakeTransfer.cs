using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System;

namespace Project12
{
    [Activity(Label = "MakeTransfer")]
    public class MakeTransfer : Activity
    {
        private EditText editTextTargetId;
        private EditText editTextAmount;
        private Button buttonSendTransfer;
        private FirebaseManager firebaseManager;
        private string currentAccountId;
        private ISharedPreferences sharedPreferences;
        private bool isRequest;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.make_transfer);

            sharedPreferences = this.GetSharedPreferences("details", FileCreationMode.Private);
            currentAccountId = sharedPreferences.GetString("id", null);


            editTextTargetId = FindViewById<EditText>(Resource.Id.editTextPhone);
            editTextAmount = FindViewById<EditText>(Resource.Id.editTextAmount);
            Spinner spinner = FindViewById<Spinner>(Resource.Id.spinnerCurrency);
            TextView currencySymbol = FindViewById<TextView>(Resource.Id.textViewCurrencySymbol);
            buttonSendTransfer = FindViewById<Button>(Resource.Id.buttonSend);


            isRequest = Intent.GetBooleanExtra("isRequestMode", false);//chacking the type of transfer
            if (isRequest)
            {
                buttonSendTransfer.Text = "Send Request";
            }


            firebaseManager = new FirebaseManager("https://project12-f950c-default-rtdb.europe-west1.firebasedatabase.app/"); 


            // רשימת המטבעות
            string[] currencies = { "₪ Shekel", "$ Dollar", "€ Euro" };

            ArrayAdapter adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, currencies);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;

            // מעקב אחר שינוי בבחירת המטבע
            spinner.ItemSelected += (s, e) =>
            {
                string selected = currencies[e.Position];
                string symbol = "₪"; // ברירת מחדל

                switch (selected)   
                {
                    case "$ Dollar":
                        symbol = "$";
                        break;
                    case "€ Euro":
                        symbol = "€";
                        break;
                }

                currencySymbol.Text = symbol;
            };



            buttonSendTransfer.Click += async (sender, e) =>
            {
                string targetId = editTextTargetId.Text.Trim();
                string amountText = editTextAmount.Text.Trim();

                if (string.IsNullOrWhiteSpace(targetId) || string.IsNullOrWhiteSpace(amountText))
                {
                    Toast.MakeText(this, "Please fill all fields", ToastLength.Short).Show();
                    return;
                }

                if (!int.TryParse(amountText, out int amount) || amount <= 0)
                {
                    Toast.MakeText(this, "Amount must be a positive number", ToastLength.Short).Show();
                    return;
                }


                try
                {
                    await firebaseManager.SendTransferAsync(currentAccountId, targetId, amount, isRequest);
                    Toast.MakeText(this, "Transfer sent", ToastLength.Short).Show();
                    Finish();
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, "Error: " + ex.Message, ToastLength.Long).Show();
                }
            };
        }
    }
}
