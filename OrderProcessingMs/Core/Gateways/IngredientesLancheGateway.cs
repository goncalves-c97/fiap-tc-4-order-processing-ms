using Core.Interfaces.Gateways;
using Core.Interfaces;
using Core.Entities;
using Core.Dtos;
using Core.Factories;

namespace Core.Gateways
{
    public class IngredientesLancheGateway(IDbConnection dbConnection) : IIngredientesLancheGateway
    {
        private readonly IDbConnection _dbConnection = dbConnection;
        private const string TableName = "Ingredientes_lanche";

        public async Task DeleteAllByLancheId(int idLanche)
        {
            await _dbConnection.DeleteAsync(TableName, "id_lanche = @Id", new { Id = idLanche });
        }

        public async Task DeleteAllByIngredienteId(int idIngrediente)
        {
            await _dbConnection.DeleteAsync(TableName, "id_ingrediente = @Id", new { Id = idIngrediente });
        }

        public async Task DeleteByLancheIdAndIngredienteId(int idLanche, int idIngrediente)
        {
            await _dbConnection.DeleteAsync(TableName, 
                "id_lanche = @IdLanche AND id_ingrediente = @IdIngrediente", 
                new { IdLanche = idLanche, IdIngrediente = idIngrediente });
        }

        public async Task<IEnumerable<IngredientesLanche>> GetAllByLancheId(int idLanche)
        {
            return await _dbConnection.SearchByParametersAsync<IngredientesLanche>(TableName, "id_lanche = @Id", new { Id = idLanche });
        }

        public async Task<IEnumerable<IngredientesLanche>> Insert(IEnumerable<CadastroIngredienteLancheDto> ingredientesLanche)
        {
            List<IngredientesLanche> ingredientesLanches = [];

            foreach(CadastroIngredienteLancheDto ingredienteLanche in ingredientesLanche)
            {
                await _dbConnection.InsertAsync(TableName, new Dictionary<string, object>
                {
                    { "id_lanche", ingredienteLanche.IdLanche },
                    { "id_ingrediente", ingredienteLanche.IdIngrediente },
                    { "quantidade", ingredienteLanche.Quantidade   }
                });

                ingredientesLanches.Add(IngredientesLancheFactory.GetByCadastroDto(ingredienteLanche));
            }

            return ingredientesLanches;
        }

        public async Task UpdateByIdLanche(IEnumerable<IngredientesLanche> ingredientesLanches)
        {
            if (ingredientesLanches.Select(x => x.IdLanche).Distinct().Count() > 1)
                throw new ArgumentException("Todos os ingredientes devem pertencer ao mesmo lanche.", nameof(ingredientesLanches));

            // Deleta todos os ingredientes do lanche atual
            await DeleteAllByLancheId(ingredientesLanches.First().IdLanche);

            // Insere os novos ingredientes do lanche
            List<CadastroIngredienteLancheDto> ingredientesLancheDtos = ingredientesLanches
                .Select(IngredientesLancheFactory.GetDtoByIngredientesLanche)
                .ToList();

            await Insert(ingredientesLancheDtos);
        }
    }
}