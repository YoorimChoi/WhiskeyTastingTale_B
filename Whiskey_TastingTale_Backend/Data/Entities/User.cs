using System.ComponentModel.DataAnnotations;

namespace Whiskey_TastingTale_Backend.Data.Entities
{
    public class User
    {
        [Key]
        public int user_id { get; set; }
        public string? nickname { get; set; }
        public string? email { get; set; }
        public string? password_hash { get; set; }
        public string? salt { get; set; }
        public string? role { get; set; }
        public bool? is_active { get; set; }
    }
}
