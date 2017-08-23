using MvvmCross.Core.ViewModels;
using UrbanGame.Core.Models;
using UrbanGame.Core.Services;

namespace UrbanGame.Core.ViewModels.Game
{
    public class TextObjectiveStepViewModel : MvxViewModel
    {
        private readonly IObjectiveService _objectiveService;
        private TextObjectiveStep _objectiveStep;

        public TextObjectiveStepViewModel(IObjectiveService objectiveService)
        {
            _objectiveService = objectiveService;
        }

        public void Init(int objectiveNo, int orderInObjective)
        {
            _objectiveStep = _objectiveService.GetObjectiveStep<TextObjectiveStep>(objectiveNo, orderInObjective);
        }

        public string Content => _objectiveStep.Text;
    }
}
