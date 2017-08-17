using MvvmCross.Core.ViewModels;
using UrbanGame.Core.Services;
using UrbanGame.Core.ViewModels.Game;
using UrbanGame.Core.ViewModels.Menu;

namespace UrbanGame.Core.Custom
{
    public class CustomAppStart : MvxNavigatingObject, IMvxAppStart
    {
        private readonly IGameStateService _gameStateService;

        public CustomAppStart(IGameStateService gameStateService)
        {
            _gameStateService = gameStateService;
        }

        public void Start(object hint = null)
        {
            if (_gameStateService.GetGameStarted())
            {
                ShowViewModel<GameViewModel>();
            }
            else
            {
                ShowViewModel<MenuViewModel>();
            }
        }
    }
}
