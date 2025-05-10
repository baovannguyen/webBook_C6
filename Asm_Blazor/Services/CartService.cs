using Blazored.SessionStorage;
using Asm_Blazor.Models;

namespace Asm_Blazor.Services
{
	public class CartService
	{
		private const string CART_KEY = "cart_items";
		private readonly ISessionStorageService _sessionStorage;

		public CartService(ISessionStorageService sessionStorage)
		{
			_sessionStorage = sessionStorage;
		}

		public async Task<List<CartItemModel>> GetCartAsync()
		{
			var cart = await _sessionStorage.GetItemAsync<List<CartItemModel>>(CART_KEY);
			return cart ?? new List<CartItemModel>();
		}

		public async Task AddItemAsync(CartItemModel item)
		{
			var cart = await GetCartAsync();
			var existing = cart.FirstOrDefault(x => x.BookId == item.BookId);
			if (existing == null)
			{
				cart.Add(item);
			}
			else
			{
				existing.Quantity += item.Quantity;
			}
			await _sessionStorage.SetItemAsync(CART_KEY, cart);
		}

		public async Task RemoveItemAsync(int bookId)
		{
			var cart = await GetCartAsync();
			cart.RemoveAll(x => x.BookId == bookId);
			await _sessionStorage.SetItemAsync(CART_KEY, cart);
		}

		public async Task ClearCartAsync()
		{
			await _sessionStorage.RemoveItemAsync(CART_KEY);
		}

		public async Task IncreaseQuantityAsync(int bookId)
		{
			var cart = await GetCartAsync();
			var item = cart.FirstOrDefault(x => x.BookId == bookId);
			if (item != null)
			{
				item.Quantity++;
				await _sessionStorage.SetItemAsync(CART_KEY, cart);
			}
		}

		public async Task DecreaseQuantityAsync(int bookId)
		{
			var cart = await GetCartAsync();
			var item = cart.FirstOrDefault(x => x.BookId == bookId);
			if (item != null)
			{
				item.Quantity--;
				if (item.Quantity <= 0)
				{
					cart.Remove(item);
				}
				await _sessionStorage.SetItemAsync(CART_KEY, cart);
			}
		}
	}
}
