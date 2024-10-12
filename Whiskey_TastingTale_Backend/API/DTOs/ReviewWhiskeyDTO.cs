using System.ComponentModel.DataAnnotations;

namespace Whiskey_TastingTale_Backend.API.DTOs
{
    public class ReviewWhiskeyDTO
    {
        [Key]
        public long review_id { get; set; }
        public int user_id { get; set; }
        public int whiskey_id { get; set; }
        public string? whiskey_name { get; set; }
        public string? whiskey_img_index { get; set; }
        public int rating { get; set; }
        public string? review_text { get; set; }
        public DateTime created_date { get; set; }
        public DateTime updated_date { get; set; }
    }
}
