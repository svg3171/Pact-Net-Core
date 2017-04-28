using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;

namespace Provider.Api.Web.Tests
{
   

    public class PactVerifierTestFixture<TStartup> : IDisposable
    {
        private readonly TestServer _server;
        private const string SolutionName = "Provider.Api.Web.csproj";

        public HttpClient Client { get; private set; }

        public PactVerifierTestFixture() : this("Provider.Api.Web") { }

        protected PactVerifierTestFixture(string solutionRelativeTargetProjectParentDirectory)
        {
            var contentRoot = GetProjectPath(solutionRelativeTargetProjectParentDirectory);

            var builder = new WebHostBuilder()
                .UseContentRoot(contentRoot)
                .ConfigureServices(InitializeServices)
                .UseEnvironment("Development")
                .UseStartup(typeof(TStartup));

            _server = new TestServer(builder);

            Client = _server.CreateClient();
            Client.BaseAddress = new Uri("http://localhost");
        }

        private string GetProjectPath(string solutionRelativePath)
        {
            var applicationBasePath = PlatformServices.Default.Application.ApplicationBasePath;

            var directoryInfo = new DirectoryInfo(applicationBasePath);

            do
            {
                var solutionFileInfo = new FileInfo(Path.Combine(directoryInfo.FullName, solutionRelativePath, SolutionName));
                if (solutionFileInfo.Exists)
                {
                    return Path.GetFullPath(Path.Combine(directoryInfo.FullName, solutionRelativePath));
                }

                directoryInfo = directoryInfo.Parent;
            }

            while (directoryInfo.Parent != null);

            throw new Exception($"Solution root could not be location using the application root {applicationBasePath}.");
        }

        protected virtual void InitializeServices(IServiceCollection services)
        {
            var startupAssembly = typeof(TStartup).GetTypeInfo().Assembly;
            

        }
        public void Dispose()
        {
            Client.Dispose();
            _server.Dispose();
        }
    }
}
