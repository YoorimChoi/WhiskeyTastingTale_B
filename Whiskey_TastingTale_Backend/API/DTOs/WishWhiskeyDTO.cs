using System.ComponentModel.DataAnnotations;

namespace Whiskey_TastingTale_Backend.API.DTOs
{
    public class WishWhiskeyDTO
    {
        [Key]
        public long wish_id { get; set; }
        public int user_id { get; set; }
        public int whiskey_id { get; set; }
        public string? whiskey_name { get; set; }
        public string? img_index { get; set; }
        public double? alcohol_degree { get; set; }
        public double? rating { get; set; }
        public int? review_count { get; set; }
    }
}
