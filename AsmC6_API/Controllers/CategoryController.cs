using AsmC6_API.Data;
using AsmC6_API.DTOs.category;
using AsmC6_API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AsmC6_API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CategoryController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly IMapper _mapper;

		public CategoryController(ApplicationDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		// GET: api/Category
		[HttpGet]
		public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
		{
			var categories = await _context.Categories.ToListAsync();
			return Ok(_mapper.Map<List<CategoryDto>>(categories));
		}

		// GET: api/Category/5
		[HttpGet("{id}")]
		public async Task<ActionResult<CategoryDto>> GetCategory(int id)
		{
			var category = await _context.Categories.FindAsync(id);
			if (category == null)
				return NotFound();

			return Ok(_mapper.Map<CategoryDto>(category));
		}

		// POST: api/Category
		[HttpPost]
		public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] CategoryDto categoryDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var category = _mapper.Map<CategoryModel>(categoryDto);
			_context.Categories.Add(category);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, _mapper.Map<CategoryDto>(category));
		}

		// PUT: api/Category/5
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDto categoryDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var category = await _context.Categories.FindAsync(id);
			if (category == null)
				return NotFound();

			_mapper.Map(categoryDto, category);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		// DELETE: api/Category/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCategory(int id)
		{
			var category = await _context.Categories.FindAsync(id);
			if (category == null)
				return NotFound();

			_context.Categories.Remove(category);
			await _context.SaveChangesAsync();

			return NoContent();
		}
	}

}
