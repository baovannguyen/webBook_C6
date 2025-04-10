using AsmC6_API.Data;
using AsmC6_API.DTOs.NewFolder;
using AsmC6_API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace AsmC6_API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class BookController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly IMapper _mapper;

		public BookController(ApplicationDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
		{
			var books = await _context.Books.ToListAsync();
			return Ok(_mapper.Map<IEnumerable<BookDto>>(books));
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<BookDto>> GetBook(int id)
		{
			var book = await _context.Books.FindAsync(id);
			if (book == null)
				return NotFound();

			return Ok(_mapper.Map<BookDto>(book));
		}

		[HttpPost]
		public async Task<ActionResult<BookDto>> CreateBook(BookCreateDto bookDto)
		{
			var book = _mapper.Map<BookModel>(bookDto);
			_context.Books.Add(book);
			await _context.SaveChangesAsync();

			var result = _mapper.Map<BookDto>(book);
			return CreatedAtAction(nameof(GetBook), new { id = book.Id }, result);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateBook(int id, BookUpdateDto bookDto)
		{
			var book = await _context.Books.FindAsync(id);
			if (book == null)
				return NotFound();

			_mapper.Map(bookDto, book);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteBook(int id)
		{
			var book = await _context.Books.FindAsync(id);
			if (book == null)
				return NotFound();

			_context.Books.Remove(book);
			await _context.SaveChangesAsync();

			return NoContent();
		}
	}


}
