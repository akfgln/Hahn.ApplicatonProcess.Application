using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Hahn.ApplicatonProcess.February2021.IntegrationTests.Helpers
{
    public class HttpClientWrapper
    {
        private readonly HttpClient client;

        public HttpClientWrapper(HttpClient client)
        {
            this.client = client;
        }

        public HttpClient Client => client;

        public async Task<T> PostAsync<T>(string url, object body)
        {
            var response = await client.PostAsync(url, new JsonContent(body));

            response.EnsureSuccessStatusCode();

            var respnoseText = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<T>(respnoseText);
            return data;
        }

        public async Task PostAsync(string url, object body)
        {
            var response = await client.PostAsync(url, new JsonContent(body));

            response.EnsureSuccessStatusCode();
        }

        public async Task<T> PutAsync<T>(string url, object body)
        {
            var response = await client.PutAsync(url, new JsonContent(body));

            response.EnsureSuccessStatusCode();

            var respnoseText = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<T>(respnoseText);
            return data;
        }
    }
}