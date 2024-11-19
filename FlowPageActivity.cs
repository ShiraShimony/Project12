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
    [Activity(Label = "FlowPageActivity")]
    public class FlowPageActivity : Activity
    {
        Button btnScreen1, btnScreen2, btnScreen3;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.flow_page);

            btnScreen1 = FindViewById<Button>(Resource.Id.btnMainScreen);
            btnScreen2 = FindViewById<Button>(Resource.Id.btnScreenActivities);
            btnScreen3 = FindViewById<Button>(Resource.Id.btnScreenFlow);

            // Set click events to navigate between screens
            btnScreen3.Click += (s, e) =>
            {
                // You are already on Screen 1, so no intent here
                Toast.MakeText(this, "Already on Screen Activities", ToastLength.Short).Show();
            };

            btnScreen1.Click += (s, e) =>
            {
                var intent = new Intent(this, typeof(MainPageActivity));
                StartActivity(intent);
            };

            btnScreen2.Click += (s, e) =>
            {
                var intent = new Intent(this, typeof(ActivityActivitiesPage));
                StartActivity(intent);
            };
        }
    }
}