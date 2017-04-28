using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace Provider.Api.Web.Tests
{
    public class SimpleTokenProviderMiddleware : AuthenticationMiddleware<SimpleTokenOptions>
    {
        public SimpleTokenProviderMiddleware(RequestDelegate next, IOptions<SimpleTokenOptions> options, ILoggerFactory loggerFactory, UrlEncoder encoder) : base(next, options, loggerFactory, encoder)
        {
        }

        protected override AuthenticationHandler<SimpleTokenOptions> CreateHandler()
        {
            return new SimpleTokenHandler();
        }


    }
}