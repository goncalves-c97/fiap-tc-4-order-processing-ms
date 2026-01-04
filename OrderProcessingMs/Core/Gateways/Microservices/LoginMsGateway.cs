using Core.Entities;
using Core.Interfaces.Gateways.Microservices;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Core.Gateways.Microservices
{
    public class LoginMsGateway : ILoginMsGateway
    {
        private readonly HttpClient _http;

        public LoginMsGateway(HttpClient http)
        {
            _http = http;
        }
        public async Task<Cliente> GetClienteById(int idCliente, string token)
        {
            using var request = new HttpRequestMessage(
               HttpMethod.Get,
               $"Cliente/GetById/{idCliente}"
            );

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            using var response = await _http.SendAsync(request);

            string content = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync()
                         ?? throw new InvalidOperationException("Invalid response from external service");

            return JsonConvert.DeserializeObject<Cliente>(result)
                   ?? throw new InvalidOperationException("Failed to deserialize cliente object");
        }
    }
}
