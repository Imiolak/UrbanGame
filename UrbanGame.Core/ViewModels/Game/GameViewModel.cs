using MvvmCross.Core.ViewModels;
using UrbanGame.Core.Custom;
using UrbanGame.Core.Services;
using UrbanGame.Core.ViewModels.Menu;

namespace UrbanGame.Core.ViewModels.Game
{
    public class GameViewModel : MvxViewModelWithNoBackStackNavigation
    {
        private readonly IGameStateService _gameStateService;

        public GameViewModel(IGameStateService gameStateService)
        {
            _gameStateService = gameStateService;
        }
        
        public string ObjNo { get; set; }
        
        public string ObjTitle { get; set; }

        public IMvxCommand ResetGameCommand => new MvxCommand(ResetGame);

        private void ResetGame()
        {
            _gameStateService.ResetGame();
            ShowViewModelAndClearBackStack<MenuViewModel>();
        }
    }
}
