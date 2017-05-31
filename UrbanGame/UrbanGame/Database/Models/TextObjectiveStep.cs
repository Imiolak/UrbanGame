namespace UrbanGame.Database.Models
{
    public class TextObjectiveStep : ObjectiveStep
    {
        public TextObjectiveStep()
        {
            ObjectiveStepType = typeof(TextObjectiveStep).FullName;
        }

        public string Text { get; set; }
    }
}
