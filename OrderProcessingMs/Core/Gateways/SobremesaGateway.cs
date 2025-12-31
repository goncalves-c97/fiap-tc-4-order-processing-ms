using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Gateways;
using Core.Dtos;

namespace Core.Gateways
{
    public class SobremesaGateway : ISobremesaGateway
    {
        private readonly IDbConnection _dbConnection;
        private const string TableName = nameof(Sobremesa);

        public SobremesaGateway(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<Sobremesa>> GetAll()
            => await _dbConnection.ListAllAsync<Sobremesa>(TableName);

        public async Task<Sobremesa?> GetById(int id)
            => await _dbConnection.SearchFirstOrDefaultByParametersAsync<Sobremesa>(TableName, "id_sobremesa = @id", new { id });

        public async Task<Sobremesa> Insert(NomePrecoDto dto)
        {
            var values = new Dictionary<string, object>
            {
                ["nome"] = dto.Nome,
                ["preco"] = dto.Preco
            };
            var id = await _dbConnection.InsertAndReturnIdAsync(TableName, values, "id_sobremesa");
            return await GetById(id) ?? throw new Exception("Insert failed");
        }

        public async Task Update(Sobremesa sobremesa)
        {
            var values = new Dictionary<string, object>
            {
                ["nome"] = sobremesa.Nome,
                ["preco"] = sobremesa.Preco
            };
            await _dbConnection.UpdateAsync(TableName, values, "id_sobremesa = @id", new { id = sobremesa.IdSobremesa });
        }

        public async Task Delete(Sobremesa sobremesa)
            => await _dbConnection.DeleteAsync(TableName, "id_sobremesa = @id", new { id = sobremesa.IdSobremesa });
    }
}