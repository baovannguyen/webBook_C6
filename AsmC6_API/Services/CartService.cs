using AsmC6_API.Models;
using AsmC6_API.Data;
using Microsoft.EntityFrameworkCore;

namespace AsmC6_API.Services
{
	public class CartService : ICartService
	{
		private readonly ApplicationDbContext _context;

		public CartService(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<List<CartItemModel>> GetCartAsync(int userId)
		{
			return await _context.CartItems
				.Where(c => c.UserId == userId)
				.ToListAsync();
		}


		public async Task ClearCartAsync(int userId)
		{
			var cartItems = await _context.CartItems
				.Where(c => c.UserId == userId)
				.ToListAsync();

			_context.CartItems.RemoveRange(cartItems);
			await _context.SaveChangesAsync();
		}

	
	}
}
