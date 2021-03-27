using PactNet;
using PactNet.Mocks.MockHttpService;
using System;

namespace UserTest
{
    public class ConsumerApiPact : IDisposable
    {
        public IPactBuilder PactBuilder { get; private set; }
        public IMockProviderService MockProviderService { get; private set; }

        public static int MockServerPort { get { return 9222; } }
        public string MockProviderServiceBaseUri { get { return $"http://localhost:{MockServerPort}"; } }

        public ConsumerApiPact(string serviceConsumer, string serviceProvider)
        {
            PactBuilder = new PactBuilder();

            PactBuilder
              .ServiceConsumer(serviceConsumer)
              .HasPactWith(serviceProvider);

            MockProviderService = PactBuilder.MockService(MockServerPort); //Configure the http mock server
        }

        public void Dispose()
        {
            PactBuilder.Build(); //NOTE: Will save the pact file once finished
        }
    }
}
