namespace UrbanGame.Database.Models
{
    public class EndGameObjectiveStep : ObjectiveStep
    {
        public EndGameObjectiveStep()
        {
            ObjectiveStepType = typeof(EndGameObjectiveStep).FullName;
        }

        public string EndGameMessageText { get; set; }
    }
}
