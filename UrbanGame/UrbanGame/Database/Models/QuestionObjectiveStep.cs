namespace UrbanGame.Database.Models
{
    public class QuestionObjectiveStep : ObjectiveStep
    {
        public QuestionObjectiveStep()
        {
            ObjectiveStepType = typeof(QuestionObjectiveStep).FullName;
        }

        public string Question { get; set; }

        public string AnswersString { get; set; }
    }
}
