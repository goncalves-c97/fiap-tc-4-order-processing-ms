using Core.Dtos;
using Core.Entities;

namespace Core.Interfaces.Gateways
{
    public interface ILancheGateway
    {
        public Task<IEnumerable<Lanche>> GetAll();
        public Task<Lanche?> GetById(int idLanche);
        public Task<Lanche> Insert(NomePrecoDto dto);
        public Task Update(Lanche lanche);
        public Task Delete(Lanche lanche);
    }
}
