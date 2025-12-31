using Core.Entities;
using Core.Enums;

namespace Core.Interfaces.Gateways
{
    public interface IComboPedidoGateway
    {
        Task AddComboOnPedido(int idCombo, int idPedido);
        Task<IEnumerable<ComboPedido>> GetAllComboPedidosByStatusPedido(StatusPedidoEnum statusPedidoEnum);
    }
}
