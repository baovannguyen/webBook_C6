using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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
		public async Task<int?> GetUserIdAsync()
		{
			var token = await _js.InvokeAsync<string>("sessionStorage.getItem", "authToken");
			if (string.IsNullOrWhiteSpace(token))
				return null;

			var handler = new JwtSecurityTokenHandler();
			var jwtToken = handler.ReadJwtToken(token);

			var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid" || c.Type == ClaimTypes.NameIdentifier);

			if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
			{
				return userId;
			}

			return null;
		}

		public async Task CheckLoginStateAsync()
		{
			var token = await _js.InvokeAsync<string>("sessionStorage.getItem", "authToken");
			if (!string.IsNullOrWhiteSpace(token))
			{
				IsLoggedIn = true;
				Username = await _js.InvokeAsync<string>("sessionStorage.getItem", "username");
				
			}
			else
			{
				IsLoggedIn = false;
				Username = "";
			}
		}

		public async Task LogoutAsync()
		{
			await _js.InvokeVoidAsync("sessionStorage.removeItem", "authToken");
			await _js.InvokeVoidAsync("sessionStorage.removeItem", "username");
			await _js.InvokeVoidAsync("sessionStorage.removeItem", "role");
		



		}
	}
}
