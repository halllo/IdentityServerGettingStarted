using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Frontend.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Frontend.Controllers
{
	public class HomeController : Controller
	{
		private readonly IConfiguration config;
		private readonly ILogger<HomeController> _logger;

		public HomeController(IConfiguration config, ILogger<HomeController> logger)
		{
			this.config = config;
			_logger = logger;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[Authorize]
		public async Task<IActionResult> Weather()
		{
			var token = await HttpContext.GetTokenAsync("access_token");

			using (var http = new HttpClient())
			{
				http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

				//https://localhost:44304/connect/userinfo

				var response = await http.GetAsync(new Uri(this.config["ApiUrl"]) + "weatherforecast");
				if (response.IsSuccessStatusCode)
				{
					var responseContent = await response.Content.ReadAsStringAsync();
					var days = JsonSerializer.Deserialize<WeatherViewModel.Day[]>(responseContent, new JsonSerializerOptions
					{
						PropertyNameCaseInsensitive = true
					});
					return View(new WeatherViewModel { Days = days.ToList() });
				}
				else
				{
					return RedirectToAction(nameof(Error));
				}
			}
		}

		public class WeatherViewModel
		{
			public List<Day> Days { get; set; }
			public class Day
			{
				public DateTime Date { get; set; }
				public int TemperatureC { get; set; }
				public string Summary { get; set; }
			}
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
