using Core.Enums;
using Core.Interfaces.Gateways.Microservices;

namespace Core.UseCases.Microservices
{
    public static class OrderMsUseCases
    {
        public static async Task<int> IniciaPedido(IOrderMsGateway orderMsGateway, string token)
        {
            if (orderMsGateway == null)
                throw new ArgumentNullException(nameof(orderMsGateway), "Order MS gateway cannot be null.");
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Token cannot be null or empty.", nameof(token));
            return await orderMsGateway.IniciaPedido(token);
        }

        public static async Task UpdateStatusPedido(IOrderMsGateway orderMsGateway, int idPedido, StatusPedidoEnum statusPedidoEnum, string token)
        {
            if(orderMsGateway == null)
                throw new ArgumentNullException(nameof(orderMsGateway), "Order MS gateway cannot be null.");
            if(idPedido <= 0)
                throw new ArgumentException("invalid idPedido", nameof(idPedido));
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Token cannot be null or empty.", nameof(token));

            await orderMsGateway.UpdateStatusPedido(idPedido, statusPedidoEnum, token);            
        }
    }
}
