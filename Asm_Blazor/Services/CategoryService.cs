using Asm_Blazor.Models;
using Asm_Blazor.Models.book;
using System.Net.Http.Json;

public class CategoryService
{
	private readonly HttpClient _http;

	public CategoryService(HttpClient http)
	{
		_http = http;
	}

	public async Task<List<CategoryModel>> GetCategoriesAsync()
	{
		return await _http.GetFromJsonAsync<List<CategoryModel>>("api/Category");
	}

	public async Task<CategoryModel?> GetCategoryByIdAsync(int id)
	{
		return await _http.GetFromJsonAsync<CategoryModel>($"api/Category/{id}");
	}



	public async Task CreateCategoryAsync(CategoryModel category)
	{
		await _http.PostAsJsonAsync("api/Category", category);
	}

	public async Task UpdateCategoryAsync(CategoryModel category)
	{
		await _http.PutAsJsonAsync($"api/Category/{category.Id}", category);
	}

	public async Task DeleteCategoryAsync(int id)
	{
		await _http.DeleteAsync($"api/Category/{id}");
	}
}
