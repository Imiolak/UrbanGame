using UrbanGame.Core.Models;

namespace UrbanGame.Core.Services
{
    public interface IObjectiveService
    {
        Objective GetObjectiveByObjectiveNo(int objectiveNo);
        ObjectiveStep GetObjectiveStep(int objectiveNo, int orderInObjective);
        int GetNumberOfObjectiveStepsObjectiveNo(int objectiveNo);

        void AddObjective(Objective objective);
        void AddObjectiveStep(ObjectiveStep objectiveStep);

        void RemoveAllObjectives();
    }
}
