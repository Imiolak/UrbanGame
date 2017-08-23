using MvvmCross.Core.ViewModels;
using UrbanGame.Core.Services;

namespace UrbanGame.Core.ViewModels.Game
{
    public class ObjectiveNavigationViewModel : MvxViewModel
    {
        private readonly IGameStateService _gameStateService;

        public ObjectiveNavigationViewModel(IGameStateService gameStateService)
        {
            _gameStateService = gameStateService;
            _currentObjective = _gameStateService.GetCurrentActiveObjectiveNo();
            ShowViewModel<ObjectiveStepNavigationViewModel>(new
            {
                objectiveNo = _currentObjective
            });
        }
        
        private int _currentObjective;
        public int CurrentObjective
        {
            get => _currentObjective;
            set
            {
                _currentObjective = value;
                RaisePropertyChanged(nameof(CurrentObjective));
                RaisePropertyChanged(nameof(PreviousObjectiveButtonEnabled));
                RaisePropertyChanged(nameof(NextObjectiveButtonEnabled));

                ShowViewModel<ObjectiveStepNavigationViewModel>(new
                {
                    objectiveNo = _currentObjective
                });
            }
        }

        public int NumberOfObjectives => _gameStateService.GetNumberOfObjectives();

        private IMvxCommand _previousObjectiveCommand;
        public IMvxCommand PreviousObjectiveCommand
        {
            get
            {
                if (_previousObjectiveCommand == null)
                    _previousObjectiveCommand = new MvxCommand(PreviousObjective);
                return _previousObjectiveCommand;
            }
        }

        private IMvxCommand _nextObjectiveCommand;
        public IMvxCommand NextObjectiveCommand
        {
            get
            {
                if (_nextObjectiveCommand == null)
                    _nextObjectiveCommand = new MvxCommand(NextObjective);
                return _nextObjectiveCommand;
            }
        }

        public bool PreviousObjectiveButtonEnabled => CurrentObjective > 1;

        //public bool NextObjectiveButtonEnabled => CurrentObjective < _gameStateService.GetCurrentActiveObjectiveNo();
        public bool NextObjectiveButtonEnabled => CurrentObjective < NumberOfObjectives;

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
