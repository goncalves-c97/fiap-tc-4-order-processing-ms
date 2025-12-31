using Core.Interfaces.Gateways;
using Core.Interfaces;
using Core.Dtos;
using Core.Entities;
using Core.Enums;

namespace Core.Gateways
{
    public class PedidoGateway(IDbConnection dbConnection) : IPedidoGateway
    {
        private readonly IDbConnection _dbConnection = dbConnection;
        // private readonly string _tableName = nameof(Pedido);

        // TODO: public async Task<IEnumerable<Pedido>> GetAllPedidos()
        public async Task<object> GetAllPedidos()
        {
            // TODO: return await _dbConnection.ListAllAsync<Pedido>(_tableName);
            return null;
        }

        // TODO: public async Task<IEnumerable<Pedido>> GetAllPedidosByStatusPedido(StatusPedidoEnum statusPedidoEnum)
        public async Task<object> GetAllPedidosByStatusPedido(StatusPedidoEnum statusPedidoEnum)
        {
            // TODO: return await _dbConnection.SearchByParametersAsync<Pedido>(_tableName, "id_status_pedido = @Status", new { Status = (int)statusPedidoEnum });
            return null;
        }

        // TODO: public async Task<Pedido?> GetById(int idPedido)
        public async Task<object> GetById(int idPedido)
        {
            //Pedido? pedido = await _dbConnection.SearchFirstOrDefaultByParametersAsync<Pedido>(
            //    _tableName,
            //    "id_pedido = @Id",
            //    new { Id = idPedido }
            //);

            //if(pedido == null)
            //    return null;

            //pedido.IdClienteNavigation = await _dbConnection.SearchFirstOrDefaultByParametersAsync<Cliente>(
            //    "Cliente",
            //    "id_cliente = @Id",
            //    new { Id = pedido.IdCliente }
            //);

            // TODO: return pedido;
            return null;
        }
    }
}