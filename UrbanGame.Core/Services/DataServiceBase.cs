using MvvmCross.Plugins.Sqlite;
using SQLite;

namespace UrbanGame.Core.Services
{
    public class DataServiceBase
    {
        private const string ConnectionString = "UrbanGameDb.dat";
        protected readonly SQLiteConnection Connection;

        protected DataServiceBase(IMvxSqliteConnectionFactory connectionFactory)
        {
            Connection = connectionFactory.GetConnection(ConnectionString);
        }
    }
}
