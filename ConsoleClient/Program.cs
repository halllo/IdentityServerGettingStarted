using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;

namespace ConsoleClient
{
	public class Program
	{
		const string authority = "https://localhost:44304";
		const string clientId = "console";
		const string clientSecret = "secret";
		const string scope = "api1.read api1.write";
		const string apiUrl = "https://localhost:44385";

		public static async Task Main(string[] args)
		{
			Console.Title = "Console Client";

			var token = await GetTokenAsync();
			if (token != null)
			{
				await CallApiAsync(token);
			}

			Console.ReadLine();
		}






		private static async Task<string> GetTokenAsync()
		{
			var discoveryClient = new HttpClient();
			var discoveryResponse = await discoveryClient.GetDiscoveryDocumentAsync(authority);
			if (discoveryResponse.IsError)
			{
				Console.WriteLine($"Disco error: {discoveryResponse.Error}");
				return null;
			}

			var tokenClient = new HttpClient();

			var tokenResponse = await ClientCredentialFlow(discoveryResponse, tokenClient);
			//var tokenResponse = await DeviceFlow(discoveryResponse, tokenClient);

			if (tokenResponse == null)
			{
				return null;
			}
			else if (tokenResponse.IsError)
			{
				Console.WriteLine($"Token endpoint error: {tokenResponse.Error}");
				return null;
			}

			Console.WriteLine($"Successfully obtaining an access token:\n{tokenResponse.AccessToken}\n");
			return tokenResponse.AccessToken;
		}

		private static async Task<TokenResponse> ClientCredentialFlow(DiscoveryDocumentResponse discoveryResponse, HttpClient tokenClient)
		{
			return await tokenClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
			{
				Address = discoveryResponse.TokenEndpoint,
				ClientId = clientId,
				ClientSecret = clientSecret,
				Scope = scope
			});
		}

		private static async Task<TokenResponse> DeviceFlow(DiscoveryDocumentResponse discoveryResponse, HttpClient tokenClient)
		{
			var deviceResponse = await tokenClient.RequestDeviceAuthorizationAsync(new DeviceAuthorizationRequest
			{
				Address = discoveryResponse.DeviceAuthorizationEndpoint,
				ClientId = clientId,
				ClientSecret = clientSecret,
				Scope = scope,
			});

			if (deviceResponse.IsError)
			{
				Console.WriteLine($"Device endpoint error: {deviceResponse.Error}");
				return null;
			}

			Console.WriteLine($"Please go to \"{new Uri(deviceResponse.VerificationUriComplete)}\" and authenticate. \nOnce you authenticated there, this session here will receive an access token and move on.");

			while (true)
			{
				var tokenResponse = await tokenClient.RequestDeviceTokenAsync(new DeviceTokenRequest
				{
					Address = discoveryResponse.TokenEndpoint,
					ClientId = clientId,
					ClientSecret = clientSecret,
					DeviceCode = deviceResponse.DeviceCode,
				});

				if (!tokenResponse.IsError)
				{
					return tokenResponse;
				}
				else
				{
					if (tokenResponse.Error == "authorization_pending" || tokenResponse.Error == "slow_down")
					{
						await Task.Delay(deviceResponse.Interval * 1000);
					}
					else
					{
						Console.WriteLine($"Token endpoint error: {tokenResponse.Error}");
						return null;
					}
				}
			}
		}



		private static async Task CallApiAsync(string token)
		{
			var client = new HttpClient();
			client.SetBearerToken(token);

			var response = await client.GetAsync(apiUrl + "/weatherforecast");
			if (!response.IsSuccessStatusCode)
			{
				Console.WriteLine(response.StatusCode);
				Console.WriteLine(response);
			}
			else
			{
				var content = await response.Content.ReadAsStringAsync();
				Console.WriteLine("Successfully calling API:\n" + JArray.Parse(content));
			}
		}
	}
}
