using UrbanGame.Database.Models;

namespace UrbanGame.Game
{
    public class GameStructureHelper
    {
        public GameStructure LoadGameStructureFromDb()
        {
            return new GameStructure
            {
                NumberOfObjectives = int.Parse(App.Database.GetApplicationVariableByName(ApplicationVariables.NumberOfObjectives).Value),
                CurrentObjective = int.Parse(App.Database.GetApplicationVariableByName(ApplicationVariables.CurrentObjective).Value),
                PointsPerMainObjective =
                    int.Parse(App.Database.GetApplicationVariableByName(ApplicationVariables.PointsPerMainObjective).Value),
                PointsPerExtraObjective =
                    int.Parse(App.Database.GetApplicationVariableByName(ApplicationVariables.PointsPerExtraObjective).Value),
                Objectives = App.Database.GetAllObjectives()
            };
        }
    }
}
