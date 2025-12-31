using Core.Dtos;

namespace Core.Interfaces.Gateways.Microservices
{
    public interface IPaymentMsGateway
    {
        Task<QrCodePagamentoDto> CheckoutPedido(int idPedido, int valorPedido, string token);
    }
}
