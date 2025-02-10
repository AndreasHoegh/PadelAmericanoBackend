using JwtAuthDotNet9.Entities;
using JwtAuthDotNet9.Models;
using JwtAuthDotNet9.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using JwtAuthDotNet9.Data;

namespace JwtAuthDotNet9.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TournamentController : ControllerBase
    {
        private readonly UserDbContext _context;
        public TournamentController(UserDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> CreateTournament([FromBody] TournamentDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            // Get the user from the database.
            var user = await _context.Users.FindAsync(Guid.Parse(userId));
            if (user == null) return Unauthorized();

            Tournament tournament = new Tournament
            {
                Id = Guid.NewGuid(),
                Username = user.Username,
                TournamentName = dto.TournamentName,
                ScoresJson = dto.ScoresJson,
                TournamentDate = dto.TournamentDate
            };

            _context.Tournaments.Add(tournament);
            await _context.SaveChangesAsync();

            return Ok(tournament);
        }

        [HttpGet("by-user")]
        public async Task<ActionResult<IEnumerable<Tournament>>> GetTournamentsForUser()
        {
            // Use the current user's username from claims.
            var username = User.Identity?.Name;
            if (username == null) return Unauthorized();

            var tournaments = await _context.Tournaments
                .Where(t => t.Username == username)
                .ToListAsync();

            return Ok(tournaments);
        }
    }
}
