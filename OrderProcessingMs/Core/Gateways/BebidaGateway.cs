using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Gateways;
using Core.Dtos;

namespace Core.Gateways
{
    public class BebidaGateway : IBebidaGateway
    {
        private readonly IDbConnection _dbConnection;
        private const string TableName = nameof(Bebida);

        public BebidaGateway(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<Bebida>> GetAll()
            => await _dbConnection.ListAllAsync<Bebida>(TableName);

        public async Task<Bebida?> GetById(int id)
            => await _dbConnection.SearchFirstOrDefaultByParametersAsync<Bebida>(TableName, "id_bebida = @id", new { id });

        public async Task<Bebida> Insert(NomePrecoDto dto)
        {
            var values = new Dictionary<string, object>
            {
                ["nome"] = dto.Nome,
                ["preco"] = dto.Preco
            };
            var id = await _dbConnection.InsertAndReturnIdAsync(TableName, values, "id_bebida");
            return await GetById(id) ?? throw new Exception("Insert failed");
        }

        public async Task Update(Bebida bebida)
        {
            var values = new Dictionary<string, object>
            {
                ["nome"] = bebida.Nome,
                ["preco"] = bebida.Preco
            };
            await _dbConnection.UpdateAsync(TableName, values, "id_bebida = @id", new { id = bebida.IdBebida });
        }

        public async Task Delete(Bebida bebida)
            => await _dbConnection.DeleteAsync(TableName, "id_bebida = @id", new { id = bebida.IdBebida });
    }
}