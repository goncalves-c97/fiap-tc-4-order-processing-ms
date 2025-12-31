using Core.Constants;
using Core.Controllers;
using Core.Dtos;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints
{
    [Authorize]
    [ApiController]
    [Route("Produto")]
    public class ProdutoEndpoint : ControllerBase
    {
        private readonly IDbConnection _dbConnection;

        public ProdutoEndpoint(IDbConnection dbConnection) => _dbConnection = dbConnection;

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] string categoria)
        {
            if (string.IsNullOrWhiteSpace(categoria))
                return BadRequest("Informe na URL o tipo de produto desejado. Possíveis valores: Acompanhamento, Bebida, Lanche, Sobremesa, Ingrediente.");

            if (!Enum.TryParse<CategoriaProdutoEnum>(categoria, true, out var categoriaEnum))
                return BadRequest("Categoria inválida. Possíveis valores: Acompanhamento, Bebida, Lanche, Sobremesa, Ingrediente.");

            return categoriaEnum switch
            {
                CategoriaProdutoEnum.Acompanhamento => Ok(await AcompanhamentoController.GetAll(_dbConnection)),
                CategoriaProdutoEnum.Bebida => Ok(await BebidaController.GetAll(_dbConnection)),
                CategoriaProdutoEnum.Lanche => Ok(await LancheController.GetAllLanchesComIngredientes(_dbConnection)),
                CategoriaProdutoEnum.Sobremesa => Ok(await SobremesaController.GetAll(_dbConnection)),
                CategoriaProdutoEnum.Ingrediente => Ok(await IngredienteController.GetAll(_dbConnection)),
                _ => NotFound("Não foram encontrados produtos a partir da categoria passada. Possíveis categorias: Acompanhamento, Bebida, Lanche e Sobremesa"),
            };
        }

        [HttpGet("GetAllAcompanhamentos")]
        public async Task<IActionResult> GetAllAcompanhamentos()
        {
            return Ok(await AcompanhamentoController.GetAll(_dbConnection));
        }

        [HttpGet("GetAllBebidas")]
        public async Task<IActionResult> GetAllBebidas()
        {
            return Ok(await BebidaController.GetAll(_dbConnection));
        }

        [HttpGet("GetAllLanches")]
        public async Task<IActionResult> GetAllLanches()
        {
            return Ok(await LancheController.GetAllLanchesComIngredientes(_dbConnection));
        }

        [HttpGet("GetAllSobremesas")]
        public async Task<IActionResult> GetAllSobremesas()
        {
            return Ok(await SobremesaController.GetAll(_dbConnection));
        }

        [HttpGet("GetAllIngredientes")]
        public async Task<IActionResult> GetAllIngredientes()
        {
            return Ok(await IngredienteController.GetAll(_dbConnection));
        }

        [Authorize(Roles = UsuarioRoles.Administrador)]
        [HttpPost("CreateIngrediente")]
        public async Task<IActionResult> CreateIngrediente([FromBody] NomePrecoDto nomePrecoDto)
        {
            Ingrediente ingrediente = await IngredienteController.InsertNewIngrediente(_dbConnection, nomePrecoDto);
            return Ok(ingrediente);
        }

        [Authorize(Roles = UsuarioRoles.Administrador)]
        [HttpPut("UpdateIngrediente")]
        public async Task<IActionResult> UpdateIngrediente(int idIngrediente, [FromBody] NomePrecoDto ingrediente)
        {
            await IngredienteController.UpdateIngrediente(_dbConnection, idIngrediente, ingrediente);
            return Ok();
        }

        [Authorize(Roles = UsuarioRoles.Administrador)]
        [HttpDelete("DeleteIngrediente")]
        public async Task<IActionResult> DeleteIngrediente([FromQuery] int idIngrediente)
        {
            await IngredienteController.DeleteIngrediente(_dbConnection, idIngrediente);
            return Ok();
        }

        [Authorize(Roles = UsuarioRoles.Administrador)]
        [HttpPost("CreateLanche")]
        public async Task<IActionResult> CreateLanche([FromBody] CadastroLancheDto cadastroLancheDto)
        {
            Lanche lanche = await LancheController.InsertNewLanche(_dbConnection, cadastroLancheDto);
            return Ok(lanche);
        }

        [Authorize(Roles = UsuarioRoles.Administrador)]
        [HttpPut("UpdateLanche")]
        public async Task<IActionResult> UpdateLanche([FromQuery] int idLanche, [FromBody] NomePrecoDto nomePrecoDto)
        {
            await LancheController.UpdateLanche(_dbConnection, idLanche, nomePrecoDto);
            return Ok();
        }

        [Authorize(Roles = UsuarioRoles.Administrador)]
        [HttpPut("UpdateLancheAndIngredientes")]
        public async Task<IActionResult> UpdateLancheAndIngredientes([FromQuery] int idLanche, [FromBody] CadastroLancheDto cadastroLancheDto)
        {
            // TODO: Atualizar os ingredientes que compõem o lanche
            await LancheController.UpdateLanche(_dbConnection, idLanche, cadastroLancheDto);
            return Ok();
        }

        [Authorize(Roles = UsuarioRoles.Administrador)]
        [HttpDelete("DeleteLanche")]
        public async Task<IActionResult> DeleteLanche([FromQuery] int idLanche)
        {
            await LancheController.DeleteLanche(_dbConnection, idLanche);
            return Ok();
        }

        [Authorize(Roles = UsuarioRoles.Administrador)]
        [HttpPost("CreateAcompanhamento")]
        public async Task<IActionResult> CreateAcompanhamento([FromBody] NomePrecoDto nomePrecoDto)
        {
            Acompanhamento acompanhamento = await AcompanhamentoController.InsertNewAcompanhamento(_dbConnection, nomePrecoDto);
            return Ok(acompanhamento);
        }

        [Authorize(Roles = UsuarioRoles.Administrador)]
        [HttpPut("UpdateAcompanhamento")]
        public async Task<IActionResult> UpdateAcompanhamento(int idAcompanhamento, [FromBody] NomePrecoDto nomePrecoDto)
        {
            await AcompanhamentoController.UpdateAcompanhamento(_dbConnection, idAcompanhamento, nomePrecoDto);
            return Ok();
        }

        [Authorize(Roles = UsuarioRoles.Administrador)]
        [HttpDelete("DeleteAcompanhamento")]
        public async Task<IActionResult> DeleteAcompanhamento(int idAcompanhamento)
        {
            await AcompanhamentoController.DeleteAcompanhamento(_dbConnection, idAcompanhamento);
            return Ok();
        }

        [Authorize(Roles = UsuarioRoles.Administrador)]
        [HttpPost("CreateBebida")]
        public async Task<IActionResult> CreateBebida([FromBody] NomePrecoDto nomePrecoDto)
        {
            Bebida bebida = await BebidaController.InsertNewBebida(_dbConnection, nomePrecoDto);
            return Ok(bebida);
        }

        [Authorize(Roles = UsuarioRoles.Administrador)]
        [HttpPut("UpdateBebida")]
        public async Task<IActionResult> UpdateBebida(int idBebida, [FromBody] NomePrecoDto nomePrecoDto)
        {
            await BebidaController.UpdateBebida(_dbConnection, idBebida, nomePrecoDto);
            return Ok();
        }

        [Authorize(Roles = UsuarioRoles.Administrador)]
        [HttpDelete("DeleteBebida")]
        public async Task<IActionResult> DeleteBebida([FromQuery] int idBebida)
        {
            await BebidaController.DeleteBebida(_dbConnection, idBebida);
            return Ok();
        }

        [Authorize(Roles = UsuarioRoles.Administrador)]
        [HttpPost("CreateSobremesa")]
        public async Task<IActionResult> CreateSobremesa([FromBody] NomePrecoDto nomePrecoDto)
        {
            Sobremesa sobremesa = await SobremesaController.InsertNewSobremesa(_dbConnection, nomePrecoDto);
            return Ok(sobremesa);
        }

        [Authorize(Roles = UsuarioRoles.Administrador)]
        [HttpPut("UpdateSobremesa")]
        public async Task<IActionResult> UpdateSobremesa(int idSobremesa, [FromBody] NomePrecoDto sobremesa)
        {
            await SobremesaController.UpdateSobremesa(_dbConnection, idSobremesa, sobremesa);
            return Ok();
        }

        [Authorize(Roles = UsuarioRoles.Administrador)]
        [HttpDelete("DeleteSobremesa")]
        public async Task<IActionResult> DeleteSobremesa([FromQuery] int idSobremesa)
        {
            await SobremesaController.DeleteSobremesa(_dbConnection, idSobremesa);
            return Ok();
        }
    }
}
