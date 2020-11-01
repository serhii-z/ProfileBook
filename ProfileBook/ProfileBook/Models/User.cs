using SQLite;

namespace ProfileBook.Models
{
    [Table("Users")]
    public class User
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }

        [Unique]
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
