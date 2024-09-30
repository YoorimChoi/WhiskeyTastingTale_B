using System.ComponentModel.DataAnnotations;

namespace Whiskey_TastingTale_Backend.Data.Entities
{
    public class Whiskey
    {
        [Key]
        public int whiskey_id { get; set; }
        public string? whiskey_name { get; set; }
        public string? img_index { get; set; }
        public double? alcohol_degree { get; set; }
        public string? maker { get; set; }
        public string? details { get; set; }
        public string? page_url { get; set; }
        public double? rating { get; set; }
        public int? review_count { get; set; }

        public DateTime created_date { get; set; }
        public DateTime updated_date { get; set; }
    }
}
