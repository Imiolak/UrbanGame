using Android.App;
using Android.OS;
using MvvmCross.Droid.Support.V4;
using UrbanGame.Core.ViewModels.Game;

namespace UrbanGame.Droid.Views.Game
{
    [Activity]
    public class GameActivity : MvxCachingFragmentActivity<GameViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.activity_game);
        }
    }
}
