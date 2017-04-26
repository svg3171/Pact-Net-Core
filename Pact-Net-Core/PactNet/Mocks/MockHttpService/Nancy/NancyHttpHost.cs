using System;
using Nancy.Bootstrapper;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using PactNet.Extensions;
using PactNet.Logging;

namespace PactNet.Mocks.MockHttpService.Nancy
{
    internal class NancyHttpHost : IHttpHost
    {
        private readonly Uri _baseUri;
        private readonly INancyBootstrapper _bootstrapper;
        private readonly ILog _log;
        private readonly PactConfig _config;
        //private readonly HostConfiguration _nancyConfiguration;
        private IWebHost _host;

        internal NancyHttpHost(Uri baseUri, string providerName, PactConfig config, INancyBootstrapper bootstrapper) :
            this(baseUri, providerName, config, false)
        {
            _bootstrapper = bootstrapper;
        }

        internal NancyHttpHost(Uri baseUri, string providerName, PactConfig config, bool bindOnAllAdapters)
        {
            var loggerName = LogProvider.CurrentLogProvider.AddLogger(config.LogDir, providerName.ToLowerSnakeCase(), "{0}_mock_service.log");
            config.LoggerName = loggerName;

            _baseUri = baseUri;
            _bootstrapper = new MockProviderNancyBootstrapper(config);
            _log = LogProvider.GetLogger(config.LoggerName);
            _config = config;

            //_nancyConfiguration = new HostConfiguration
            //{
            //    AllowChunkedEncoding = false
            //};

            if (bindOnAllAdapters)
            {
                //_nancyConfiguration.UrlReservations = new UrlReservations
                //{
                //    CreateAutomatically = true
                //};
                //_nancyConfiguration.RewriteLocalhost = true;
            }
            else
            {
                //_nancyConfiguration.RewriteLocalhost = false;
            }
        }

        public void Start()
        {
            Stop();
            try
            {
                // todo (HACK): Find a better way to inject pact config into MockProviderNancyBootstrapper
                MockProviderNancyBootstrapper.PactConfig = _config;

                _host = new WebHostBuilder()
                    .UseUrls($"{_baseUri}")
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseKestrel()
                    .UseStartup<Startup4ConsumerRun>()
                    .Build();

                _host.Start();
                //_host.Run();  //Use this instead of Start() in order to interact via PostMan etc. (it blocks)
            }
            catch (Exception ex)
            {
                ////An existing binding is present, flip to binding on all adapters
                ////This code exists to sociably handle changing the default binding mode to not require admin privileges
                //_nancyConfiguration.UrlReservations = new UrlReservations
                //{
                //    CreateAutomatically = true
                //};
                //_nancyConfiguration.RewriteLocalhost = true;
                //_host = new NancyHost(_bootstrapper, _nancyConfiguration, _baseUri);
                //_host.Start();
                _log.Error(ex.ToString());
            }

            _log.InfoFormat("Started {0}", _baseUri.OriginalString);
        }

        public void Stop()
        {
            if (_host != null)
            {
                //_host.Stop();
                Dispose(_host);
                _host = null;
                _log.InfoFormat("Stopped {0}", _baseUri.OriginalString);

                LogProvider.CurrentLogProvider.RemoveLogger(_config.LoggerName);
            }
        }

        private void Dispose(IDisposable disposable)
        {
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
    }
}