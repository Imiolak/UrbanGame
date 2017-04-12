using System.Collections.Generic;
using UrbanGame.Database.Models;

namespace UrbanGame.Game
{
    public class GameStructure
    {
        public int NumberOfObjectives { get; set; }

        public int CurrentObjective { get; set; }

        public int PointsPerMainObjective { get; set; }

        public int PointPerExtraObjective { get; set; }

        public IEnumerable<Objective> Objectives { get; set; }
    }
}
