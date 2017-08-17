using Android.OS;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Shared.Attributes;
using MvvmCross.Droid.Support.V4;
using UrbanGame.Core.ViewModels.Game;
using UrbanGame.Core.ViewModels.Objective;

namespace UrbanGame.Droid.Views.Objective
{
    [MvxFragment(typeof(GameViewModel), Resource.Id.objectiveFragmentContainer)]
    public class ObjecviteFragment : MvxFragment<ObjectiveViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            return this.BindingInflate(Resource.Layout.fragment_objective, null);
        }
    }
}
