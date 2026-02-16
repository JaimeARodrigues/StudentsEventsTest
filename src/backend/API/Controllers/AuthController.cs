using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EventManager.Application.DTOs;

namespace EventManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost("login")]
    public ActionResult<LoginResponse> Login([FromBody] LoginRequest request)
    {
        // Para o desafio, aceitamos qualquer usuário com email válido
        // Em produção, valide contra um banco de dados real
        if (string.IsNullOrEmpty(request.Email))
            return BadRequest("Email é obrigatório");

        var token = GenerateJwtToken(request.Email);
        
        var response = new LoginResponse
        {
            Token = token,
            User = new UserDto
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                DisplayName = request.Email.Split('@')[0]
            }
        };

        return Ok(response);
    }

    private string GenerateJwtToken(string email)
    {
        var secret = _configuration["Jwt:Secret"] ?? "your-super-secret-key-here-change-in-production";
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Email, email),
            new Claim("sub", Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: "EventManager",
            audience: "EventManagerClient",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
