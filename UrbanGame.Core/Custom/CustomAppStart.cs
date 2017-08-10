using MvvmCross.Core.ViewModels;
using UrbanGame.Core.Services;
using UrbanGame.Core.ViewModels.Game;
using UrbanGame.Core.ViewModels.Menu;

namespace UrbanGame.Core.Custom
{
    public class CustomAppStart : MvxNavigatingObject, IMvxAppStart
    {
        private readonly IApplicationVariableService _applicationVariableService;

        public CustomAppStart(IApplicationVariableService applicationVariableService)
        {
            _applicationVariableService = applicationVariableService;
        }

        public void Start(object hint = null)
        {
            var gameStarted = _applicationVariableService.GetValueByKey("GameStarted");
            if (gameStarted != null && bool.Parse(gameStarted))
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
