using Microsoft.AspNetCore.Mvc;
using WalletApi.Models.DTOs;
using WalletApi.Services;
using WalletApi.Models;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly PasswordService _passwordService;
    private readonly JwtService _jwt;

    public AuthController(AppDbContext db, PasswordService passwordService, JwtService jwt)
    {
        _db = db;
        _passwordService = passwordService;
        _jwt = jwt;
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterDto dto)
    {
        if (_db.Users.Any(u => u.Email == dto.Email))
        {
            return BadRequest(new { message = "Email already in use." });
        }

        if (_db.Users.Any(u => u.Username == dto.Username))
        {
            return BadRequest(new { message = "Username already in use." });
        }

        var user = new User
        {
            Email = dto.Email,
            Username = dto.Username,
            PasswordHash = _passwordService.HashPassword(dto.Password),
        };

        _db.Users.Add(user);
        _db.SaveChanges();

        return Ok(new { message = "User registered successfully." });

    }

    [HttpPost("login")]
    public IActionResult Login(LoginDto dto)
    {
        var user = _db.Users.FirstOrDefault(u => u.Username == dto.Username);
        if (user == null)
            return BadRequest(new { message = "Invalid username or password" });
        if (!_passwordService.VerifyPassword(user.PasswordHash, dto.Password))
            return BadRequest(new { message = "Invalid username or password" });

        var token = _jwt.GenerateToken(user.Id, user.Username);
        return Ok(new { token });
    }
    
}

