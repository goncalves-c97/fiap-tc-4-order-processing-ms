using Core.Interfaces.Gateways;
using Core.Interfaces;
using Core.Entities;
using Core.Dtos;

namespace Core.Gateways
{
    public class AcompanhamentoGateway(IDbConnection dbConnection) : IAcompanhamentoGateway
    {
        private readonly IDbConnection _dbConnection = dbConnection;
        private const string TableName = nameof(Acompanhamento);

        public async Task Delete(Acompanhamento acompanhamento)
        {
            await _dbConnection.DeleteAsync(TableName, "id_acompanhamento = @Id", new { Id = acompanhamento.IdAcompanhamento });
        }

        public async Task<IEnumerable<Acompanhamento>> GetAll()
        {
            return await _dbConnection.ListAllAsync<Acompanhamento>(nameof(Acompanhamento));
        }

        public async Task<Acompanhamento?> GetById(int idAcompanhamento)
        {
            return await _dbConnection.SearchFirstOrDefaultByParametersAsync<Acompanhamento>(
                TableName,
                "id_acompanhamento = @Id",
                new { Id = idAcompanhamento }
            );
        }

        public async Task<Acompanhamento> Insert(NomePrecoDto nomePrecoDto)
        {
            int registeredId = await _dbConnection.InsertAndReturnIdAsync(TableName, new Dictionary<string, object>
            {
                { "nome", nomePrecoDto.Nome },
                { "preco", nomePrecoDto.Preco }
            }, "id_acompanhamento");

            return await GetById(registeredId);
        }

        public async Task Update(Acompanhamento acompanhamento)
        {
            await _dbConnection.UpdateAsync(TableName,
                new Dictionary<string, object>
                {
                    { "nome", acompanhamento.Nome },
                    { "preco", acompanhamento.Preco }
                }, "id_acompanhamento = @Id", new { Id = acompanhamento.IdAcompanhamento });
        }
    }
}