using Core.Dtos;
using Core.Entities;

namespace Core.Interfaces.Gateways
{
    public interface IIngredienteGateway
    {
        public Task<IEnumerable<Ingrediente>> GetAll();
        public Task<Ingrediente?> GetById(int idIngrediente);
        public Task<Ingrediente> Insert(NomePrecoDto nomePrecoDto);
        public Task Update(Ingrediente ingrediente);
        public Task Delete(Ingrediente ingrediente);
    }
}
