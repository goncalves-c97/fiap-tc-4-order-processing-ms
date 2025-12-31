using Core.Dtos;
using Core.Entities;
using Core.Enums;
using Core.Gateways;
using Core.Gateways.Microservices;
using Core.Interfaces;
using Core.Interfaces.Gateways.Microservices;
using Core.UseCases;
using Core.UseCases.Microservices;

namespace Core.Controllers
{
    public static class PedidoController
    {
        public static async Task<int> IniciaPedido(IOrderMsGateway orderMsGateway, string tokenCliente)
        {
            int idPedido = await OrderMsUseCases.IniciaPedido(orderMsGateway, tokenCliente);
            return idPedido;
        }

        public static async Task<QrCodePagamentoDto> CheckoutPedido(IDbConnection dbConnection, IPaymentMsGateway paymentMsGateway, int idPedido, List<ComboDto> combosDto, string token)
        {
            ComboGateway comboGateway = new(dbConnection);
            ComboPedidoGateway comboPedidoGateway = new(dbConnection);
            AlteracaoIngredienteComboGateway alteracaoIngredienteComboGateway = new(dbConnection);

            List<Combo> combos = await PedidoUseCases.RegisterCombosOnPedido(comboGateway, comboPedidoGateway, alteracaoIngredienteComboGateway, idPedido, combosDto);

            QrCodePagamentoDto qrCodePagamento = await PaymentMsUseCases.CheckoutPedido(paymentMsGateway, idPedido, combos, token);

            return qrCodePagamento;
        }

        public static async Task InformaPedidoEmPreparo(IOrderMsGateway orderMsGateway, int idPedido, string token)
        {
            
            await PedidoUseCases.UpdatePedidoStatusToEmPreparacao(orderMsGateway, idPedido, token);
        }

        public static async Task InformaPedidoPronto(IOrderMsGateway orderMsGateway, IEmailService emailService, int idPedido, string token)
        {

            await PedidoUseCases.UpdatePedidoStatusToPronto(orderMsGateway, idPedido, token);

            await EmailUseCases.SendNotificacaoPedidoPronto(emailService, idPedido, token);
        }

        public static async Task InformaPedidoFinalizado(IOrderMsGateway orderMsGateway, int idPedido, string token)
        { 
            await PedidoUseCases.UpdatePedidoStatusToFinalizado(orderMsGateway, idPedido, token);
        }

        public static async Task<Dictionary<string, List<object>>> GetPedidosTelaoPedidos(IDbConnection dbConnection)
        {
            PedidoGateway pedidoGateway = new(dbConnection);
            ComboPedidoGateway comboPedidoGateway = new(dbConnection);

            return await PedidoUseCases.GetPedidosTelaoPedidos(pedidoGateway, comboPedidoGateway);
        }

        public static async Task<Dictionary<string, List<object>>> GetPedidosPainelCozinha(IDbConnection dbConnection)
        {
            PedidoGateway pedidoGateway = new(dbConnection);
            ComboPedidoGateway comboPedidoGateway = new(dbConnection);

            return await PedidoUseCases.GetPedidosPainelCozinha(pedidoGateway, comboPedidoGateway);
        }

        public static async Task<Dictionary<string, List<object>>> GetPedidosPainelAtendente(IDbConnection dbConnection)
        {
            PedidoGateway pedidoGateway = new(dbConnection);
            ComboPedidoGateway comboPedidoGateway = new(dbConnection);

            return await PedidoUseCases.GetPedidosPainelAtendente(pedidoGateway, comboPedidoGateway);
        }

        public static async Task<Dictionary<string, List<object>>> GetPedidosPainelAdministrador(IDbConnection dbConnection)
        {
            PedidoGateway pedidoGateway = new(dbConnection);
            ComboPedidoGateway comboPedidoGateway = new(dbConnection);

            return await PedidoUseCases.GetPedidosPainelAdministrador(pedidoGateway, comboPedidoGateway);
        }

        //public static async Task<StatusPagamentoEnum> CheckStatusPagamentoPedido(IDbConnection dbConnection, int idPedido)
        //{
        //    PagamentoGateway pagamentoGateway = new(dbConnection);
        //    StatusPagamentoEnum statusPagamento = await PagamentoUseCases.VerificaStatusPagamentoPedido(pagamentoGateway, idPedido);
        //    return statusPagamento;
        //}
    }
}
