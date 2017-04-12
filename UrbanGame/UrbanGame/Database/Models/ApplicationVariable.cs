using SQLite.Net.Attributes;

namespace UrbanGame.Database.Models
{
    public class ApplicationVariable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed(Unique = true)]
        public string Name { get; set; }

        public string Value { get; set; }
    }

    public enum ApplicationVariables
    {
        GameStarted,
        GameStartedTimestamp,
        GameEnded,
        GameEndedTimestamp,
        NumberOfObjectives,
        CurrentObjective,
        PointsPerMainObjective,
        PointsPerExtraObjective
    }
}
