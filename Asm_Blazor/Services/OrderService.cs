using AsmC6_API.DTOs.order;
using System.Net.Http.Json;

namespace Asm_Blazor.Services
{
	public class OrderService
	{
		private readonly HttpClient _httpClient;

		public OrderService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		// Lấy danh sách đơn hàng
		public async Task<List<OrderDto>> GetOrdersAsync()
		{
			return await _httpClient.GetFromJsonAsync<List<OrderDto>>("api/order");
		}

		// Lấy chi tiết đơn hàng
		public async Task<OrderDto> GetOrderByIdAsync(int orderId)
		{
			return await _httpClient.GetFromJsonAsync<OrderDto>($"api/order/{orderId}");
		}

		// Cập nhật trạng thái đơn hàng
		public async Task<bool> UpdateOrderStatusAsync(int orderId, string newStatus)
		{
			var result = await _httpClient.PutAsJsonAsync($"api/order/{orderId}", newStatus);
			return result.IsSuccessStatusCode;
		}

		// Xóa đơn hàng
		public async Task<bool> DeleteOrderAsync(int orderId)
		{
			var result = await _httpClient.DeleteAsync($"api/order/{orderId}");
			return result.IsSuccessStatusCode;
		}
	}
}
