using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace UrbanGame.Database.Models
{
    public abstract class ObjectiveStep
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int OrderInObjective { get; set; }

        public string ObjectiveStepType { get; set; }

        public bool IsStarted { get; set; } = false;

        public bool IsCompleted { get; set; } = false;

        [ForeignKey(typeof(Objective))]
        public int ObjectiveId { get; set; }
    }
}
