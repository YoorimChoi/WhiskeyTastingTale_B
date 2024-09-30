using System.ComponentModel.DataAnnotations;

namespace Whiskey_TastingTale_Backend.Data.Entities
{
    public class User
    {
        [Key]
        public int user_id { get; set; }
        public required string nickname { get; set; }
        public required string email { get; set; }
        public required string password_hash { get; set; }
        public required string salt { get; set; }
        public required string role { get; set; }
        public bool is_active { get; set; }
    }
}
