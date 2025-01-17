using Microsoft.AspNetCore.Mvc;
using SieveApp.Data;
using SieveApp.Models;
using BCrypt.Net;

namespace SieveApp.Controllers
{
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
private readonly AppDbContext _context;

public AuthController(AppDbContext context)
{
_context = context;
}

[HttpPost("register")]
public IActionResult Register([FromBody] RegisterRequest request)
{
if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
{
Console.WriteLine("Ошибка: Логин и пароль не могут быть пустыми.");
return BadRequest("Username and password are required.");
}

if (request.Username.Length < 3)
{
Console.WriteLine("Ошибка: Логин должен содержать минимум 3 символа.");
return BadRequest("Username must be at least 3 characters long.");
}

if (request.Password.Length < 6)
{
Console.WriteLine("Ошибка: Пароль должен содержать минимум 6 символов.");
return BadRequest("Password must be at least 6 characters long.");
}
if (_context.Users.Any(u => u.Username == request.Username))
{
Console.WriteLine("Ошибка: Пользователь с таким логином уже существует.");
return BadRequest("Username is already taken.");
}
var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
var newUser = new User
{
Username = request.Username,
PasswordHash = passwordHash,
Token = null // Токен не задан при регистрации
};

_context.Users.Add(newUser);
_context.SaveChanges();

Console.WriteLine("Пользователь успешно зарегистрирован.");
return Ok("User registered successfully.");
}

[HttpPost("login")]
public IActionResult Login([FromBody] LoginRequest request)
{
if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
{
Console.WriteLine("Ошибка: Логин и пароль не могут быть пустыми.");
return BadRequest("Username and password are required.");
}

var existingUser = _context.Users.FirstOrDefault(u => u.Username == request.Username);
if (existingUser == null || !BCrypt.Net.BCrypt.Verify(request.Password, existingUser.PasswordHash))
{
Console.WriteLine("Ошибка: Неверный логин или пароль.");
return BadRequest("Invalid username or password.");
}

Console.WriteLine("Пользователь успешно авторизован.");
return Ok("Login successful.");
}
}
}









