using Core.Dtos;
using Core.Entities;
using Core.Gateways;
using Core.Interfaces;
using Core.UseCases;

namespace Core.Controllers
{
    public static class IngredienteController
    {
        public static async Task<IEnumerable<Ingrediente>> GetAll(IDbConnection dbConnection)
        {
            var gateway = new IngredienteGateway(dbConnection);
            return await IngredienteUseCases.GetAllIngredientes(gateway);
        }

        public static async Task<Ingrediente> InsertNewIngrediente(IDbConnection dbConnection, NomePrecoDto dto)
        {
            var gateway = new IngredienteGateway(dbConnection);
            return await IngredienteUseCases.InsertIngrediente(gateway, dto);
        }

        public static async Task UpdateIngrediente(IDbConnection dbConnection, int id, NomePrecoDto dto)
        {
            var gateway = new IngredienteGateway(dbConnection);
            await IngredienteUseCases.UpdateIngrediente(gateway, id, dto);
        }

        public static async Task DeleteIngrediente(IDbConnection dbConnection, int id)
        {
            var ingredienteGateway = new IngredienteGateway(dbConnection);
            var ingredientesLancheGateway = new IngredientesLancheGateway(dbConnection);

            // Deleta todos os relacionametos do ingrediente com os lanches
            await IngredientesLancheUseCases.DeleteAllByIngredienteId(ingredientesLancheGateway, id);

            // Deleta o ingrediente
            await IngredienteUseCases.DeleteIngrediente(ingredienteGateway, id);
        }
    }
}