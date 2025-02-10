namespace JwtAuthDotNet9.Models
{
    public class TournamentDto
    {
        public string TournamentName { get; set; } = string.Empty;
        public string ScoresJson { get; set; } = "{}";
        public DateTime TournamentDate { get; set; } = DateTime.UtcNow;
    }
} 