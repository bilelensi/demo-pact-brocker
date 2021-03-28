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
        [Test]
        public async Task GetUserEmailTest()
        {
            //Arrange
            var date = DateTime.Now;
            var position = "";
            var userId = 2;
            var expected = "bilel.rezgui@mail.com";
            var expectedBody = new
            {
                id = userId,
                email = expected,
                firstName = "Bilel",
                lastName = "Rezgui",
                lastUpdate = Match.Type(date),
                gender = "Male",
                position = Match.Regex(position, "[(Manager)(Engineer)(Coordinator)]?")
            };
            var path = $"/api/users/{userId}";
            var pact = new ConsumerApiPact("Get user email", "Get user details Api");
            pact.SetupProviderService(path, expectedBody);
            var consumer = new UserClient(pact.MockProviderServiceBaseUri);

            //Act
            var actual = await consumer.GetUserMail(userId);

            //Assert
            Assert.AreEqual(expected, actual);

            pact.VerifyInteractions();

            Assert.IsTrue(pact.PactVerify("1.0.0", "1.0.0"));
        }

        [Test]
        public async Task GetUserPositionTest()
        {
            //Arrange
            var date = DateTime.Now;
            var userId = 2;
            var expected = "Engineer";
            var expectedBody = new
            {
                id = userId,
                email = "bilel.rezgui@mail.com",
                firstName = "Bilel",
                lastName = "Rezgui",
                lastUpdate = Match.Type(date),
                gender = "Male",
                position = expected
            };
            var path = $"/api/users/{userId}";
            var pact = new ConsumerApiPact("Get user position", "Get user details Api");
            pact.SetupProviderService(path, expectedBody);
            var consumer = new UserClient(pact.MockProviderServiceBaseUri);

            //Act
            var actual = await consumer.GetUserPosition(userId);

            //Assert
            Assert.AreEqual(expected, actual);

            pact.VerifyInteractions();

            Assert.IsTrue(pact.PactVerify("1.0.0", "1.0.0"));
        }
    }

    
}