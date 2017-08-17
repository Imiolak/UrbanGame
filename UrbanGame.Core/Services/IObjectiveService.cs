using UrbanGame.Core.Models;

namespace UrbanGame.Core.Services
{
    public interface IObjectiveService
    {
        Objective GetObjectiveByObjectiveNo(int objectiveNo);

        void AddObjective(Objective objective);

        void RemoveAllObjectives();
    }
}
