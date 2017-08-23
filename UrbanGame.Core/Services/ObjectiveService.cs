using MvvmCross.Plugins.Sqlite;
using System.Collections.Generic;
using System.Linq;
using UrbanGame.Core.Models;

namespace UrbanGame.Core.Services
{
    public class ObjectiveService : DataServiceBase, IObjectiveService
    {
        public ObjectiveService(IMvxSqliteConnectionFactory connectionFactory) 
            : base(connectionFactory)
        {
            Connection.CreateTable<Objective>();
            Connection.CreateTable<ObjectiveStepBase>();
            Connection.CreateTable<TextObjectiveStep>();
        }

        public Objective GetObjectiveByObjectiveNo(int objectiveNo)
        {
            return Connection.Find<Objective>(o => o.ObjectiveNo == objectiveNo);
        }

        public T GetObjectiveStep<T>(int objectiveNo, int orderInObjective)
            where T : ObjectiveStepBase, new()
        {
            return Connection.Find<T>(os => os.ObjectiveNo == objectiveNo
                                            && os.OrderInObjective == orderInObjective);
        }

        public int GetNumberOfObjectiveStepsByObjectiveNo(int objectiveNo)
        {
            return GetObjectiveStepsForObjective(objectiveNo).Count;
        }

        public string GetObjectiveStepType(int objectiveNo, int orderInObjective)
        {
            return GetObjectiveStepsForObjective(objectiveNo)
                .Single(os => os.OrderInObjective == orderInObjective)
                .Type;
        }

        public void AddObjective(Objective objective)
        {
            Connection.Insert(objective);
        }

        public void AddObjectiveStep(ObjectiveStepBase objectiveStep)
        {
            Connection.Insert(objectiveStep, objectiveStep.GetType());
        }

        public void RemoveAllObjectives()
        {
            Connection.DeleteAll<Objective>();
            Connection.DeleteAll<TextObjectiveStep>();
            Connection.DeleteAll<ObjectiveStepBase>();
        }

        private IList<ObjectiveStepBase> GetObjectiveStepsForObjective(int objectiveNo)
        {
            var objectiveSteps = new List<ObjectiveStepBase>();
            objectiveSteps.AddRange(Connection.Table<TextObjectiveStep>()
                .Where(os => os.ObjectiveNo == objectiveNo));

            return objectiveSteps;
        }
    }
}
