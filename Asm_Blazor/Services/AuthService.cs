using Microsoft.JSInterop;

namespace Asm_Blazor.Services
{
	public class AuthService
	{
		private readonly IJSRuntime _js;
		public bool IsLoggedIn { get; private set; }
		public string Username { get; private set; } = "";

		public AuthService(IJSRuntime js)
		{
			_js = js;
		}

		public async Task CheckLoginStateAsync()
		{
			var token = await _js.InvokeAsync<string>("localStorage.getItem", "authToken");
			if (!string.IsNullOrWhiteSpace(token))
			{
				IsLoggedIn = true;
				Username = await _js.InvokeAsync<string>("localStorage.getItem", "username");
			}
			else
			{
				IsLoggedIn = false;
				Username = "";
			}
		}

		public async Task LogoutAsync()
		{
			await _js.InvokeVoidAsync("localStorage.removeItem", "authToken");
			await _js.InvokeVoidAsync("localStorage.removeItem", "username");
			IsLoggedIn = false;
			Username = "";
		}
	}
}
