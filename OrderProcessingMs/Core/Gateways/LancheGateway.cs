using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Gateways;
using Core.Dtos;

namespace Core.Gateways
{
    public class LancheGateway : ILancheGateway
    {
        private readonly IDbConnection _dbConnection;
        private const string TableName = nameof(Lanche);

        public LancheGateway(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<Lanche>> GetAll()
            => await _dbConnection.ListAllAsync<Lanche>(TableName);

        public async Task<Lanche?> GetById(int id)
            => await _dbConnection.SearchFirstOrDefaultByParametersAsync<Lanche>(TableName, "id_lanche = @id", new { id });

        public async Task<Lanche> Insert(NomePrecoDto dto)
        {
            var values = new Dictionary<string, object>
            {
                ["nome"] = dto.Nome,
                ["preco"] = dto.Preco
            };
            var id = await _dbConnection.InsertAndReturnIdAsync(TableName, values, "id_lanche");
            return await GetById(id) ?? throw new Exception("Insert failed");
        }

        public async Task Update(Lanche lanche)
        {
            var values = new Dictionary<string, object>
            {
                ["nome"] = lanche.Nome,
                ["preco"] = lanche.Preco
            };
            await _dbConnection.UpdateAsync(TableName, values, "id_lanche = @id", new { id = lanche.IdLanche });
        }

        public async Task Delete(Lanche lanche)
            => await _dbConnection.DeleteAsync(TableName, "id_lanche = @id", new { id = lanche.IdLanche });
    }
}