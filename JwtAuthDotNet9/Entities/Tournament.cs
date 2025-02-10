namespace JwtAuthDotNet9.Entities
{
    public class Tournament
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string TournamentName { get; set; } = string.Empty;
        public string ScoresJson { get; set; } = "{}";
        public DateTime TournamentDate { get; set; } = DateTime.UtcNow;
    }
}
