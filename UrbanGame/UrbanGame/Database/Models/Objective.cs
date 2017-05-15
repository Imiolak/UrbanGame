using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace UrbanGame.Database.Models
{
    public class Objective
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed(Unique = true)]
        public int ObjectiveNo { get; set; }

        public bool IsStarted { get; set; }

        public bool IsCompleted { get; set; }
        
        public int NumberOfExtraObjectives { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Clue> Clues { get; set; }

        [Ignore]
        public Clue MainClue => Clues.SingleOrDefault(c => c.Minor == 0);

        [Ignore]
        public IEnumerable<Clue> ExtraClues => Clues.Where(c => c.Minor != 0).OrderBy(c => c.Minor);
    }
}