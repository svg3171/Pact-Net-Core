using PactNet;
using PactNet.Reporters.Outputters;
using PactNet.TestContextInfo;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Provider.Api.Web.Tests
{
    public class PactVerifierTest : IClassFixture<PactVerifierTestFixture<PactVerifierStartup>>
    {
        private readonly HttpClient _client;

        public PactVerifierTest(PactVerifierTestFixture<PactVerifierStartup> fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task Values_Get_Returns_Value()
        {
            // arrange
            var id = 1;

            // act
            var response = await _client.GetAsync(string.Format("/api/values/{0}", id.ToString()));

            // assert
            Assert.Equal("value", await response.Content.ReadAsStringAsync());

        }

        [Fact]
        public void EnsureEventApiHonoursPactWithConsumer_new()
        {
            ContextInfo.SetTestContextName(GetType().Name);

            //Arrange
            var outputter = new CustomOutputter();
            var config = new PactVerifierConfig();
            config.ReportOutputters.Add(outputter);


            IPactVerifier pactVerifier = new PactVerifier(() => { }, () => { }, config);

            pactVerifier
                .ProviderState(
                    "there are events with ids '45D80D13-D5A2-48D7-8353-CBB4C0EAABF5', '83F9262F-28F1-4703-AB1A-8CFD9E8249C9' and '3E83A96B-2A0C-49B1-9959-26DF23F83AEB'",
                    setUp: InsertEventsIntoDatabase)
                .ProviderState("there is an event with id '83f9262f-28f1-4703-ab1a-8cfd9e8249c9'",
                    setUp: InsertEventIntoDatabase)
                .ProviderState("there is one event with type 'DetailsView'",
                    setUp: EnsureOneDetailsViewEventExists);

            var pactUri = @"C:\Users\kharshaw\Documents\GitHub\Pact-Net-Core\Samples\EventApi\Provider.Api.Web.Tests\Consumer.Tests\pacts\consumer-event_api.json";
            
            //Act / Assert
            pactVerifier
                   .ServiceProvider("Event API", _client)
                   .HonoursPactWith("Consumer")
                   .PactUri(pactUri)
                   .Verify();

            // Verify that verifaction log is also sent to additional reporters defined in the config
            Assert.Contains("Verifying a Pact between Consumer and Event API", outputter.Output);
        }

        private void EnsureOneDetailsViewEventExists()
        {
            return;
        }

        private void InsertEventIntoDatabase()
        {
            return;
        }

        private void InsertEventsIntoDatabase()
        {
            return;
        }

        private class CustomOutputter : IReportOutputter
        {
            public string Output { get; private set; }

            public void Write(string report)
            {
                Output += report;
            }
        }
    }


}
