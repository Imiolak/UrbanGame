using Android.App;
using Android.OS;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Views;
using MvvmCross.Platform.Core;
using UrbanGame.Core.Interactions;
using UrbanGame.Core.ViewModels.Menu;
using ZXing.Mobile;
using Resource = UrbanGame.Droid.Resources.Resource;

namespace UrbanGame.Droid.Views.Menu
{
    [Activity]
    public class MenuActivity : MvxActivity<MenuViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            var set = this.CreateBindingSet<MenuActivity, MenuViewModel>();
            set.Bind(this)
                .For(view => view.DialogInteraction)
                .To(viewModel => viewModel.DialogInteraction).OneTime();
            set.Apply();

            MobileBarcodeScanner.Initialize(Application);
            SetContentView(Resource.Layout.activity_menu);
        }

        private IMvxInteraction<DialogInteraction> _dialogInteraction;
        public IMvxInteraction<DialogInteraction> DialogInteraction
        {
            get => _dialogInteraction;
            set
            {
                if (_dialogInteraction != null)
                    _dialogInteraction.Requested -= OnDialogInteractionRequested;

                _dialogInteraction = value;
                _dialogInteraction.Requested += OnDialogInteractionRequested;
            }
        }

        private void OnDialogInteractionRequested(object sender, MvxValueEventArgs<DialogInteraction> e)
        {
            var dialogInteraction = e.Value;
            var dialog = new AlertDialog.Builder(this)
                .SetTitle(dialogInteraction.Title)
                .SetMessage(dialogInteraction.Text)
                .SetPositiveButton("OK", (o, args) => {})
                .Create();

            dialog.Show();
        }
    }
}
