using AsmC6_API.Data;
using AsmC6_API.DTOs.user;
using AsmC6_API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AsmC6_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly IMapper _mapper;

		public UserController(ApplicationDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
		{
			var users = await _context.Users.ToListAsync();
			return Ok(_mapper.Map<List<UserDto>>(users));
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<UserDto>> GetUser(int id)
		{
			var user = await _context.Users.FindAsync(id);
			if (user == null) return NotFound();

			return Ok(_mapper.Map<UserDto>(user));
		}
		[HttpPost]
		public async Task<ActionResult<UserDto>> CreateUser(UserCreateDto dto)
		{
			// Bước 1: Map DTO sang UserModel
			var user = _mapper.Map<UserModel>(dto);

			// Bước 2: Hash password
			var hasher = new PasswordHasher<UserModel>();
			user.PasswordHash = hasher.HashPassword(user, dto.Password);

			// Bước 3: Thêm vào DB
			_context.Users.Add(user);
			await _context.SaveChangesAsync();

			// Bước 4: Trả về UserDto
			var userDto = _mapper.Map<UserDto>(user);
			return CreatedAtAction(nameof(GetUser), new { id = user.Id }, userDto);
		}


		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateUser(int id, UserUpdateDto dto)
		{
			var user = await _context.Users.FindAsync(id);
			if (user == null) return NotFound();

			_mapper.Map(dto, user);
			await _context.SaveChangesAsync();

			return NoContent();
		}
		[Authorize(Roles = "Admin")]
		[HttpPut("admin/{id}")]
		public async Task<IActionResult> UpdateUserByAdmin(int id, UserUpdateAdminDto dto)
		{
			var user = await _context.Users.FindAsync(id);
			if (user == null) return NotFound();

			_mapper.Map(dto, user);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteUser(int id)
		{
			var user = await _context.Users.FindAsync(id);
			if (user == null) return NotFound();

			_context.Users.Remove(user);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		[Authorize(Roles = "Admin")]
		[HttpGet("admin-only")]
		public IActionResult GetAdminData()
		{
			return Ok("Bạn là Admin!");
		}
	}
}
