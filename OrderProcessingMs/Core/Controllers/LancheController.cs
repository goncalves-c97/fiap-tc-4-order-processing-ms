using Core.Dtos;
using Core.Entities;
using Core.Gateways;
using Core.Interfaces;
using Core.UseCases;

namespace Core.Controllers
{
    public static class LancheController
    {
        public static async Task<IEnumerable<Lanche>> GetAll(IDbConnection dbConnection)
        {
            var gateway = new LancheGateway(dbConnection);
            return await LancheUseCases.GetAllLanches(gateway);
        }

        public static async Task<IEnumerable<Lanche>> GetAllLanchesComIngredientes(IDbConnection dbConnection)
        {
            var lancheGateway = new LancheGateway(dbConnection);
            var ingredientesLancheGateway = new IngredientesLancheGateway(dbConnection);
            var ingredienteGateway = new IngredienteGateway(dbConnection);
            return await LancheUseCases.GetAllLanchesComIngredientes(lancheGateway, ingredientesLancheGateway, ingredienteGateway);
        }

        public static async Task<Lanche> InsertNewLanche(IDbConnection dbConnection, CadastroLancheDto dto)
        {
            var lancheGateway = new LancheGateway(dbConnection);
            var ingredientesLancheGateway = new IngredientesLancheGateway(dbConnection);
            var ingredienteGateway = new IngredienteGateway(dbConnection);
            return await LancheUseCases.InsertLanche(lancheGateway, ingredientesLancheGateway, ingredienteGateway, dto);
        }

        public static async Task UpdateLanche(IDbConnection dbConnection, int id, NomePrecoDto dto)
        {
            var gateway = new LancheGateway(dbConnection);
            await LancheUseCases.UpdateLanche(gateway, id, dto);
        }

        public static async Task UpdateLanche(IDbConnection dbConnection, int id, CadastroLancheDto dto)
        {
            var lancheGateway = new LancheGateway(dbConnection);
            var ingredientesLancheGateway = new IngredientesLancheGateway(dbConnection);

            await LancheUseCases.UpdateLancheAndIngredientes(lancheGateway, ingredientesLancheGateway, id, dto);
        }

        public static async Task DeleteLanche(IDbConnection dbConnection, int id)
        {
            var lancheGateway = new LancheGateway(dbConnection);
            var ingredientesLancheGateway = new IngredientesLancheGateway(dbConnection);
            await LancheUseCases.DeleteLanche(lancheGateway, ingredientesLancheGateway, id);
        }
    }
}