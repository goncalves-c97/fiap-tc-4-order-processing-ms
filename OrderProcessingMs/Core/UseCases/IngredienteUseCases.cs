using Core.Dtos;
using Core.Entities;
using Core.Interfaces.Gateways;

namespace Core.UseCases
{
    public static class IngredienteUseCases
    {
        public static Task<IEnumerable<Ingrediente>> GetAllIngredientes(IIngredienteGateway gateway)
            => gateway.GetAll();

        public static Task<Ingrediente?> GetIngredienteById(IIngredienteGateway gateway, int id)
            => gateway.GetById(id);

        public static Task<Ingrediente> InsertIngrediente(IIngredienteGateway gateway, NomePrecoDto dto)
            => gateway.Insert(dto);

        public static async Task UpdateIngrediente(IIngredienteGateway gateway, int id, NomePrecoDto dto)
        {
            var ingrediente = await gateway.GetById(id) ?? throw new KeyNotFoundException();
            ingrediente.Nome = dto.Nome;
            ingrediente.PrecoAdicional = dto.Preco;
            await gateway.Update(ingrediente);
        }

        public static async Task DeleteIngrediente(IIngredienteGateway gateway, int id)
        {
            var ingrediente = await gateway.GetById(id);
            if (ingrediente != null)
                await gateway.Delete(ingrediente);
        }
    }
}