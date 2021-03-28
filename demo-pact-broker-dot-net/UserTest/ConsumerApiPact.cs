using PactNet;
using PactNet.Infrastructure.Outputters;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using System;
using System.Collections.Generic;
using System.Web;

namespace UserTest
{
    public class ConsumerApiPact
    {
        public int MockServerPort { get { return TestProperties.MockServerPort; } }

        public string MockProviderServiceBaseUri { get { return $"{TestProperties.MockProviderServiceBaseUri}:{MockServerPort}"; } }

        public string ServiceProviderBaseUri { get { return TestProperties.ServiceProviderBaseUri; } }

        public string PactBrokerHost { get { return TestProperties.PactBrokerHost; } }

        public string ServiceConsumer { get; }

        public string ServiceProvider { get; }

        private readonly IPactBuilder _pactBuilder;

        private readonly IMockProviderService _mockProviderService;

        public ConsumerApiPact(string serviceConsumer, string serviceProvider)
        {
            if (string.IsNullOrWhiteSpace(serviceConsumer))
            {
                throw new ArgumentException($"'{nameof(serviceConsumer)}' cannot be null or whitespace.", nameof(serviceConsumer));
            }

            if (string.IsNullOrWhiteSpace(serviceProvider))
            {
                throw new ArgumentException($"'{nameof(serviceProvider)}' cannot be null or whitespace.", nameof(serviceProvider));
            }

            _pactBuilder = new PactBuilder();

            _pactBuilder
              .ServiceConsumer(serviceConsumer)
              .HasPactWith(serviceProvider);

            _mockProviderService = _pactBuilder.MockService(MockServerPort); //Configure the http mock server
            ServiceConsumer = serviceConsumer;
            ServiceProvider = serviceProvider;
        }

        public void SetupProviderService(string path, dynamic expectedBody)
        {
            _mockProviderService.ClearInteractions();

            _mockProviderService
                 .Given(ServiceProvider)
                 .UponReceiving(ServiceConsumer)
                 .With(new ProviderServiceRequest
                 {
                     Method = HttpVerb.Get,
                     Path = path,
                     Headers = new Dictionary<string, object>
                     {
                        { "Accept", "application/json" }
                     }
                 })
                 .WillRespondWith(new ProviderServiceResponse
                 {
                     Status = 200,
                     Headers = new Dictionary<string, object>
                     {
                      { "Content-Type", "application/json; charset=utf-8" }
                     },
                     Body = expectedBody
                 });
        }

        public void VerifyInteractions()
        {
            _mockProviderService.VerifyInteractions();
        }

        public void Build()
        {
            _pactBuilder.Build(); // Will save the pact file once finished
        }

        public void BuildAndPublish(string version, string  tag = "master")
        {
            Build();
            var pactPublisher = new PactPublisher(PactBrokerHost);
            pactPublisher.PublishToBroker(
                $"..\\..\\..\\pacts\\{ServiceConsumer.ToLower().Replace(" ","_")}-{ServiceProvider.ToLower().Replace(" ", "_")}.json",
                version, new[] { tag });
        }

        public bool PactVerify(string ProviderVersion, string ConsumerVersion, string consumerTag = "master", string providerTag = "master")
        {
            BuildAndPublish(ConsumerVersion, consumerTag);

            var output = new NUnitOutput();

            IPactVerifier pactVerifier = new PactVerifier(new PactVerifierConfig()
            {
                ProviderVersion = ProviderVersion,
                PublishVerificationResults = true,
                Outputters = new List<IOutput>
                {
                    output
                }
            });

            pactVerifier
                .ServiceProvider(ServiceProvider, ServiceProviderBaseUri)
                .HonoursPactWith(ServiceConsumer)
                .PactUri($"..\\..\\..\\pacts\\{ServiceConsumer.ToLower().Replace(" ", "_")}-{ServiceProvider.ToLower().Replace(" ", "_")}.json")
                .Verify();

            return !output.HasError;
        }


    }
}
