using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace Project12
{
    [Activity(Label = "MainPageActivity")]
    public class MainPageActivity : Activity
    {
        Button btnScreen1, btnScreen2, btnScreen3;
        TextView tvName, tvAuccVal;
        FireBaseManager firebase;
        ISharedPreferences sharedPreferences;
        Account thisAccount;

        async protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.main_page);

            btnScreen1 = FindViewById<Button>(Resource.Id.btnMainScreen);
            btnScreen2 = FindViewById<Button>(Resource.Id.btnScreenActivities);
            btnScreen3 = FindViewById<Button>(Resource.Id.btnScreenFlow);
            tvName = FindViewById<TextView>(Resource.Id.textViewName);
            tvAuccVal = FindViewById<TextView>(Resource.Id.textViewAuccVal);

            sharedPreferences = this.GetSharedPreferences("details", FileCreationMode.Private);
            firebase = new FireBaseManager();
            thisAccount = await firebase.GetAccount(sharedPreferences.GetString("id", null));
            //does not handle null option' because no such possibility
            tvName.Text = "Hello " + thisAccount.Name;
            tvAuccVal.Text ="Current account: " + thisAccount.Remainder.ToString();


            // Set click events to navigate between screens
            btnScreen1.Click += (s, e) => {
                // You are already on Screen 1, so no intent here
                Toast.MakeText(this, "Already on Screen 1", ToastLength.Short).Show();
            };

            btnScreen2.Click += (s, e) => {
                var intent = new Intent(this, typeof(ActivityActivitiesPage));
                StartActivity(intent);
            };

            btnScreen3.Click += (s, e) => {
                var intent = new Intent(this, typeof(FlowPageActivity));
                StartActivity(intent);
            };

            
        }
    }
}
