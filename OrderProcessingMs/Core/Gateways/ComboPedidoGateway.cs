using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Core.Interfaces.Gateways;
using Core.Interfaces.Gateways.Microservices;

namespace Core.Gateways
{
    public class ComboPedidoGateway : IComboPedidoGateway
    {
        private readonly IDbConnection _dbConnection;
        private const string _tableName = "Combo_pedido";

        public ComboPedidoGateway(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task AddComboOnPedido(int idCombo, int idPedido)
        {
            await _dbConnection.InsertAsync(_tableName, new Dictionary<string, object>
            {
                { "id_pedido", idPedido },
                { "id_combo", idCombo }
            });
        }

        public async Task<IEnumerable<ComboPedido>> GetAllComboPedidosByStatusPedido(IOrderMsGateway orderMsGateway, IPaymentMsGateway paymentMsGateway, ILoginMsGateway loginMsGateway, StatusPedidoEnum statusPedidoEnum, string token)
        {
            IEnumerable<Pedido> pedidos = await orderMsGateway.GetAll(statusPedidoEnum, token);

            IEnumerable<int> pedidoIds = pedidos
                .Select(x => x.IdPedido)
                .Distinct();

            IEnumerable<ComboPedido> comboPedidos = await _dbConnection.SearchByParametersAsync<ComboPedido>(
                _tableName,
                "id_pedido IN @Pedidos",
                new { Pedidos = pedidoIds }
            );

            foreach (ComboPedido comboPedido in comboPedidos)
            {
                comboPedido.IdPedidoNavigation = pedidos.FirstOrDefault(p => p.IdPedido == comboPedido.IdPedido);

                if (comboPedido.IdPedidoNavigation != null)
                {
                    comboPedido.IdPedidoNavigation.IdPagamentoNavigation = await paymentMsGateway.GetPagamentoByIdPedido(comboPedido.IdPedido, token);

                    comboPedido.IdPedidoNavigation.IdClienteNavigation = await loginMsGateway.GetClienteById(comboPedido.IdPedidoNavigation.IdCliente, token);
                }

                comboPedido.IdComboNavigation = (await _dbConnection.SearchFirstOrDefaultByParametersAsync<Combo>(
                    "Combo",
                    "id_combo = @Id",
                    new { Id = comboPedido.IdCombo }
                ))!;

                comboPedido.IdComboNavigation.IdLancheNavigation = await _dbConnection.SearchFirstOrDefaultByParametersAsync<Lanche>(
                    "Lanche",
                    "id_lanche = @Id",
                    new { Id = comboPedido.IdComboNavigation.IdLanche }
                 );

                if (comboPedido.IdComboNavigation.IdLancheNavigation != null)
                {
                    comboPedido.IdComboNavigation.AlteracoesIngredientes = (await _dbConnection.SearchByParametersAsync<AlteracaoIngredienteCombo>(
                        "Alteracao_ingrediente_combo",
                        "id_combo = @Id",
                        new { Id = comboPedido.IdCombo }
                    )).ToList();

                    foreach (AlteracaoIngredienteCombo alteracao in comboPedido.IdComboNavigation.AlteracoesIngredientes)
                    {
                        alteracao.IdIngredienteNavigation = (await _dbConnection.SearchFirstOrDefaultByParametersAsync<Ingrediente>(
                            "Ingrediente",
                            "id_ingrediente = @Id",
                            new { Id = alteracao.IdIngrediente }
                        ))!;
                    }
                }

                comboPedido.IdComboNavigation.IdAcompanhamentoNavigation = await _dbConnection.SearchFirstOrDefaultByParametersAsync<Acompanhamento>(
                    "Acompanhamento",
                    "id_acompanhamento = @Id",
                    new { Id = comboPedido.IdComboNavigation.IdAcompanhamento }
                 );

                comboPedido.IdComboNavigation.IdBebidaNavigation = await _dbConnection.SearchFirstOrDefaultByParametersAsync<Bebida>(
                    "Bebida",
                    "id_bebida = @Id",
                    new { Id = comboPedido.IdComboNavigation.IdBebida }
                 );

                comboPedido.IdComboNavigation.IdSobremesaNavigation = await _dbConnection.SearchFirstOrDefaultByParametersAsync<Sobremesa>(
                    "Sobremesa",
                    "id_sobremesa = @Id",
                    new { Id = comboPedido.IdComboNavigation.IdSobremesa }
                 );
            }

            return comboPedidos;
        }
    }
}

