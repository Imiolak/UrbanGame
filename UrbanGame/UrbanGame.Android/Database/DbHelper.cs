using SQLite.Net;
using SQLite.Net.Platform.XamarinAndroid;
using System;
using System.IO;
using UrbanGame.Database;
using UrbanGame.Droid.Database;
using Xamarin.Forms;

[assembly: Dependency(typeof(DbHelper))]
namespace UrbanGame.Droid.Database
{
    public class DbHelper : IDbHelper
    {
        public SQLiteConnection GetConnection(string dbFileName)
        {
            var personal = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var dbPath = Path.Combine(personal, dbFileName);

            return new SQLiteConnection(new SQLitePlatformAndroidN(), dbPath);
        }
    }
}