using Android.App;
using Android.OS;
using MvvmCross.Droid.Views;

namespace UrbanGame.Droid.Views
{
    [Activity(Label = "View for GamePageViewModel", 
        NoHistory = true)]
    public class GamePageView : MvxActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.GamePageView);
        }
    }
}
