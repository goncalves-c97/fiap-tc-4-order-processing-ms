using Core.Dtos;
using Core.Entities;

namespace Core.Interfaces.Gateways
{
    public interface IBebidaGateway
    {
        public Task<IEnumerable<Bebida>> GetAll();
        public Task<Bebida?> GetById(int idBebida);
        public Task<Bebida> Insert(NomePrecoDto nomePrecoDto);
        public Task Update(Bebida bebida);
        public Task Delete(Bebida bebida);
    }
}
