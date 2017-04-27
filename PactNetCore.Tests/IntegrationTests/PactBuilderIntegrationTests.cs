using PactNet.Mocks.MockHttpService;
using PactNet.TestContextInfo;
using Xunit;

namespace PactNet.Tests.IntegrationTests
{
    public class PactBuilderIntegrationTests : IClassFixture<IntegrationTestsMyApiPact>
    {
        private IMockProviderService _mockProviderService;
        private string _mockProviderServiceBaseUri;

        public PactBuilderIntegrationTests(IntegrationTestsMyApiPact data)
        {
            _mockProviderService = data.MockProviderService;
            _mockProviderServiceBaseUri = data.MockProviderServiceBaseUri;
            _mockProviderService.ClearInteractions();
        }

        [Fact]
        public void WhenNotRegisteringAnyInteractions_VerificationSucceeds()
        {
            ContextInfo.SetTestContextName(GetType().Name);

            _mockProviderService.VerifyInteractions();
        }
    }
}
