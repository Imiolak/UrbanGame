using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using UrbanGame.Core.Services;

namespace UrbanGame.Core.ViewModels
{
    public class GamePageViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IApplicationVariableService _applicationVariableService;

        public GamePageViewModel(IMvxNavigationService navigationService,
            IApplicationVariableService applicationVariableService)
        {
            _navigationService = navigationService;
            _applicationVariableService = applicationVariableService;
        }

        public MvxCommand ResetGameCommand => new MvxCommand(ResetGame);

        private void ResetGame()
        {
            _applicationVariableService.SetValue("GameStarted", false.ToString());
            _navigationService.Navigate<LandingPageViewModel>();
        }
    }
}
