using Core.Entities;
using Core.Enums;

namespace Core.Interfaces.Gateways.Microservices
{
    public interface IOrderMsGateway
    {
        Task<IEnumerable<Pedido>> GetAll(StatusPedidoEnum? statusPedidoEnum, string token);
        Task<int> IniciaPedido(string token);
        Task UpdateStatusPedido(int idPedido, StatusPedidoEnum statusPedidoEnum, string token);
    }
}
