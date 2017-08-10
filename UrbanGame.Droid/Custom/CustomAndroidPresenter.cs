using Android.Content;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Views;

namespace UrbanGame.Droid.Custom
{
    public class CustomAndroidPresenter : MvxAndroidViewPresenter
    {
        public override void Show(MvxViewModelRequest request)
        {
            if (request?.PresentationValues != null
                && request.PresentationValues.ContainsKey("ClearBackStack"))
            {
                var intent = base.CreateIntentForRequest(request);
                intent.SetFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);

                base.Show(intent);
                return;
            }

            base.Show(request);
        }
    }
}
