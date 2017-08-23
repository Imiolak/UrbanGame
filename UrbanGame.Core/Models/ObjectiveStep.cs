using SQLite;

namespace UrbanGame.Core.Models
{
    public class ObjectiveStep
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed(Name = "ObjectiveNoOrder", Order = 1, Unique = true)]
        public int ObjectiveNo { get; set; }

        [Indexed(Name = "ObjectiveNoOrder", Order = 2, Unique = true)]
        public int OrderInObjective { get; set; }

        public string Text { get; set; }
    }
}
