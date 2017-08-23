using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using UrbanGame.Core.Models;
using UrbanGame.Core.Services;

namespace UrbanGame.Core.ViewModels.Game
{
    public class ObjectiveStepNavigationViewModel : MvxViewModel
    {
        private readonly IDictionary<string, Type> _viewModelTypesByObjectiveStepType = new Dictionary<string, Type>
        {
            { typeof(TextObjectiveStep).FullName, typeof(TextObjectiveStepViewModel) }
        };
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

                var objectiveStepType = _objectiveService.GetObjectiveStepType(_objectiveNo, _currentObjectiveStep);
                ShowViewModel(_viewModelTypesByObjectiveStepType[objectiveStepType], new
                {
                    objectiveNo = _objectiveNo,
                    orderInObjective = _currentObjectiveStep
                });
            }
        }
        
        public int NumberOfObjectiveSteps => _objectiveService.GetNumberOfObjectiveStepsByObjectiveNo(_objectiveNo);

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
