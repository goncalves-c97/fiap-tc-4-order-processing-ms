using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Gateways;
using Core.Dtos;

namespace Core.Gateways
{
    public class IngredienteGateway : IIngredienteGateway
    {
        private readonly IDbConnection _dbConnection;
        private const string TableName = nameof(Ingrediente);

        public IngredienteGateway(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<Ingrediente>> GetAll()
            => await _dbConnection.ListAllAsync<Ingrediente>(TableName);

        public async Task<Ingrediente?> GetById(int id)
            => await _dbConnection.SearchFirstOrDefaultByParametersAsync<Ingrediente>(TableName, "id_ingrediente = @id", new { id });

        public async Task<Ingrediente> Insert(NomePrecoDto dto)
        {
            var values = new Dictionary<string, object>
            {
                ["nome"] = dto.Nome,
                ["preco_adicional"] = dto.Preco
            };
            var id = await _dbConnection.InsertAndReturnIdAsync(TableName, values, "id_ingrediente");
            return await GetById(id) ?? throw new Exception("Insert failed");
        }

        public async Task Update(Ingrediente ingrediente)
        {
            var values = new Dictionary<string, object>
            {
                ["nome"] = ingrediente.Nome,
                ["preco_adicional"] = ingrediente.PrecoAdicional
            };
            await _dbConnection.UpdateAsync(TableName, values, "id_ingrediente = @id", new { id = ingrediente.IdIngrediente });
        }

        public async Task Delete(Ingrediente ingrediente)
            => await _dbConnection.DeleteAsync(TableName, "id_ingrediente = @id", new { id = ingrediente.IdIngrediente });
    }
}