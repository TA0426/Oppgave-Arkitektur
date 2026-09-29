using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace OppgaveUkeEnModul3.WebApi.Controllers;



[ApiController]
[Route("auth")]
public class AuthController(IConfiguration configuration) : ControllerBase
{

    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        if (request.Username != "player" || request.Password != "password123")
        {
            return Unauthorized("Feil brukernavn eller passord.");
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, request.Username),
            new Claim(ClaimTypes.NameIdentifier, "player-1")
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new LoginResponse(tokenString));
    }
}

public record LoginRequest(string Username, string Password);

public record LoginResponse(string Token);