using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content;
using System.Threading.Tasks;

namespace Bitcoin.Curses.Droid
{
    [Activity(Label = "Bitcoin Exchange Rate", Icon = "@drawable/icon", Theme = "@style/MainTheme.Splash", MainLauncher = true, NoHistory = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class SplashScreen : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //Xamarin.Forms.Forms.Init(this, savedInstanceState);
        }

        protected override void OnResume()
        {
            base.OnResume();

            Task tmpStartupWork = new Task(() =>
            {
                StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            });

            tmpStartupWork.Start();
        }
    }
}