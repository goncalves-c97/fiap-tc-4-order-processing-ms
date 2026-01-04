using Core.Dtos;
using Core.Entities;

namespace Core.Interfaces.Gateways.Microservices
{
    public interface IPaymentMsGateway
    {
        Task<QrCodePagamentoDto> CheckoutPedido(int idPedido, int valorPedido, string token);
        Task<Pagamento> GetPagamentoByIdPedido(int idPedido, string token);
    }
}
