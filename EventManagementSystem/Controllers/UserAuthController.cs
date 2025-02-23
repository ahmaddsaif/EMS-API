using Asp.Versioning;
using EventManagementSystem.Data;
using EventManagementSystem.DTOs.UserAuth;
using EventManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EventManagementSystem.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{v:apiVersion}/userAuth")]
public class UserAuthController(AppDbContext context, IConfiguration configuration, ILogger<UserAuthController> logger) : ControllerBase
{
    private readonly AppDbContext _context = context;
    private readonly IConfiguration _configuration = configuration;
    private readonly ILogger<UserAuthController> _logger = logger;

    [HttpPost("login")]
    //[MapToApiVersion("1.0")]
    public IActionResult Login([FromBody] LoginRequestDto loginRequest)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == loginRequest.Email);

        if (user == null || user.Password != HashPassword(loginRequest.Password))
        {
            _logger.LogWarning("Failed login attempt for email: {Email}", loginRequest.Email);
            return Unauthorized(new { message = "Invalid credentials" });
        }

        if(user.MustChangePassword)
        {
            _logger.LogInformation("User {Email} must reset password before logging in.", loginRequest.Email);
            return Unauthorized(new { message = "Password change required" });
        }

        var token = GenerateJwtToken(user);

        _logger.LogInformation("User {Email} login successful", loginRequest.Email);
        return Ok(new { token });
    }

    [HttpPost("register")]
    [Authorize(Roles = "SuperAdmin")]
    //[MapToApiVersion("1.0")]
    public IActionResult Register([FromBody] RegistrationRequestDto registerRequest)
    {
        if (!Enum.IsDefined(registerRequest.Role))
        {
            return BadRequest(new { message = "Invalid role specified" });
        }

        if (_context.Users.Any(u => u.Email == registerRequest.Email))
        {
            return Conflict(new { message = "Email already registered" });
        }

        var hashedPassword = HashPassword(registerRequest.Password);

        var newUser = new User
        {
            Name = registerRequest.Name,
            Email = registerRequest.Email,
            Password = hashedPassword,
            Role = registerRequest.Role
        };

        _context.Users.Add(newUser);
        _context.SaveChanges();

        return Ok(new { message = "User registered successfully" });
    }

    [HttpPost("forgotPassword")]
    public IActionResult ForgotPassword([FromBody] RegistrationRequestDto registerRequest)
    {
        if (!Enum.IsDefined(registerRequest.Role))
        {
            return BadRequest(new { message = "Invalid role specified" });
        }

        if (_context.Users.Any(u => u.Email == registerRequest.Email))
        {
            return Conflict(new { message = "Email already registered" });
        }

        var hashedPassword = HashPassword(registerRequest.Password);

        var newUser = new User
        {
            Name = registerRequest.Name,
            Email = registerRequest.Email,
            Password = hashedPassword,
            Role = registerRequest.Role
        };

        _context.Users.Add(newUser);
        _context.SaveChanges();

        return Ok(new { message = "User registered successfully" });
    }

    [HttpPost("resetPassword")]
    [Authorize]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto request)
    {
        int userId = int.Parse(User.Claims.First(c => c.Type == "sub").Value);
        var user = _context.Users.FirstOrDefault(u => u.UserId == userId);

        if (user == null)
        {
            return BadRequest(new { message = "User not found" });
        }

        if (HashPassword(request.OldPassword) != HashPassword(user.Password))
        {
            return Conflict(new { message = "Old password is incorrect" });
        }

        user.Password = HashPassword(request.NewPassword);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Password changed successfully" });
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? "")); // TODO

        var claims = new[]
        {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string HashPassword(string password)
    {
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = SHA256.HashData(bytes);
        return Convert.ToBase64String(hash);
    }
}