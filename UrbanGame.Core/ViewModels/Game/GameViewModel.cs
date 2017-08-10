using MvvmCross.Core.ViewModels;
using UrbanGame.Core.Custom;
using UrbanGame.Core.Services;
using UrbanGame.Core.ViewModels.Menu;

namespace UrbanGame.Core.ViewModels.Game
{
    public class GameViewModel : MvxViewModelWithNoBackStackNavigation
    {
        private readonly IApplicationVariableService _applicationVariableService;

        public GameViewModel(IApplicationVariableService applicationVariableService)
        {
            _applicationVariableService = applicationVariableService;
        }

        public MvxCommand ResetGameCommand => new MvxCommand(ResetGame);

        private void ResetGame()
        {
            _applicationVariableService.SetValue("GameStarted", false.ToString());
            ShowViewModelAndClearBackStack<MenuViewModel>();
        }
    }
}
