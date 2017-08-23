using MvvmCross.Plugins.Sqlite;
using UrbanGame.Core.Models;

namespace UrbanGame.Core.Services
{
    public class ObjectiveService : DataServiceBase, IObjectiveService
    {
        public ObjectiveService(IMvxSqliteConnectionFactory connectionFactory) 
            : base(connectionFactory)
        {
            Connection.CreateTable<ObjectiveStep>();
            Connection.CreateTable<Objective>();
        }

        public Objective GetObjectiveByObjectiveNo(int objectiveNo)
        {
            return Connection.Find<Objective>(o => o.ObjectiveNo == objectiveNo);
        }

        public ObjectiveStep GetObjectiveStep(int objectiveNo, int orderInObjective)
        {
            return Connection.Find<ObjectiveStep>(os => os.ObjectiveNo == objectiveNo
                                                        && os.OrderInObjective == orderInObjective);
        }

        public int GetNumberOfObjectiveStepsObjectiveNo(int objectiveNo)
        {
            return Connection.Table<ObjectiveStep>()
                .Count(o => o.ObjectiveNo == objectiveNo);
        }

        public void AddObjective(Objective objective)
        {
            Connection.Insert(objective);
        }

        public void AddObjectiveStep(ObjectiveStep objectiveStep)
        {
            Connection.Insert(objectiveStep);
        }

        public void RemoveAllObjectives()
        {
            Connection.DeleteAll<Objective>();
            Connection.DeleteAll<ObjectiveStep>();
        }
    }
}
