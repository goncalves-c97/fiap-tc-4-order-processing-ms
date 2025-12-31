using Core.Constants;
using Core.Controllers;
using Core.Interfaces;
using Core.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;

namespace WebApi.Endpoints
{
    [ApiController]
    [Route("Painel")]
    public class PainelEndpoint(IDbConnection dbConnection) : ControllerBase
    {
        private readonly IDbConnection _dbConnection = dbConnection;

        [Authorize(Roles = $"{UsuarioRoles.ClienteIdentificado}, {UsuarioRoles.ClienteAnonimo}")]
        [HttpGet, Route("TotemAutoatendimento")]
        public async Task<IActionResult> TotemAutotendimento()
        {
            return Ok(await PedidoController.GetPedidosTelaoPedidos(_dbConnection));
        }

        [Authorize(Roles = UsuarioRoles.Cozinheiro)]
        [HttpGet, Route("Cozinha")]
        public async Task<IActionResult> Cozinha()
        {
            return Ok(await PedidoController.GetPedidosPainelCozinha(_dbConnection));
        }

        [Authorize(Roles = UsuarioRoles.Atendente)]
        [HttpGet, Route("Atendente")]
        public async Task<IActionResult> Atendente()
        {
            return Ok(await PedidoController.GetPedidosPainelAtendente(_dbConnection));
        }

        [Authorize(Roles = UsuarioRoles.Administrador)]
        [HttpGet, Route("Administrador")]
        public async Task<IActionResult> Administrador()
        {
            return Ok(await PedidoController.GetPedidosPainelAdministrador(_dbConnection));
        }
    }
}
