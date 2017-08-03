using Android.App;
using Android.Content;
using Android.Support.V7.App;
using System;
using System.Threading.Tasks;

namespace UrbanGame.Droid.Activities
{
    [Activity(Theme = "@style/UrbanGame.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnResume()
        {
            base.OnResume();
            var startupTask = new Task(DispatchLandingPage);
            startupTask.Start();
        }

        private async void DispatchLandingPage()
        {
            await Task.Delay(TimeSpan.FromSeconds(5));
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}
