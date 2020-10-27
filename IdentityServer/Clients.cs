using System;
using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace IdentityServer
{
	internal class Clients
	{
		public static IEnumerable<Client> Get()
		{
			return new List<Client>
			{
				new Client
				{
					ClientId = "frontend",
					ClientName = "Frontend (ASP.NET MVC)",
					ClientSecrets = new List<Secret> {new Secret("secret".Sha256())},

					AllowedGrantTypes = GrantTypes.Code,
					RedirectUris = new List<string> {"https://localhost:44336/signin-oidc"},
					AllowedScopes = new List<string>
					{
						IdentityServerConstants.StandardScopes.OpenId,
						IdentityServerConstants.StandardScopes.Profile,
						IdentityServerConstants.StandardScopes.Email,
						"api1.read"
					},

					RequirePkce = true,
					AllowPlainTextPkce = false,
				},
				new Client
				{
					ClientId = "frontend-angular",
					ClientName = "Frontend (Angular)",

					AllowedGrantTypes = GrantTypes.Code,
					RedirectUris = new List<string> {
						"http://localhost:4200/",
						"http://localhost:4200/assets/pages/silent-token-refresh.html"
					},
					PostLogoutRedirectUris = new List<string> { "http://localhost:4200/" },
					AllowedCorsOrigins = new List<string> { "http://localhost:4200" },
					AllowedScopes = new List<string>
					{
						IdentityServerConstants.StandardScopes.OpenId,
						IdentityServerConstants.StandardScopes.Profile,
						IdentityServerConstants.StandardScopes.Email,
						"api1.read"
					},

					RequirePkce = true,
					AllowPlainTextPkce = false,
					RequireClientSecret = false,
					AccessTokenLifetime = (int)TimeSpan.FromMinutes(2).TotalSeconds,
				}
	};
		}
	}
}
