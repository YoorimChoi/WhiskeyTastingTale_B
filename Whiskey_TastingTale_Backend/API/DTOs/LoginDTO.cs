namespace Whiskey_TastingTale_Backend.API.DTOs
{
    public class LoginDTO
    {
        public string token {  get; set; } 
        public int user_id { get; set; }
        public string nickname { get; set; }
        public string email { get; set; }
        public string role { get; set; }
    }
}
