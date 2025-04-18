
using AsmC6_API.Models;

namespace AsmC6_API.Services
{
	public interface ICartService
	{
		Task<List<CartItemModel>> GetCartAsync(int userId);
		Task ClearCartAsync(int userId);
	}
}
