using AsmC6_API.Data;
using AsmC6_API.DTOs.cart;
using AsmC6_API.Migrations;
using AsmC6_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace AsmC6_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CartController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public CartController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: api/Cart/{userId}
		[HttpGet("{userId}")]
		public async Task<IActionResult> GetCart(string userId)
		{
			var cart = await _context.Carts
				.Include(c => c.Items)
				.ThenInclude(i => i.Book)
				.FirstOrDefaultAsync(c => c.UserId == userId);

			if (cart == null)
				return Ok(new { Items = new List<CartItemModel>(), Total = 0 });

			var total = cart.Items.Sum(x => x.Quantity * (x.Book?.Price ?? 0));
			return Ok(new { cart.Items, Total = total });
		}

		// POST: api/Cart/add
		[HttpPost("add")]
		public async Task<IActionResult> AddItem([FromBody] CartItemAddDto dto)
		{
			var cart = await _context.Carts
				.Include(c => c.Items)
				.FirstOrDefaultAsync(c => c.UserId == dto.UserId);

			if (cart == null)
			{
				cart = new CartModel { UserId = dto.UserId };
				_context.Carts.Add(cart);
				await _context.SaveChangesAsync();
			}

			var existingItem = cart.Items.FirstOrDefault(x => x.BookId == dto.BookId);
			if (existingItem != null)
			{
				existingItem.Quantity += dto.Quantity;
			}
			else
			{
				var newItem = new CartItemModel
				{
					CartId = cart.Id,
					BookId = dto.BookId,
					Quantity = dto.Quantity
				};
				_context.CartItems.Add(newItem);
			}

			await _context.SaveChangesAsync();
			return Ok(cart.Items);
		}

		// PUT: api/Cart/update
		[HttpPut("update")]
		public async Task<IActionResult> UpdateItem([FromBody] CartItemUpdateDto dto)
		{
			var item = await _context.CartItems.FindAsync(dto.ItemId);
			if (item == null)
				return NotFound();

			item.Quantity = dto.Quantity;
			await _context.SaveChangesAsync();
			return Ok(item);
		}

		// DELETE: api/Cart/remove/{itemId}
		[HttpDelete("remove/{itemId}")]
		public async Task<IActionResult> RemoveItem(int itemId)
		{
			var item = await _context.CartItems.FindAsync(itemId);
			if (item == null)
				return NotFound();

			_context.CartItems.Remove(item);
			await _context.SaveChangesAsync();
			return Ok(new { Message = "Item removed" });
		}

		// DELETE: api/Cart/clear/{userId}
		[HttpDelete("clear/{userId}")]
		public async Task<IActionResult> ClearCart(string userId)
		{
			var cart = await _context.Carts
				.Include(c => c.Items)
				.FirstOrDefaultAsync(c => c.UserId == userId);

			if (cart == null)
				return NotFound();

			_context.CartItems.RemoveRange(cart.Items);
			await _context.SaveChangesAsync();
			return Ok(new { Message = "Cart cleared" });
		}
	}


}
