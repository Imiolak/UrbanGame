using Android.OS;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platform;
using UrbanGame.Core.ViewModels.Game;

namespace UrbanGame.Droid.Views.Game
{
    public class ObjectiveNavigationFragment : MvxFragment<ObjectiveNavigationViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            ViewModel = Mvx.IocConstruct<ObjectiveNavigationViewModel>();
            return this.BindingInflate(Resource.Layout.fragment_objectivenavigation, null);
        }
    }
}
