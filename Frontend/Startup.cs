using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Frontend
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllersWithViews();

			services.AddAuthentication(options =>
			{
				options.DefaultScheme = "cookie";
				options.DefaultChallengeScheme = "oidc";
			})
				.AddCookie("cookie")
				.AddOpenIdConnect("oidc", options =>
				{
					options.Authority = this.Configuration["Authority"];
					options.ClientId = this.Configuration["ClientId"];
					options.ClientSecret = this.Configuration["ClientSecret"];
					options.ResponseType = "code";
					options.UsePkce = true;
					options.ResponseMode = "query";
					// options.CallbackPath = "/signin-oidc"; // default redirect URI
					options.Scope.Add("email");
					options.Scope.Add("api1.read");
					options.SaveTokens = true;
					options.GetClaimsFromUserInfoEndpoint = true;
					options.TokenValidationParameters = new TokenValidationParameters
					{
						NameClaimType = "email",
					};
					options.ClaimActions.MapUniqueJsonKey("email", "email");
				});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapDefaultControllerRoute();
			});
		}
	}
}
