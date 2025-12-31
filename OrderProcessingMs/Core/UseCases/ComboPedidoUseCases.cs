using Core.Entities;
using Core.Enums;
using Core.Gateways;
using Core.Interfaces.Gateways;

namespace Core.UseCases
{
    public static class ComboPedidoUseCases
    {
        public static async Task AddComboOnPedido(IComboPedidoGateway comboPedidoGateway, int idCombo, int idPedido)
        {
            if (idCombo <= 0)
                throw new ArgumentException($"'{nameof(idCombo)}' deve ser maior que zero");

            if (idPedido <= 0)
                throw new ArgumentException($"'{nameof(idPedido)}' deve ser maior que zero");

            await comboPedidoGateway.AddComboOnPedido(idCombo, idPedido);
        }

        public static async Task<IEnumerable<ComboPedido>> GetAllComboPedidosByStatusPedido(IComboPedidoGateway comboPedidoGateway, StatusPedidoEnum statusPedidoEnum)
        {
            return await comboPedidoGateway.GetAllComboPedidosByStatusPedido(statusPedidoEnum);
        }
    }
}
