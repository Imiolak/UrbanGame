namespace UrbanGame.Database.Models
{
    public class GoToLocationObjectiveStep : ObjectiveStep
    {
        public GoToLocationObjectiveStep()
        {
            ObjectiveStepType = typeof(GoToLocationObjectiveStep).FullName;
        }

        public string Text { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }
    }
}
