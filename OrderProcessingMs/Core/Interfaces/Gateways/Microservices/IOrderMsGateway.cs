using Core.Enums;

namespace Core.Interfaces.Gateways.Microservices
{
    public interface IOrderMsGateway
    {
        Task<int> IniciaPedido(string token);
        Task UpdateStatusPedido(int idPedido, StatusPedidoEnum statusPedidoEnum, string token);
    }
}
