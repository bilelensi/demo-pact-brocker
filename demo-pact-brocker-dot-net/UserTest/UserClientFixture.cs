using NUnit.Framework;
using PactNet;
using PactNet.Infrastructure.Outputters;
using PactNet.Matchers;
using PactNet.Mocks.MockHttpService.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using userConsumer;

namespace UserTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task GetUserEmailTest()
        { 
            using (var pact = new ConsumerApiPact("Get user email", "Get user details Api"))
            {
                var mockProviderService = pact.MockProviderService;
                mockProviderService.ClearInteractions();
                var mockProviderServiceBaseUri = pact.MockProviderServiceBaseUri;
                var date = DateTime.Now;
                var position = "";

                //Arrange
                var userId = 2;

                mockProviderService
                  .Given("Get user details Api")
                  .UponReceiving("Get user email")
                  .With(new ProviderServiceRequest
                  {
                      Method = HttpVerb.Get,
                      Path = $"/api/users/{userId}",
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
                      Body = new
                           {
                               id = userId,
                               email = "bilel.rezgui@mail.com",  
                               firstName = "Bilel",
                               lastName = "Rezgui",
                               lastUpdate = Match.Type(date),
                               gender = "Male",
                               position = Match.Regex(position, "[(Manager)(Engineer)(Coordinator)]?")
                           }
                  });

                var consumer = new UserClient(mockProviderServiceBaseUri);

                //Act
                var result = await consumer.GetUserMail(userId);

                //Assert
                Assert.IsNotNull(result);

                mockProviderService.VerifyInteractions();
            }

          //  Thread.Sleep(1000);

          var output = new NUnitOutput();

            IPactVerifier pactVerifier = new PactVerifier(new PactVerifierConfig()
            {
                ProviderVersion = "1.0.0",
                PublishVerificationResults = true,
                Outputters = new List<IOutput>
                {
                    output
                }
            });

            var pactPublisher = new PactPublisher("http://localhost:9292");
            pactPublisher.PublishToBroker(
                "..\\..\\..\\pacts\\get_user_email-get_user_details_api.json",
                "1.0.0", new[] { "master" });


              const string serviceUri = "http://localhost:5003";
              var pactUriOptions = new PactUriOptions();

            var verify = pactVerifier
                .ServiceProvider("Get user details Api", serviceUri)
                .HonoursPactWith("Get user email")
                .PactBroker("http://localhost:9292", uriOptions: pactUriOptions, enablePending: true, consumerVersionTags: new List<string> { "master" }, providerVersionTags: new List<string> { "master" }, consumerVersionSelectors: new List<VersionTagSelector> { new VersionTagSelector("master") });


            verify.Verify();

            Assert.IsFalse(output.HasError);

        }
    }

    public class NUnitOutput : IOutput
    {
        public bool HasError { get; private set; } = false;
        public void WriteLine(string line)
        {
            Console.WriteLine(line);
            if (line.Contains("FAILED"))
            {
                HasError = true;
                Console.Error.WriteLine(line);
            }
        }
    }
}