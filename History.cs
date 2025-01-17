namespace SieveApp.Models
{
    public class History
    {
        public int Id { get; set; } 
        public int UserId { get; set; } 
        public string Data { get; set; } = string.Empty; 
        public DateTime Timestamp { get; set; } = DateTime.UtcNow; 

        public User User { get; set; } = null!;
    }
}


