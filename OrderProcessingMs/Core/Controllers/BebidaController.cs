using Core.Dtos;
using Core.Entities;
using Core.Gateways;
using Core.Interfaces;
using Core.UseCases;

namespace Core.Controllers
{
    public static class BebidaController
    {
        public static async Task<IEnumerable<Bebida>> GetAll(IDbConnection dbConnection)
        {
            var gateway = new BebidaGateway(dbConnection);
            return await BebidaUseCases.GetAllBebidas(gateway);
        }

        public static async Task<Bebida> InsertNewBebida(IDbConnection dbConnection, NomePrecoDto dto)
        {
            var gateway = new BebidaGateway(dbConnection);
            return await BebidaUseCases.InsertBebida(gateway, dto);
        }

        public static async Task UpdateBebida(IDbConnection dbConnection, int id, NomePrecoDto dto)
        {
            var gateway = new BebidaGateway(dbConnection);
            await BebidaUseCases.UpdateBebida(gateway, id, dto);
        }

        public static async Task DeleteBebida(IDbConnection dbConnection, int id)
        {
            var gateway = new BebidaGateway(dbConnection);
            await BebidaUseCases.DeleteBebida(gateway, id);
        }
    }
}