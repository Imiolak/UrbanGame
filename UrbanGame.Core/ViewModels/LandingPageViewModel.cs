using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using UrbanGame.Core.Interactions;
using UrbanGame.Core.Services;

namespace UrbanGame.Core.ViewModels
{
    public class LandingPageViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IApplicationVariableService _applicationVariableService;

        public LandingPageViewModel(IMvxNavigationService navigationService, 
            IApplicationVariableService applicationVariableService)
        {
            _navigationService = navigationService;
            _applicationVariableService = applicationVariableService;
        }

        public IMvxCommand StartGameCommand => new MvxCommand(StartGame);

        public IMvxCommand ActionNotImplementedCommand => new MvxCommand(ActionNotImplemented);

        private readonly MvxInteraction<DialogInteraction> _showNotImplementedDialogIteraction 
            = new MvxInteraction<DialogInteraction>();

        public IMvxInteraction<DialogInteraction> ShowNotImplementedDialogIteraction =>
            _showNotImplementedDialogIteraction;
        
        public override void ViewAppearing()
        {
            var gameStarted = _applicationVariableService.GetValueByKey("GameStarted");
            if (gameStarted != null && bool.Parse(gameStarted))
            {
                _navigationService.Navigate<GamePageViewModel>();
            }

            base.ViewAppearing();
        }
        
        private void StartGame()
        {
            _applicationVariableService.SetValue("GameStarted", true.ToString());
            _navigationService.Navigate<GamePageViewModel>();
        }

        private void ActionNotImplemented()
        {
            _showNotImplementedDialogIteraction.Raise(new DialogInteraction
            {
                Text = "Soon\u2122"
            });
        }
    }
}