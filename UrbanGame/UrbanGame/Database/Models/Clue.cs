using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace UrbanGame.Database.Models
{
    public class Clue
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        
        [Indexed(Name = "ClueNumber", Order = 1, Unique = true)]
        public int Major { get; set; }

        [Indexed(Name = "ClueNumber", Order = 2, Unique = true)]
        public int Minor { get; set; }

        public string Content { get; set; }

        [ForeignKey(typeof(Objective))]
        public int ObjectiveId { get; set; }
    }
}