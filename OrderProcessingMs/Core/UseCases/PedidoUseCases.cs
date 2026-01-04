using Core.Dtos;
using Core.Entities;
using Core.Enums;
using Core.Gateways;
using Core.Gateways.Microservices;
using Core.Interfaces.Gateways;
using Core.Interfaces.Gateways.Microservices;
using Core.UseCases.Microservices;

namespace Core.UseCases
{
    public static class PedidoUseCases
    {
        //public static async Task<IEnumerable<Pedido>> GetAllPedidos(IPedidoGateway pedidoGateway, StatusPedidoEnum? status = null)
        //{
        //    if (pedidoGateway == null)
        //        throw new ArgumentNullException(nameof(pedidoGateway), "Pedido gateway cannot be null.");

        //    if (status.HasValue)
        //        return await pedidoGateway.GetAllPedidosByStatusPedido(status.Value);
        //    else
        //        return await pedidoGateway.GetAllPedidos();
        //}
        //public static async Task<Pedido> GetPedidoById(IPedidoGateway pedidoGateway, int idPedido)
        //{
        //    if (pedidoGateway == null)
        //        throw new ArgumentNullException(nameof(pedidoGateway), "Pedido gateway cannot be null.");

        //    return await pedidoGateway.GetById(idPedido) ?? throw new KeyNotFoundException($"Pedido with ID {idPedido} not found.");
        //}

        //public static async Task<Pedido> CreatePedido(IPedidoGateway pedidoGateway, int idCliente)
        //{
        //    if (pedidoGateway == null)
        //        throw new ArgumentNullException(nameof(pedidoGateway), "Pedido gateway cannot be null.");

        //    Pedido pedido = new(idCliente);

        //    return await pedidoGateway.InsertPedido(pedido);
        //}

        public static async Task<List<Combo>> RegisterCombosOnPedido(IComboGateway comboGateway, IComboPedidoGateway comboPedidoGateway, IAlteracaoIngredienteComboGateway alteracaoIngredienteComboGateway, int idPedido, List<ComboDto> combosDto)
        {
            List<Combo> fullCombos = [];

            foreach (ComboDto comboDto in combosDto)
            {
                // Insere o combo
                Combo combo = await ComboUseCases.Insert(comboGateway, comboDto);

                // Vincula o combo no pedido
                await ComboPedidoUseCases.AddComboOnPedido(comboPedidoGateway, combo.IdCombo, idPedido);

                // Se houver alterações de ingrediente, atualiza no gateway
                if (comboDto.HasAlteracoesIngrediente)
                    await AlteracaoIngredienteComboUseCases.UpdateAlteracoesIngredienteOnCombo(alteracaoIngredienteComboGateway, combo.IdCombo, comboDto.AlteracoesIngrediente!);

                // Busca o combo completo, com todos os detalhes
                combo = await ComboUseCases.GetCompleteComboById(comboGateway, combo.IdCombo);

                // Adiciona o combo completo à lista
                fullCombos.Add(combo);
            }

            return fullCombos;
        }

        public static async Task UpdatePedidoStatusToEmPreparacao(IOrderMsGateway orderMsGateway, int idPedido, string token)
        {
            await OrderMsUseCases.UpdateStatusPedido(orderMsGateway, idPedido, StatusPedidoEnum.EmPreparacao, token);
        }

        public static async Task UpdatePedidoStatusToRecebido(IOrderMsGateway orderMsGateway, int idPedido, string token)
        {
            await OrderMsUseCases.UpdateStatusPedido(orderMsGateway, idPedido, StatusPedidoEnum.Recebido, token);
        }

        public static async Task UpdatePedidoStatusToPronto(IOrderMsGateway orderMsGateway, int idPedido, string token)
        {
            await OrderMsUseCases.UpdateStatusPedido(orderMsGateway, idPedido, StatusPedidoEnum.Pronto, token);
        }

        public static async Task UpdatePedidoStatusToFinalizado(IOrderMsGateway orderMsGateway, int idPedido, string token)
        {
            await OrderMsUseCases.UpdateStatusPedido(orderMsGateway, idPedido, StatusPedidoEnum.Finalizado, token);
        }

        public static async Task<Dictionary<string, List<object>>> GetPedidosTelaoPedidos(IPedidoGateway pedidoGateway, IComboPedidoGateway comboPedidoGateway, IOrderMsGateway orderMsGateway, IPaymentMsGateway paymentMsGateway, ILoginMsGateway loginMsGateway, string token)
        {
            List<ComboPedido> comboPedidos = [];

            Dictionary<string, List<object>> itens = new()
            {
                { StatusPedidoEnum.Recebido.ToString(), new List<object>() },
                { StatusPedidoEnum.EmPreparacao.ToString(), new List<object>() },
                { StatusPedidoEnum.Pronto.ToString(), new List<object>() },
                { StatusPedidoEnum.Finalizado.ToString(), new List<object>() },
            };

            comboPedidos.AddRange(await comboPedidoGateway.GetAllComboPedidosByStatusPedido(orderMsGateway, paymentMsGateway, loginMsGateway, StatusPedidoEnum.Recebido, token));
            comboPedidos.AddRange(await comboPedidoGateway.GetAllComboPedidosByStatusPedido(orderMsGateway, paymentMsGateway, loginMsGateway, StatusPedidoEnum.EmPreparacao, token));
            comboPedidos.AddRange(await comboPedidoGateway.GetAllComboPedidosByStatusPedido(orderMsGateway, paymentMsGateway, loginMsGateway, StatusPedidoEnum.Pronto, token));
            comboPedidos.AddRange(await comboPedidoGateway.GetAllComboPedidosByStatusPedido(orderMsGateway, paymentMsGateway, loginMsGateway, StatusPedidoEnum.Finalizado, token));

            foreach (ComboPedido comboPedido in comboPedidos)
            {
                string dictKey = ((StatusPedidoEnum)comboPedido.IdPedidoNavigation.IdStatusPedido!).ToString();
                string identificador = comboPedido.IdPedidoNavigation.IdClienteNavigation.Nome ?? comboPedido.IdPedido.ToString();
                itens[dictKey].Add(new { identificador });
            }

            return itens;
        }

        public static async Task<Dictionary<string, List<object>>> GetPedidosPainelCozinha(IOrderMsGateway orderMsGateway, IPaymentMsGateway paymentMsGateway, ILoginMsGateway loginMsGateway, IPedidoGateway pedidoGateway, IComboPedidoGateway comboPedidoGateway, string token)
        {
            List<ComboPedido> comboPedidos = [];

            Dictionary<string, List<object>> itens = new Dictionary<string, List<object>>()
            {
                { StatusPedidoEnum.Recebido.ToString(), new List<object>() },
                { StatusPedidoEnum.EmPreparacao.ToString(), new List<object>() },
            };

            comboPedidos.AddRange(await comboPedidoGateway.GetAllComboPedidosByStatusPedido(orderMsGateway, paymentMsGateway, loginMsGateway, StatusPedidoEnum.Recebido, token));
            comboPedidos.AddRange(await comboPedidoGateway.GetAllComboPedidosByStatusPedido(orderMsGateway, paymentMsGateway, loginMsGateway, StatusPedidoEnum.EmPreparacao, token));

            foreach (ComboPedido comboPedido in comboPedidos)
            {
                string dictKey = ((StatusPedidoEnum)comboPedido.IdPedidoNavigation.IdStatusPedido!).ToString();

                string lanche = comboPedido.IdComboNavigation.IdLancheNavigation != null ? comboPedido.IdComboNavigation.IdLancheNavigation.Nome : "n/a";
                string acompanhamento = comboPedido.IdComboNavigation.IdAcompanhamentoNavigation != null ? comboPedido.IdComboNavigation.IdAcompanhamentoNavigation.Nome : "n/a";
                string bebida = comboPedido.IdComboNavigation.IdBebidaNavigation != null ? comboPedido.IdComboNavigation.IdBebidaNavigation.Nome : "n/a";
                string sobremesa = comboPedido.IdComboNavigation.IdSobremesaNavigation != null ? comboPedido.IdComboNavigation.IdSobremesaNavigation.Nome : "n/a";

                List<string> alteracoesIngrediente = [];

                if (comboPedido.IdComboNavigation.AlteracoesIngredientes.Count != 0)
                {
                    foreach (AlteracaoIngredienteCombo alteracao in comboPedido.IdComboNavigation.AlteracoesIngredientes)
                    {
                        alteracoesIngrediente.Add($"*** {alteracao.Quantidade}x - {alteracao.IdIngredienteNavigation.Nome}");
                    }
                }

                itens[dictKey].Add(new
                {
                    idPedido = comboPedido.IdPedido,
                    lanche,
                    acompanhamento,
                    bebida,
                    sobremesa,
                    alteracoesIngrediente
                });
            }

            return itens;
        }

        public static async Task<Dictionary<string, List<object>>> GetPedidosPainelAtendente(IPedidoGateway pedidoGateway, IComboPedidoGateway comboPedidoGateway, IOrderMsGateway orderMsGateway, IPaymentMsGateway paymentMsGateway, ILoginMsGateway loginMsGateway, string token)
        {
            List<ComboPedido> comboPedidos = [];

            Dictionary<string, List<object>> itens = new Dictionary<string, List<object>>()
            {
                { StatusPedidoEnum.EmPreparacao.ToString(), new List<object>() },
                { StatusPedidoEnum.Pronto.ToString(), new List<object>() },
            };

            comboPedidos.AddRange(await comboPedidoGateway.GetAllComboPedidosByStatusPedido(orderMsGateway, paymentMsGateway, loginMsGateway, StatusPedidoEnum.EmPreparacao, token));
            comboPedidos.AddRange(await comboPedidoGateway.GetAllComboPedidosByStatusPedido(orderMsGateway, paymentMsGateway, loginMsGateway, StatusPedidoEnum.Pronto, token));

            foreach (ComboPedido comboPedido in comboPedidos)
            {
                string dictKey = ((StatusPedidoEnum)comboPedido.IdPedidoNavigation.IdStatusPedido!).ToString();

                string lanche = comboPedido.IdComboNavigation.IdLancheNavigation != null ? comboPedido.IdComboNavigation.IdLancheNavigation.Nome : "n/a";
                string acompanhamento = comboPedido.IdComboNavigation.IdAcompanhamentoNavigation != null ? comboPedido.IdComboNavigation.IdAcompanhamentoNavigation.Nome : "n/a";
                string bebida = comboPedido.IdComboNavigation.IdBebidaNavigation != null ? comboPedido.IdComboNavigation.IdBebidaNavigation.Nome : "n/a";
                string sobremesa = comboPedido.IdComboNavigation.IdSobremesaNavigation != null ? comboPedido.IdComboNavigation.IdSobremesaNavigation.Nome : "n/a";

                List<string> alteracoesIngrediente = [];

                if (comboPedido.IdComboNavigation.AlteracoesIngredientes.Count != 0)
                {
                    foreach (AlteracaoIngredienteCombo alteracao in comboPedido.IdComboNavigation.AlteracoesIngredientes)
                    {
                        alteracoesIngrediente.Add($"*** {alteracao.Quantidade}x - {alteracao.IdIngredienteNavigation.Nome}");
                    }
                }

                itens[dictKey].Add(new
                {
                    idPedido = comboPedido.IdPedido,
                    lanche,
                    acompanhamento,
                    bebida,
                    sobremesa,
                    alteracoesIngrediente
                });
            }

            return itens;
        }

        public static async Task<Dictionary<string, List<object>>> GetPedidosPainelAdministrador(IPedidoGateway pedidoGateway, IComboPedidoGateway comboPedidoGateway, IOrderMsGateway orderMsGateway, IPaymentMsGateway paymentMsGateway, ILoginMsGateway loginMsGateway, string token)
        {
            List<ComboPedido> comboPedidos = [];

            Dictionary<string, List<object>> itens = new Dictionary<string, List<object>>()
            {
                { StatusPedidoEnum.Recebido.ToString(), new List<object>() },
                { StatusPedidoEnum.EmPreparacao.ToString(), new List<object>() },
                { StatusPedidoEnum.Pronto.ToString(), new List<object>() },
                { StatusPedidoEnum.Finalizado.ToString(), new List<object>() },
            };

            comboPedidos.AddRange(await comboPedidoGateway.GetAllComboPedidosByStatusPedido(orderMsGateway, paymentMsGateway, loginMsGateway, StatusPedidoEnum.Recebido, token));
            comboPedidos.AddRange(await comboPedidoGateway.GetAllComboPedidosByStatusPedido(orderMsGateway, paymentMsGateway, loginMsGateway, StatusPedidoEnum.EmPreparacao, token));
            comboPedidos.AddRange(await comboPedidoGateway.GetAllComboPedidosByStatusPedido(orderMsGateway, paymentMsGateway, loginMsGateway, StatusPedidoEnum.Pronto, token));
            comboPedidos.AddRange(await comboPedidoGateway.GetAllComboPedidosByStatusPedido(orderMsGateway, paymentMsGateway, loginMsGateway, StatusPedidoEnum.Finalizado,token));

            foreach (ComboPedido comboPedido in comboPedidos)
            {
                string dictKey = ((StatusPedidoEnum)comboPedido.IdPedidoNavigation.IdStatusPedido!).ToString();

                string lanche = comboPedido.IdComboNavigation.IdLancheNavigation != null ? comboPedido.IdComboNavigation.IdLancheNavigation.Nome : "n/a";
                string acompanhamento = comboPedido.IdComboNavigation.IdAcompanhamentoNavigation != null ? comboPedido.IdComboNavigation.IdAcompanhamentoNavigation.Nome : "n/a";
                string bebida = comboPedido.IdComboNavigation.IdBebidaNavigation != null ? comboPedido.IdComboNavigation.IdBebidaNavigation.Nome : "n/a";
                string sobremesa = comboPedido.IdComboNavigation.IdSobremesaNavigation != null ? comboPedido.IdComboNavigation.IdSobremesaNavigation.Nome : "n/a";

                List<string> alteracoesIngrediente = [];

                if (comboPedido.IdComboNavigation.AlteracoesIngredientes.Count != 0)
                {
                    foreach (AlteracaoIngredienteCombo alteracao in comboPedido.IdComboNavigation.AlteracoesIngredientes)
                    {
                        alteracoesIngrediente.Add($"*** {alteracao.Quantidade}x - {alteracao.IdIngredienteNavigation.Nome}");
                    }
                }

                itens[dictKey].Add(new
                {
                    idPedido = comboPedido.IdPedido,
                    lanche,
                    acompanhamento,
                    bebida,
                    sobremesa,
                    alteracoesIngrediente
                });
            }

            return itens;
        }
    }
}
