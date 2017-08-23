using MvvmCross.Core.ViewModels;
using UrbanGame.Core.Models;
using UrbanGame.Core.Services;

namespace UrbanGame.Core.ViewModels.Game
{
    public class ObjectiveStepViewModel : MvxViewModel
    {
        private readonly IObjectiveService _objectiveService;
        private ObjectiveStep _objective;

        public ObjectiveStepViewModel(IObjectiveService objectiveService)
        {
            _objectiveService = objectiveService;
        }

        public void Init(int objectiveNo, int orderInObjective)
        {
            _objective = _objectiveService.GetObjectiveStep(objectiveNo, orderInObjective);
        }

        public string ObjectiveStepText => _objective.Text;
    }
}
