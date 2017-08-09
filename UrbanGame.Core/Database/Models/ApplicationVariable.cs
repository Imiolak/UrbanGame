using SQLite;

namespace UrbanGame.Core.Database.Models
{
    public class ApplicationVariable
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		[Indexed(Unique = true)]
		public string Key { get; set; }

		public string Value { get; set; }
	}
}
