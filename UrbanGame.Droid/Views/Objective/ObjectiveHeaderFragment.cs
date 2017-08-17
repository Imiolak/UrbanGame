using Android.OS;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Shared.Attributes;
using MvvmCross.Droid.Support.V4;
using UrbanGame.Core.ViewModels.Game;

namespace UrbanGame.Droid.Views.Objective
{
    [MvxFragment(typeof(GameViewModel), Resource.Id.objectiveHeaderContainer)]
    public class ObjectiveHeaderFragment : MvxFragment<ObjectiveHeaderViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            return this.BindingInflate(Resource.Layout.fragment_objectiveheader, null);
        }
    }
}
