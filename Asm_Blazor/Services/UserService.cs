using Asm_Blazor.Models;
using System.Net.Http.Json;

namespace Asm_Blazor.Services
{
	public class UserService
	{
		private readonly HttpClient _http;

		public UserService(HttpClient http)
		{
			_http = http;
		}
		public async Task<UserModel?> GetUserByIdAsync(string id)
		{
			return await _http.GetFromJsonAsync<UserModel>($"api/User/{id}");
		}

		public async Task<bool> CreateUserAsync(UserCreateModel user)
		{
			var response = await _http.PostAsJsonAsync("api/User", user);
			return response.IsSuccessStatusCode;
		}

		public async Task<bool> UpdateUserAsync(UserModel user)
		{
			var response = await _http.PutAsJsonAsync($"api/User/{user.Id}", user);
			return response.IsSuccessStatusCode;
		}

		public async Task<List<UserModel>> GetUsersAsync()
			=> await _http.GetFromJsonAsync<List<UserModel>>("api/User");
		

		public async Task<bool> DeleteUserAsync(int id)
		{
			var response = await _http.DeleteAsync($"api/User/{id}");
			return response.IsSuccessStatusCode;
		}
	}
}
