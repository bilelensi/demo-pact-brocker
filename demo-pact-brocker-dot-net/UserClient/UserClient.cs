using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace userConsumer
{
    public class UserClient
    {
        private readonly HttpClient _client;

        public UserClient(string baseUri)
        {
            _client = new HttpClient { BaseAddress = new Uri(baseUri) };
        }

        public async Task<string> GetUserMail(int id)
        {
            string reasonPhrase;

            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/users/{id}");
            request.Headers.Add("Accept", "application/json");

            var response = await _client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();
            var status = response.StatusCode;

            reasonPhrase = response.ReasonPhrase;

            request.Dispose();
            response.Dispose();

            if (status == HttpStatusCode.OK)
            {
                var user = !string.IsNullOrEmpty(content) ?
                  JsonConvert.DeserializeObject<dynamic>(content)
                  : null;

                if (user != null)
                    return (string)user["email"];
                else
                    throw new Exception("user not found.");
            }

            throw new Exception(reasonPhrase);
        }
    }
}
