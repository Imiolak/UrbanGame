using MvvmCross.Core.ViewModels;
using UrbanGame.Core.Services;

namespace UrbanGame.Core.ViewModels.Game
{
    public class ObjectiveNavigationViewModel : MvxViewModel
    {
        private readonly IApplicationVariableService _applicationVariableService;

        public ObjectiveNavigationViewModel(IApplicationVariableService applicationVariableService)
        {
            _applicationVariableService = applicationVariableService;
        }

        private int _currentObjective = 1;
        public int CurrentObjective
        {
            get => _currentObjective;
            set
            {
                _currentObjective = value;
                RaisePropertyChanged(nameof(CurrentObjective));
            }
        }

        public int NumberOfObjectives => 10;

        public MvxCommand PreviousObjectiveCommand => new MvxCommand(PreviousObjective);
        
        public MvxCommand NextObjectiveCommand => new MvxCommand(NextObjective);

        private void PreviousObjective()
        {
            CurrentObjective--;
        }

        private void NextObjective()
        {
            CurrentObjective++;
        }
    }
}
