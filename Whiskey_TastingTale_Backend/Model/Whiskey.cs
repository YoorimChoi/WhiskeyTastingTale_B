using System.ComponentModel.DataAnnotations;

namespace Whiskey_TastingTale_Backend.Model
{
    public class Whiskey
    {
        [Key]
        public int whiskey_id {  get; set; }  

        public string? img_index { get; set; }
        public double alcohol_degree { get; set; }
        public string? maker { get; set; }
        public string? details { get; set; }
        public string? page_url { get; set; }
    }
}
