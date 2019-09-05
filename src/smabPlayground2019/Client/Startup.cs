using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using smabPlayground2019.Client.Services;

namespace smabPlayground2019.Client
{
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddBlazoredLocalStorage();
			services.AddAuthorizationCore();
			services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
			services.AddScoped<IAuthService, AuthService>();
		}

		public void Configure(IComponentsApplicationBuilder app)
		{
			app.AddComponent<App>("app");
		}
	}
}
