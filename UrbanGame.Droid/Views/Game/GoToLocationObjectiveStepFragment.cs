using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Shared.Attributes;
using MvvmCross.Droid.Support.V4;
using UrbanGame.Core.ViewModels.Game;

namespace UrbanGame.Droid.Views.Game
{
    [MvxFragment(typeof(GameViewModel), Resource.Id.objectiveStepViewContainer)]
    public class GoToLocationObjectiveStepFragment : MvxFragment<GoToLocationObjectiveStepViewModel>, IOnMapReadyCallback
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            return this.BindingInflate(Resource.Layout.fragment_gotolocationobjectivestep, null);
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            var latlng = new LatLng(ViewModel.Latitude, ViewModel.Longitude);
            var position = CameraUpdateFactory.NewLatLngZoom(latlng, 10);

            googleMap.MoveCamera(position);
        }
    }
}
