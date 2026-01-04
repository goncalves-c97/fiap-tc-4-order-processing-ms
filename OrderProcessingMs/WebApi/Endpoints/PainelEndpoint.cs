using Core.Constants;
using Core.Controllers;
using Core.Interfaces;
using Core.Interfaces.Gateways.Microservices;
using Core.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;

namespace WebApi.Endpoints
{
    [ApiController]
    [Route("Painel")]
    public class PainelEndpoint(IDbConnection dbConnection, IOrderMsGateway orderMsGateway, IPaymentMsGateway paymentMsGateway, ILoginMsGateway loginMsGateway) : ControllerBase
    {
        private readonly IDbConnection _dbConnection = dbConnection;
        private readonly IOrderMsGateway _orderMsGateway = orderMsGateway;
        private readonly IPaymentMsGateway _paymentMsGateway = paymentMsGateway;
        private readonly ILoginMsGateway _loginMsGateway = loginMsGateway;

        [Authorize(Roles = $"{UsuarioRoles.ClienteIdentificado}, {UsuarioRoles.ClienteAnonimo}")]
        [HttpGet, Route("TotemAutoatendimento")]
        public async Task<IActionResult> TotemAutotendimento()
        {
            return Ok(await PedidoController.GetPedidosTelaoPedidos(_dbConnection, _orderMsGateway, _paymentMsGateway, _loginMsGateway, GetRequestToken(this)));
        }

        [Authorize(Roles = UsuarioRoles.Cozinheiro)]
        [HttpGet, Route("Cozinha")]
        public async Task<IActionResult> Cozinha()
        {
            return Ok(await PedidoController.GetPedidosPainelCozinha(_dbConnection, _orderMsGateway, _paymentMsGateway, _loginMsGateway, GetRequestToken(this)));
        }

        [Authorize(Roles = UsuarioRoles.Atendente)]
        [HttpGet, Route("Atendente")]
        public async Task<IActionResult> Atendente()
        {
            return Ok(await PedidoController.GetPedidosPainelAtendente(_dbConnection, _orderMsGateway, _paymentMsGateway, _loginMsGateway, GetRequestToken(this)));
        }

        [Authorize(Roles = UsuarioRoles.Administrador)]
        [HttpGet, Route("Administrador")]
        public async Task<IActionResult> Administrador()
        {
            return Ok(await PedidoController.GetPedidosPainelAdministrador(_dbConnection, _orderMsGateway, _paymentMsGateway, _loginMsGateway, GetRequestToken(this)));
        }

        [NonAction]
        private static string GetRequestToken(ControllerBase context)
        {
            return context.Request.Headers.Authorization
                .ToString()
                .Replace("Bearer ", string.Empty);
        }
    }
}
