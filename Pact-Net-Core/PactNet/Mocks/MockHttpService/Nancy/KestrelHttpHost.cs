using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Nancy.Owin;



namespace PactNet.Mocks.MockHttpService.Nancy
{
    class KestrelHttpHost : IHttpHost
    {

        public KestrelHttpHost(Uri baseUri, string providerName, PactConfig config, bool bindOnAllAdapters)
        {
            var host = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseKestrel()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }

        public void Start()
        {
            throw new NotImplementedException();
        }


        public void Stop()
        {
            throw new NotImplementedException();
        }
    }


    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseOwin(x => x.UseNancy());
        }
    }
}
