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
					ClientId = "oidcClient",
					ClientName = "Example Client Application",
					ClientSecrets = new List<Secret> {new Secret("secret".Sha256())},

					AllowedGrantTypes = GrantTypes.Code,
					RedirectUris = new List<string> {"https://localhost:44336/signin-oidc"},
					AllowedScopes = new List<string>
					{
						IdentityServerConstants.StandardScopes.OpenId,
						IdentityServerConstants.StandardScopes.Profile,
						IdentityServerConstants.StandardScopes.Email,
						"role",
						"api1.read"
					},

					RequirePkce = true,
					AllowPlainTextPkce = false,
				}
			};
		}
	}
}
