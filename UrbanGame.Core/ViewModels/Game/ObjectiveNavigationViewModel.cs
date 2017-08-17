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
        }

        public int NumberOfObjectives => _gameStateService.GetNumberOfObjectives();

        private int _currentObjective = 2;
        public int CurrentObjective
        {
            get => _currentObjective;
            set
            {
                _currentObjective = value;
                RaisePropertyChanged(nameof(CurrentObjective));
                RaisePropertyChanged(nameof(PreviousObjectiveButtonEnabled));
                RaisePropertyChanged(nameof(NextObjectiveButtonEnabled));
            }
        }
        
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
