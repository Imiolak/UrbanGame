using UrbanGame.Core.Models;

namespace UrbanGame.Core.Services
{
    public interface IObjectiveService
    {
        Objective GetObjectiveByObjectiveNo(int objectiveNo);
        T GetObjectiveStep<T>(int objectiveNo, int orderInObjective) where T : ObjectiveStepBase, new();
        int GetNumberOfObjectiveStepsByObjectiveNo(int objectiveNo);
        string GetObjectiveStepType(int objectiveNo, int orderInObjective);


        void AddObjective(Objective objective);
        void AddObjectiveStep(ObjectiveStepBase objectiveStep);

        void RemoveAllObjectives();
    }
}
