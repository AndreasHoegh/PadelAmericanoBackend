using JwtAuthDotNet9.Entities;
using JwtAuthDotNet9.Models;
using JwtAuthDotNet9.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JwtAuthDotNet9.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            try 
            {
                if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                {
                    return BadRequest(new { message = "Username and password are required" });
                }

                var user = await authService.RegisterAsync(request);
                if (user is null)
                {
                    return BadRequest(new { message = "Username already exists" });
                }

                return Ok(new { message = "Registration successful", userId = user.Id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Registration error: {ex}");
                return StatusCode(500, new { message = "Internal server error during registration" });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login(UserDto request)
        {
            var result = await authService.LoginAsync(request);
            if (result is null)
                return BadRequest("Invalid username or password.");

            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
        {
            var result = await authService.RefreshTokensAsync(request);
            if (result is null || result.AccessToken is null || result.RefreshToken is null)
                return Unauthorized("Invalid refresh token.");

            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        public IActionResult AuthenticatedOnlyEndpoint()
        {
            return Ok("You are authenticated!");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        public IActionResult AdminOnlyEndpoint()
        {
            return Ok("You are and admin!");
        }

        [Authorize]
        [HttpPost("increment-tournaments")]
        public async Task<ActionResult> IncrementTournamentsPlayed()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            var success = await authService.IncrementTournamentsPlayedAsync(userId);
            if (!success)
                return NotFound();

            var user = await authService.GetUserProfileAsync(userId);
            return Ok(new { tournamentsPlayed = user?.TournamentsPlayed });
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<ActionResult> GetProfile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            var user = await authService.GetUserProfileAsync(userId);
            if (user == null)
                return NotFound();

            return Ok(new { 
                username = user.Username, 
                tournamentsPlayed = user.TournamentsPlayed 
            });
        }
    }
}
