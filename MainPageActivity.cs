using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace Project12
{
    [Activity(Label = "MainPageActivity", MainLauncher = true)]
    public class MainPageActivity : Activity
    {
        Button btnScreen1, btnScreen2, btnScreen3;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.main_page);

            btnScreen1 = FindViewById<Button>(Resource.Id.btnMainScreen);
            btnScreen2 = FindViewById<Button>(Resource.Id.btnScreenActivities);
            btnScreen3 = FindViewById<Button>(Resource.Id.btnScreenFlow);

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
