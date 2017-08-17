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
        
        public string ObjNo { get; set; }
        
        public string ObjTitle { get; set; }

        public IMvxCommand ResetGameCommand => new MvxCommand(ResetGame);

        public IMvxCommand SetViewModelCommand => new MvxCommand(SetViewModel);

        private void SetViewModel()
        {
            ShowViewModel<ObjectiveHeaderViewModel>(new
            {
                objectiveNo = ObjNo,
                objectiveTitle = ObjTitle
            });
        }

        private void ResetGame()
        {
            _applicationVariableService.SetValue("GameStarted", false.ToString());
            ShowViewModelAndClearBackStack<MenuViewModel>();
        }
    }
}
