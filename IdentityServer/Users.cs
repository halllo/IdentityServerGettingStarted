using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Test;

namespace IdentityServer
{
	internal class Users
	{
		public static List<TestUser> Get()
		{
			return new List<TestUser> {
				new TestUser {
					SubjectId = "5BE86359-073C-434B-AD2D-A3932222DABE",
					Username = "bob",
					Password = "password",
					Claims = new List<Claim> {
						new Claim(JwtClaimTypes.Email, "bob@mail.com"),
						new Claim(JwtClaimTypes.Role, "admin")
					}
				}
			};
		}
	}
}
