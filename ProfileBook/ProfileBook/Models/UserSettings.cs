using SQLite;

namespace ProfileBook.Models
{
    [Table("UserSettings")]
    public class UserSettings
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }

        public string Sorting { get; set; }
        public string Theme { get; set; }
        public string Culture { get; set; }
        public int UserId { get; set; }
    }
}
