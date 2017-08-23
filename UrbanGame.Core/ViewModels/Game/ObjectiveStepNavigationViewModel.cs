using MvvmCross.Core.ViewModels;
using UrbanGame.Core.Services;

namespace UrbanGame.Core.ViewModels.Game
{
    public class ObjectiveStepNavigationViewModel : MvxViewModel
    {
        private readonly IObjectiveService _objectiveService;
        private int _objectiveNo;

        public ObjectiveStepNavigationViewModel(IObjectiveService objectiveService)
        {
            _objectiveService = objectiveService;
        }

        public void Init(int objectiveNo)
        {
            _objectiveNo = objectiveNo;
        }

        private int _currentObjectiveStep = 1;
        public int CurrentObjectiveStep
        {
            get => _currentObjectiveStep;
            set
            {
                _currentObjectiveStep = value;
                RaisePropertyChanged(nameof(CurrentObjectiveStep));
                RaisePropertyChanged(nameof(PreviousObjectiveStepButtonEnabled));
                RaisePropertyChanged(nameof(NextObjectiveStepButtonEnabled));

                ShowViewModel<ObjectiveStepViewModel>(new
                {
                    objectiveNo = _objectiveNo,
                    orderInObjective = _currentObjectiveStep
                });
            }
        }
        
        public int NumberOfObjectiveSteps => _objectiveService.GetNumberOfObjectiveStepsObjectiveNo(_objectiveNo);

        private IMvxCommand _previousObjectiveStepCommand;
        public IMvxCommand PreviousObjectiveStepCommand
        {
            get
            {
                if (_previousObjectiveStepCommand == null)
                    _previousObjectiveStepCommand = new MvxCommand(PreviousObjectiveStep);
                return _previousObjectiveStepCommand;
            }
        }
        
        private IMvxCommand _nextObjectiveStepCommand;
        public IMvxCommand NextObjectiveStepCommand
        {
            get
            {
                if (_nextObjectiveStepCommand == null)
                    _nextObjectiveStepCommand = new MvxCommand(NextObjectiveStep);
                return _nextObjectiveStepCommand;
            }
        }

        public bool PreviousObjectiveStepButtonEnabled => CurrentObjectiveStep > 1;
        
        public bool NextObjectiveStepButtonEnabled => CurrentObjectiveStep < NumberOfObjectiveSteps;

        private void PreviousObjectiveStep()
        {
            CurrentObjectiveStep--;
        }

        private void NextObjectiveStep()
        {
            CurrentObjectiveStep++;
        }
    }
}
