using Core.Enums;
using Core.Interfaces.Gateways.Microservices;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading;

namespace Core.Gateways.Microservices
{
    public class OrderMsGateway : IOrderMsGateway
    {
        private readonly HttpClient _http;

        public OrderMsGateway(HttpClient http)
        {
            _http = http;
        }

        public async Task<int> IniciaPedido(string token)
        {
            using var request = new HttpRequestMessage(
               HttpMethod.Post,
               "Pedido/IniciaPedido"
            );

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            using var response = await _http.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync()
                         ?? throw new InvalidOperationException("Invalid response from external service");
            
            if(int.TryParse(result, out int idPedido))
                return idPedido;
            else
                throw new Exception("Falha ao obter ID do pedido criado");
        }

        public async Task UpdateStatusPedido(int idPedido, StatusPedidoEnum statusPedidoEnum, string token)
        {
            using var request = new HttpRequestMessage(
               HttpMethod.Put,
               $"Pedido/UpdateStatusPedido?idPedido={idPedido}&status={(int)statusPedidoEnum}"
            );

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            using var response = await _http.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
    }
}
