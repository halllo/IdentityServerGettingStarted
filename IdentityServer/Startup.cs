using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServer
{
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddIdentityServer()
				.AddInMemoryClients(Clients.Get())
				.AddInMemoryIdentityResources(Resources.GetIdentityResources())
				.AddInMemoryApiResources(Resources.GetApiResources())
				.AddInMemoryApiScopes(Resources.GetApiScopes())
				.AddTestUsers(Users.Get())
				.AddDeveloperSigningCredential()
				.AddScopeParser<FilteringScopeParser>();

			services.AddControllersWithViews();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseStaticFiles();
			app.UseRouting();

			app.UseIdentityServer();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				/*
				 * Getting basic UI (https://github.com/IdentityServer/IdentityServer4.Quickstart.UI):
				 * iex((New - Object System.Net.WebClient).DownloadString('https://raw.githubusercontent.com/IdentityServer/IdentityServer4.Quickstart.UI/main/getmain.ps1'))
				 */
				endpoints.MapDefaultControllerRoute();
			});
		}
	}
}
