using Android.Content;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Views;

namespace UrbanGame.Droid.Custom
{
    public class CustomAndroidPresenter : MvxAndroidViewPresenter
    {
        private readonly IMvxAndroidViewPresenter _innerPresenter;

        public CustomAndroidPresenter(IMvxAndroidViewPresenter innerPresenter)
        {
            _innerPresenter = innerPresenter;
        }

        public override void Show(MvxViewModelRequest request)
        {
            if (request?.PresentationValues != null
                && request.PresentationValues.ContainsKey("ClearBackStack"))
            {
                var intent = CreateIntentForRequest(request);
                intent.SetFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);

                base.Show(intent);
                return;
            }

            _innerPresenter.Show(request);
        }
    }
}
