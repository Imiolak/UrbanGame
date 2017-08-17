using MvvmCross.Plugins.Sqlite;
using UrbanGame.Core.Models;

namespace UrbanGame.Core.Services
{
    public class ObjectiveService : DataServiceBase, IObjectiveService
    {
        public ObjectiveService(IMvxSqliteConnectionFactory connectionFactory) 
            : base(connectionFactory)
        {
            Connection.CreateTable<Objective>();
        }

        public Objective GetObjectiveByObjectiveNo(int objectiveNo)
        {
            return Connection.Find<Objective>(o => o.ObjectiveNo == objectiveNo);
        }

        public void AddObjective(Objective objective)
        {
            Connection.Insert(objective);
        }

        public void RemoveAllObjectives()
        {
            Connection.DeleteAll<Objective>();
        }
    }
}
