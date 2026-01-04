using Core.Constants;
using Core.Controllers;
using Core.Dtos;
using Core.Interfaces;
using Core.Interfaces.Gateways.Microservices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Endpoints
{
    [ApiController]
    [Route("Pedido")]
    public class PedidoEndpoint(IDbConnection dbConnection, IOrderMsGateway orderMsGateway, IPaymentMsGateway paymentMsGateway) : ControllerBase
    {
        private readonly IDbConnection _dbConnection = dbConnection;
        private readonly IOrderMsGateway _orderMsGateway = orderMsGateway;
        private readonly IPaymentMsGateway _paymentMsGateway = paymentMsGateway;

        [Authorize(Roles = $"{UsuarioRoles.ClienteIdentificado}, {UsuarioRoles.ClienteAnonimo}")]
        [HttpPost, Route("IniciaPedido")]
        public async Task<IActionResult> IniciaPedido()
        {
            string? idClienteClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (idClienteClaim == null)
                return Unauthorized("ID do cliente não encontrado.");

            if (!int.TryParse(idClienteClaim, out int idCliente))
                return Unauthorized("ID do cliente inválido!");

            int pedido = await PedidoController.IniciaPedido(_orderMsGateway, GetRequestToken(this));

            return Ok(new { idPedido = pedido });
        }

        [Authorize(Roles = $"{UsuarioRoles.ClienteIdentificado}, {UsuarioRoles.ClienteAnonimo}")]
        [HttpPost, Route("CheckoutPedido")]
        public async Task<IActionResult> ConfirmaPedido([FromQuery] int idPedido, [FromBody] List<ComboDto> combosPedido)
        {
            QrCodePagamentoDto qrCodePagamento = await PedidoController.CheckoutPedido(_dbConnection, _paymentMsGateway, idPedido, combosPedido, GetRequestToken(this));
            return Ok(qrCodePagamento);
        }

        [Authorize(Roles = $"{UsuarioRoles.Cozinheiro}")]
        [HttpPut, Route("IniciaPreparo")]
        public async Task<IActionResult> IniciaPreparo([FromQuery] int idPedido)
        {
            await PedidoController.InformaPedidoEmPreparo(_orderMsGateway, idPedido, GetRequestToken(this));
            return Ok();
        }

        [Authorize(Roles = $"{UsuarioRoles.Cozinheiro}")]
        [HttpPut, Route("FinalizaPreparo")]
        public async Task<IActionResult> FinalizaPreparo([FromQuery] int idPedido)
        {
            await PedidoController.InformaPedidoPronto(_orderMsGateway, idPedido, GetRequestToken(this));
            return Ok();
        }

        [Authorize(Roles = $"{UsuarioRoles.Atendente}")]
        [HttpPut, Route("FinalizaPedido")]
        public async Task<IActionResult> FinalizaPedido([FromQuery] int idPedido)
        {
            await PedidoController.InformaPedidoFinalizado(_orderMsGateway, idPedido, GetRequestToken(this));
            return Ok();
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
