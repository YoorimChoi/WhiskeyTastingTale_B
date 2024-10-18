using System.ComponentModel.DataAnnotations;

namespace Whiskey_TastingTale_Backend.Data.Entities
{
    public class WhiskeyRequest
    {
        [Key]
        public int request_id { get; set; }
        public int user_id { get; set; }
        public string? name { get; set; }
        public string? img_index { get; set; }
        public double? alcohol_degree { get; set; }
        public string? maker { get; set; }
        public string? details { get; set; }
        public bool is_completed { get; set; }
        public bool is_accepted { get; set; }
        public int? whiskey_id { get; set; }
    }
}
