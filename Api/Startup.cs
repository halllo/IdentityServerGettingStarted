using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Api
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
			services.AddCors(options => options.AddPolicy("mycustomcorspolicy", b => b.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader()));

			services.AddControllers();

			services.AddAuthentication("Bearer")
				.AddIdentityServerAuthentication("Bearer", options =>
				{
					options.ApiName = "api1";
					options.Authority = this.Configuration["Authority"];
				});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();
			
			app.UseCors("mycustomcorspolicy");
			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapDefaultControllerRoute();
				endpoints.Map("/", context => context.Response.WriteAsync("API"));
			});
		}
	}
}
