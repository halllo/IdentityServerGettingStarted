using System.Collections.Generic;
using System.Linq;
using IdentityServer4.Extensions;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace IdentityServer
{
	public class FilteringScopeParser : IScopeParser
	{
		private readonly DefaultScopeParser defaultScopeParser;
		private readonly IHttpContextAccessor httpContextAccessor;

		public FilteringScopeParser(ILogger<DefaultScopeParser> logger, IHttpContextAccessor httpContextAccessor)
		{
			this.defaultScopeParser = new DefaultScopeParser(logger);
			this.httpContextAccessor = httpContextAccessor;
		}

		public ParsedScopesResult ParseScopeValues(IEnumerable<string> scopeValues)
		{
			var user = this.httpContextAccessor.HttpContext.User;
			if (user.IsAuthenticated())
			{
				if (user.GetSubjectId() == "5BE86359-073C-434B-AD2D-A3932222DABE")
				{
					scopeValues = scopeValues.ToList();
				}
				else
				{
					scopeValues = scopeValues.Where(s => !s.Contains("write"));
				}
			}

			return defaultScopeParser.ParseScopeValues(scopeValues);
		}
	}
}
