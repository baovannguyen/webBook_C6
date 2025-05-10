using AsmC6_API.Data;
using AsmC6_API.DTOs;
using AsmC6_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
	private readonly ApplicationDbContext _context;
	private readonly TokenService _tokenService;

	public AuthController(ApplicationDbContext context, TokenService tokenService)
	{
		_context = context;
		_tokenService = tokenService;
	}

	[HttpPost("register")]
	public IActionResult Register(RegisterDto dto)
	{
		if (_context.Users.Any(x => x.Username == dto.Username))
			return BadRequest("Username already exists");

		var user = new UserModel
		{
			Username = dto.Username,
		
			Role = "Customer",
			Address = dto.Address,
			DateOfBirth= dto.DateOfBirth,
			PhoneNumber = dto.PhoneNumber,
			FullName = dto.FullName,
		};

		var hasher = new PasswordHasher<UserModel>();
		user.PasswordHash = hasher.HashPassword(user, dto.Password);

		_context.Users.Add(user);
		_context.SaveChanges();

		return Ok("User created");
	}


	[HttpPost("login")]
	public IActionResult Login(LoginDto dto)
	{
		var user = _context.Users.SingleOrDefault(x => x.Username == dto.Username);
		if (user == null) return Unauthorized("User not found");

		var hasher = new PasswordHasher<UserModel>();
		var result = hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

		if (result == PasswordVerificationResult.Failed)
			return Unauthorized("Invalid password");

		var token = _tokenService.CreateToken(user);
		return Ok(new AuthResponseDto
		{
			Token = token,
			Username = user.Username,
			Role = user.Role,
			
		});
	}
}
