using Core.Dtos;
using Core.Entities;
using Core.Interfaces.Gateways;

namespace Core.UseCases
{
    public static class BebidaUseCases
    {
        public static Task<IEnumerable<Bebida>> GetAllBebidas(IBebidaGateway gateway)
            => gateway.GetAll();

        public static Task<Bebida?> GetBebidaById(IBebidaGateway gateway, int id)
            => gateway.GetById(id);

        public static Task<Bebida> InsertBebida(IBebidaGateway gateway, NomePrecoDto dto)
            => gateway.Insert(dto);

        public static async Task UpdateBebida(IBebidaGateway gateway, int id, NomePrecoDto dto)
        {
            var bebida = await gateway.GetById(id) ?? throw new KeyNotFoundException();
            bebida.Nome = dto.Nome;
            bebida.Preco = dto.Preco;
            await gateway.Update(bebida);
        }

        public static async Task DeleteBebida(IBebidaGateway gateway, int id)
        {
            var bebida = await gateway.GetById(id);
            if (bebida != null)
                await gateway.Delete(bebida);
        }
    }
}