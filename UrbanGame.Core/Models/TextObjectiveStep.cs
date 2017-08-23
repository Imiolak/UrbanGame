namespace UrbanGame.Core.Models
{
    public class TextObjectiveStep : ObjectiveStepBase
    {
        public TextObjectiveStep()
        {
            Type = typeof(TextObjectiveStep).FullName;
        }

        public string Text { get; set; }
    }
}
