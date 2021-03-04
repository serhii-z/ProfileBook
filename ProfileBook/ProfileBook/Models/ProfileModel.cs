using SQLite;
using System;

namespace ProfileBook.Models
{
    [Table("Profiles")]
    public class ProfileModel : IEntityBase
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string ImagePath { get; set; }
        public string NickName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationTime { get; set; }
        public int UserId { get; set; }
    }
}
