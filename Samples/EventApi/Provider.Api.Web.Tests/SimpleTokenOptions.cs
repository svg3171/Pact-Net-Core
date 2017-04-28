using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Provider.Api.Web.Tests
{
    public class SimpleTokenOptions : AuthenticationOptions
    {
        public SimpleTokenOptions()
        {
            AuthenticationScheme = "SimpleTokenHandler";
            AutomaticAuthenticate = true;
            AutomaticChallenge = true;
           
        }
    }
}
