using SQLite.Net;

namespace UrbanGame.Database
{
    public interface IDbHelper
    {
        SQLiteConnection GetConnection(string dbFileName);
    }
}
