using NUnit.Framework;
using PactNet;
using PactNet.Infrastructure.Outputters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserTest
{
    public class UserApiFixture
    {
        [Test]
        public void ServiceTest()
        {
            var output = new NUnitOutput();
            var config = new PactVerifierConfig
            {
                Outputters = new List<IOutput>
                {
                    output
                },
                Verbose = true,
                ProviderVersion = "1.0.0",
                PublishVerificationResults = true
            };


            IPactVerifier pactVerifier = new PactVerifier(config);
            var pactUriOptions = new PactUriOptions();

            pactVerifier
                .ServiceProvider("Get user details Api", TestProperties.ServiceProviderBaseUri)
                .PactBroker(TestProperties.PactBrokerHost, uriOptions: pactUriOptions, enablePending: true, consumerVersionTags: new List<string> { "master" }, providerVersionTags: new List<string> { "master" }, consumerVersionSelectors: new List<VersionTagSelector> { new VersionTagSelector("master") })
                .Verify();

            Assert.IsFalse(output.HasError);

        }
    }
}
