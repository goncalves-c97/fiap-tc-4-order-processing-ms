using Core.Dtos;
using Core.Entities;
using Core.Gateways;
using Core.Interfaces;
using Core.UseCases;

namespace Core.Controllers
{
    public static class SobremesaController
    {
        public static async Task<IEnumerable<Sobremesa>> GetAll(IDbConnection dbConnection)
        {
            var gateway = new SobremesaGateway(dbConnection);
            return await SobremesaUseCases.GetAllSobremesas(gateway);
        }

        public static async Task<Sobremesa> InsertNewSobremesa(IDbConnection dbConnection, NomePrecoDto dto)
        {
            var gateway = new SobremesaGateway(dbConnection);
            return await SobremesaUseCases.InsertSobremesa(gateway, dto);
        }

        public static async Task UpdateSobremesa(IDbConnection dbConnection, int id, NomePrecoDto dto)
        {
            var gateway = new SobremesaGateway(dbConnection);
            await SobremesaUseCases.UpdateSobremesa(gateway, id, dto);
        }

        public static async Task DeleteSobremesa(IDbConnection dbConnection, int id)
        {
            var gateway = new SobremesaGateway(dbConnection);
            await SobremesaUseCases.DeleteSobremesa(gateway, id);
        }
    }
}