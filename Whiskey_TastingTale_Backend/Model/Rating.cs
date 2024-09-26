using System.ComponentModel.DataAnnotations;

namespace Whiskey_TastingTale_Backend.Model
{
    public class Rating
    {
        [Key]
        public int rating_id { get; set; }
        public int whiskey_id { get; set; }
        public double rating { get; set; }

        public DateTime created_date { get; set; }
        public DateTime updated_date { get; set; }
    }
}