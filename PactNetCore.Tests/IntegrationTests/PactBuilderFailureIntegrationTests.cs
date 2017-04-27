﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using PactNet.TestContextInfo;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace PactNet.Tests.IntegrationTests
{
    public class PactBuilderFailureIntegrationTests : IClassFixture<FailureIntegrationTestsMyApiPact>
    {
        private IMockProviderService _mockProviderService;
        private string _mockProviderServiceBaseUri;

        public PactBuilderFailureIntegrationTests(FailureIntegrationTestsMyApiPact data)
        {
            _mockProviderService = data.MockProviderService;
            _mockProviderServiceBaseUri = data.MockProviderServiceBaseUri;
            _mockProviderService.ClearInteractions();
        }

        [Fact]
        public void WhenRegisteringTheSameInteractionTwiceInATest_ThenPactFailureExceptionIsThrown()
        {
            ContextInfo.SetTestContextName(GetType().Name);

            var description = "A POST request to create a new thing";
            var request = new ProviderServiceRequest
            {
                Method = HttpVerb.Post,
                Path = "/things",
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json; charset=utf-8" }
                },
                Body = new
                {
                    thingId = 1234,
                    type = "Awesome"
                }
            };

            var response = new ProviderServiceResponse
            {
                Status = 201
            };

            _mockProviderService
                .UponReceiving(description)
                .With(request)
                .WillRespondWith(response);

            _mockProviderService
                .UponReceiving(description)
                .With(request);
                
            Assert.Throws<PactFailureException>(() => _mockProviderService.WillRespondWith(response));
        }

        [Fact]
        public void WhenRegisteringAnInteractionThatIsNeverSent_ThenPactFailureExceptionIsThrown()
        {
            ContextInfo.SetTestContextName(GetType().Name);

            _mockProviderService
                .UponReceiving("A POST request to create a new thing")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Post,
                    Path = "/things",
                    Headers = new Dictionary<string, string>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    },
                    Body = new
                    {
                        thingId = 1234,
                        type = "Awesome"
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 201
                });

            Assert.Throws<PactFailureException>(() => _mockProviderService.VerifyInteractions());
        }

        [Fact]
        public void WhenRegisteringAnInteractionThatIsSentMultipleTimes_ThenPactFailureExceptionIsThrown()
        {
            ContextInfo.SetTestContextName(GetType().Name);

            _mockProviderService
                .UponReceiving("A GET request to retrieve a thing")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/things/1234"
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200
                });

            var httpClient = new HttpClient {BaseAddress = new Uri(_mockProviderServiceBaseUri)};

            var request1 = new HttpRequestMessage(HttpMethod.Get, "/things/1234");
            var request2 = new HttpRequestMessage(HttpMethod.Get, "/things/1234");

            var response1 = httpClient.SendAsync(request1).Result;
            var response2 = httpClient.SendAsync(request2).Result;

            if (response1.StatusCode != HttpStatusCode.OK || response2.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(String.Format("Wrong status code '{0} and {1}' was returned", response1.StatusCode, response2.StatusCode));
            }

            Assert.Throws<PactFailureException>(() => _mockProviderService.VerifyInteractions());
        }

        [Fact]
        public void WhenRegisteringAnInteractionWhereTheRequestDoesNotExactlyMatchTheActualRequest_ThenStatusCodeReturnedIs500AndPactFailureExceptionIsThrown()
        {
            ContextInfo.SetTestContextName(GetType().Name);

            _mockProviderService
                .UponReceiving("A GET request to retrieve things by type")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/things",
                    Query = "type=awesome",
                    Headers = new Dictionary<string, string>
                    {
                        { "Accept", "application/json; charset=utf-8" }
                    },
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200
                });

            var httpClient = new HttpClient { BaseAddress = new Uri(_mockProviderServiceBaseUri) };

            var request = new HttpRequestMessage(HttpMethod.Get, "/things?type=awesome");

            var response = httpClient.SendAsync(request).Result;

            if (response.StatusCode != HttpStatusCode.InternalServerError)
            {
                throw new Exception(String.Format("Wrong status code '{0}' was returned", response.StatusCode));
            }

            Assert.Throws<PactFailureException>(() => _mockProviderService.VerifyInteractions());
        }
    }
}
