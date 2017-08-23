using SQLite;

namespace UrbanGame.Core.Models
{
    public class Objective
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed(Unique = true)]
        public int ObjectiveNo { get; set; }
    }
}
