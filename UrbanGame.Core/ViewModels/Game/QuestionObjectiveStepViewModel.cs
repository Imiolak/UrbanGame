using MvvmCross.Core.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using UrbanGame.Core.Extensions;
using UrbanGame.Core.Models;
using UrbanGame.Core.Services;

namespace UrbanGame.Core.ViewModels.Game
{
    public class QuestionObjectiveStepViewModel : MvxViewModel
    {
        private readonly IObjectiveService _objectiveService;
        private QuestionObjectiveStep _objectiveStep;

        public QuestionObjectiveStepViewModel(IObjectiveService objectiveService)
        {
            _objectiveService = objectiveService;
        }

        public void Init(int objectiveNo, int orderInObjective)
        {
            _objectiveStep = _objectiveService.GetObjectiveStep<QuestionObjectiveStep>(objectiveNo, orderInObjective);

            var answers = _objectiveStep.AnswersString
                .Split(';')
                .Where(a => !string.IsNullOrWhiteSpace(a))
                .Select(a => new AnswerViewModel {Text = a})
                .ToList();
            answers.First().IsCorrect = true;
            answers.Shuffle();

            Answers = new MvxObservableCollection<AnswerViewModel>(answers);
        }

        public string Question => _objectiveStep.Question;

        public ObservableCollection<AnswerViewModel> Answers { get; private set; }
    }
}
