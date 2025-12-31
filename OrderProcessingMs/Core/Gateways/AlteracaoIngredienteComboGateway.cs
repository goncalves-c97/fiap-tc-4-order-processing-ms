using Core.Interfaces.Gateways;
using Core.Interfaces;
using Core.Entities;
using Core.Dtos;

namespace Core.Gateways
{
    public class AlteracaoIngredienteComboGateway(IDbConnection dbConnection) : IAlteracaoIngredienteComboGateway
    {
        private readonly IDbConnection _dbConnection = dbConnection;
        private readonly string _tableName = "Alteracao_ingrediente_combo";

        public async Task AddAlteracaoIngredienteOnCombo(int idCombo, IngredienteLancheDto alteracaoIngredienteDto)
        {
            await _dbConnection.InsertAsync(
                _tableName,
                new Dictionary<string, object>
                {
                    { "id_combo", idCombo },
                    { "id_ingrediente", alteracaoIngredienteDto.IdIngrediente },
                    { "quantidade", alteracaoIngredienteDto.Quantidade }
                }
            );
        }

        public async Task DeleteAlteracoesIngredienteOnCombo(int idCombo)
        {
            await _dbConnection.DeleteAsync(
                _tableName,
                "id_combo = @Id",
                new { Id = idCombo }
            );
        }

        public async Task<IEnumerable<AlteracaoIngredienteCombo>> GetAllByIdCombo(int idCombo)
        {
            return await _dbConnection.SearchByParametersAsync<AlteracaoIngredienteCombo>(
                "AlteracaoIngredienteCombo",
                "id_combo = @Id",
                new { Id = idCombo }
            );
        }
    }
}