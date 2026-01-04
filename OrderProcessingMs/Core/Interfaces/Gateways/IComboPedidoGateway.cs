using Core.Entities;
using Core.Enums;
using Core.Interfaces.Gateways.Microservices;

namespace Core.Interfaces.Gateways
{
    public interface IComboPedidoGateway
    {
        Task AddComboOnPedido(int idCombo, int idPedido);
        Task<IEnumerable<ComboPedido>> GetAllComboPedidosByStatusPedido(IOrderMsGateway orderMsGateway, IPaymentMsGateway paymentMsGateway, ILoginMsGateway loginMsGateway, StatusPedidoEnum statusPedidoEnum, string token);
    }
}
