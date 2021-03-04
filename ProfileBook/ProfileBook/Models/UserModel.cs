using SQLite;

namespace ProfileBook.Models
{
    [Table("Users")]
    public class UserModel : IEntityBase
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Unique]
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
