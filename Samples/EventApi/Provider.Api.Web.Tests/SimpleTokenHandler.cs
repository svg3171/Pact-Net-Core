using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Authentication;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Provider.Api.Web.Tests
{
    public class SimpleTokenHandler : AuthenticationHandler<SimpleTokenOptions>
    {

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claimsIdentity = new ClaimsIdentity("SimpleTokenHandler");

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            Task<AuthenticateResult> authenticateResult;
            var token = Context.Request.Headers["Authorization"];

            if (token.Count > 0 && token.ToString() != string.Empty && token.ToString().Split(' ')[1] != "None" )
            {
                authenticateResult = Task.Run(() => AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal,
                    new AuthenticationProperties(),
                    Options.AuthenticationScheme)));
            }
            else
            {
                authenticateResult = Task.Run(() => AuthenticateResult.Fail("Authorization has been denied for this request."));
                Context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                Context.Response.Headers.Add("Content-Type", "application/json; charset=utf-8");

                var body = Encoding.ASCII.GetBytes("{\r\n\"message\": \"Authorization has been denied for this request.\"\r\n}");

                Context.Response.Body.Write(body, 0, body.Length);
                
                
            }
            return authenticateResult;
        }
    }
}
