using Core.Dtos;
using Core.Entities;
using Core.Interfaces.Gateways;

namespace Core.UseCases
{
    public static class SobremesaUseCases
    {
        public static Task<IEnumerable<Sobremesa>> GetAllSobremesas(ISobremesaGateway gateway)
            => gateway.GetAll();

        public static Task<Sobremesa?> GetSobremesaById(ISobremesaGateway gateway, int id)
            => gateway.GetById(id);

        public static Task<Sobremesa> InsertSobremesa(ISobremesaGateway gateway, NomePrecoDto dto)
            => gateway.Insert(dto);

        public static async Task UpdateSobremesa(ISobremesaGateway gateway, int id, NomePrecoDto dto)
        {
            var sobremesa = await gateway.GetById(id) ?? throw new KeyNotFoundException();
            sobremesa.Nome = dto.Nome;
            sobremesa.Preco = dto.Preco;
            await gateway.Update(sobremesa);
        }

        public static async Task DeleteSobremesa(ISobremesaGateway gateway, int id)
        {
            var sobremesa = await gateway.GetById(id);
            if (sobremesa != null)
                await gateway.Delete(sobremesa);
        }
    }
}