using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserTest
{
    public static class TestProperties
    {
        public static int MockServerPort { get; } = TestContext.Parameters.Get("MockServerPort", 9222);
        public static string MockProviderServiceBaseUri { get; } = TestContext.Parameters.Get("MockProviderServiceBaseUri", "http://localhost");
        public static string PactBrokerHost { get; } = TestContext.Parameters.Get("PactBrokerHost", "http://localhost:9292");
        public static string ServiceProviderBaseUri { get; } = TestContext.Parameters.Get("ServiceProviderBaseUri", "http://localhost:5003");
    }
}
