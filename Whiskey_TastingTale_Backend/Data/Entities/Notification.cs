using System.ComponentModel.DataAnnotations;

namespace Whiskey_TastingTale_Backend.Data.Entities
{
    public class Notification
    {
        [Key]
        public int notification_id { get; set; }
        public int? user_id { get; set; }
        public string message { get; set; }
        public string notification_type { get; set; }
        public string? target_url { get; set; }              // 알림 클릭 시 이동할 URL
        public int? related_entity_id { get; set; }           // 연관된 엔티티 (nullable)
        public bool is_read { get; set; }
        public DateTime created_at { get; set; }
    }

}
