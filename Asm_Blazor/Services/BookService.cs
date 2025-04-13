using System.Net.Http.Json;
using Asm_Blazor.Models.book;
namespace Asm_Blazor.Services
{
	public class BookService
	{
		private readonly HttpClient _http;

		public BookService(HttpClient http)
		{
			_http = http;
		}
	

		public async Task<List<BookModel>> GetBooksAsync()
			=> await _http.GetFromJsonAsync<List<BookModel>>("api/book");

		public async Task<BookUpdateModel> GetBookByIdAsync(int id)
			=> await _http.GetFromJsonAsync<BookUpdateModel>($"api/book/getupdate/{id}");

		public async Task CreateBookAsync(BookCreateModel book)
			=> await _http.PostAsJsonAsync("api/book", book);
		
		public async Task UpdateBookAsync(BookUpdateModel book)
			=> await _http.PutAsJsonAsync($"api/book/{book.Id}", book);

		public async Task DeleteBookAsync(int id)
			=> await _http.DeleteAsync($"api/book/{id}");
	}
}
