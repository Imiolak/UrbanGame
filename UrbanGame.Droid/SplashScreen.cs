using Android.App;
using Android.Content.PM;
using MvvmCross.Droid.Views;
using UrbanGame.Droid.Resources;

namespace UrbanGame.Droid
{
    [Activity(Label = "UrbanGame.Droid", 
        MainLauncher = true, 
        Icon = "@drawable/icon", 
        Theme = "@style/Theme.Splash", 
        NoHistory = true, 
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : MvxSplashScreenActivity
    {
        public SplashScreen() : base(Resource.Layout.splash_screen)
        {
        }
    }
}
