using Whiskey_TastingTale_Backend.Data.Entities;

namespace Whiskey_TastingTale_Backend.API.DTOs
{
    public class ReviewUserWhiskeyPageDTO
    {
        public List<ReviewUserWhiskeyDTO> reviews { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
        public int totalCount { get; set; }
        public int totalPages { get; set; } 
    }
}
