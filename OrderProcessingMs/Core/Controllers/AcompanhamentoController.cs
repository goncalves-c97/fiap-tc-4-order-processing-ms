using Core.Dtos;
using Core.Entities;
using Core.Gateways;
using Core.Interfaces;
using Core.UseCases;

namespace Core.Controllers
{
    public static class AcompanhamentoController
    {
        public static async Task<IEnumerable<Acompanhamento>> GetAll(IDbConnection dbConnection)
        {
            AcompanhamentoGateway gateway = new(dbConnection);
            IEnumerable<Acompanhamento> acompanhamentos = await AcompanhamentoUseCases.GetAllAcompanhamentos(gateway);
            return acompanhamentos;
        }

        public static async Task<Acompanhamento> InsertNewAcompanhamento(IDbConnection dbConnection, NomePrecoDto nomePrecoDto)
        {
            AcompanhamentoGateway gateway = new(dbConnection);
            Acompanhamento acompanhamento = await AcompanhamentoUseCases.InsertAcompanhamento(gateway, nomePrecoDto);
            return acompanhamento;
        }

        public static async Task UpdateAcompanhamento(IDbConnection dbConnection, int idAcompanhamento, NomePrecoDto nomePrecoDto)
        {
            AcompanhamentoGateway gateway = new(dbConnection);
            await AcompanhamentoUseCases.UpdateAcompanhamento(gateway, idAcompanhamento, nomePrecoDto);
        }

        public static async Task DeleteAcompanhamento(IDbConnection dbConnection, int idAcompanhamento)
        {
            AcompanhamentoGateway gateway = new(dbConnection);
            await AcompanhamentoUseCases.DeleteAcompanhamento(gateway, idAcompanhamento);
        }
    }
}
