using Core.Dtos;
using Core.Entities;
using Core.Interfaces.Gateways.Microservices;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Core.Gateways.Microservices
{
    public class PaymentMsGateway : IPaymentMsGateway
    {
        private readonly HttpClient _http;

        public PaymentMsGateway(HttpClient http)
        {
            _http = http;
        }

        public async Task<QrCodePagamentoDto> CheckoutPedido(int idPedido, int valorPedido, string token)
        {
            using var request = new HttpRequestMessage(
               HttpMethod.Post,
               $"Pedido/CheckoutPedido?idPedido={idPedido}&valorPedido={valorPedido}"
            );

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token.Replace("Bearer ", string.Empty));

            using var response = await _http.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<QrCodePagamentoDto>()
                         ?? throw new InvalidOperationException("Invalid response from external service");

            return result;
        }

        public async Task<Pagamento> GetPagamentoByIdPedido(int idPedido, string token)
        {
            
            using var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"Pedido/GetById?idPedido={idPedido}"
            );

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token.Replace("Bearer ", string.Empty));

            using var response = await _http.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var pagamento = await response.Content.ReadFromJsonAsync<Pagamento>()
                               ?? throw new InvalidOperationException("Invalid response from external service");

            return pagamento;
        }
    }
}
