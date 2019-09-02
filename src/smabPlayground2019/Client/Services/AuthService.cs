using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using smabPlayground2019.Shared;

namespace smabPlayground2019.Client.Services
{
	public class AuthService : IAuthService
	{
		private readonly HttpClient _httpClient;
		private readonly AuthenticationStateProvider _authenticationStateProvider;
		private readonly ILocalStorageService _localStorage;

		public AuthService(HttpClient httpClient,
						   AuthenticationStateProvider authenticationStateProvider,
						   ILocalStorageService localStorage)
		{
			_httpClient = httpClient;
			_authenticationStateProvider = authenticationStateProvider;
			_localStorage = localStorage;
		}

		public async Task<RegisterResult> Register(RegisterModel registerModel)
		{
			var requestJson = JsonSerializer.Serialize(registerModel, JsonSerializerOptionsProvider.Options);
			var response = await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, "api/accounts")
			{
				 Content = new StringContent(requestJson, Encoding.UTF8, "application/json")
			});

			var stringContent = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<RegisterResult>(stringContent, JsonSerializerOptionsProvider.Options);
		}

		public async Task<LoginResult> Login(LoginModel loginModel)
		{
			var requestJson = JsonSerializer.Serialize(loginModel, JsonSerializerOptionsProvider.Options);
			var response = await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, "api/login")
			{
				Content = new StringContent(requestJson, Encoding.UTF8, "application/json")
			});

			var stringContent = await response.Content.ReadAsStringAsync();
			var result = JsonSerializer.Deserialize<LoginResult>(stringContent, JsonSerializerOptionsProvider.Options);

			if (result.Successful)
			{
				await _localStorage.SetItemAsync("authToken", result.Token);
				((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(result.Token);
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);

				return result;
			}

			return result;
		}

		public async Task Logout()
		{
			await _localStorage.RemoveItemAsync("authToken");
			((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
			_httpClient.DefaultRequestHeaders.Authorization = null;
		}
	}
	internal static class JsonSerializerOptionsProvider
	{
		public static readonly JsonSerializerOptions Options = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			PropertyNameCaseInsensitive = true,
		};
	}
}
