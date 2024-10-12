using System.ComponentModel.DataAnnotations;

namespace Whiskey_TastingTale_Backend.Data.Entities
{
    public class Wish
    {
        [Key]
        public long wish_id { get; set; }
        public int user_id { get; set; }
        public int whiskey_id { get; set; }
    }
}
