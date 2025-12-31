using Core.Dtos;
using Core.Entities;

namespace Core.Interfaces.Gateways
{
    public interface ISobremesaGateway
    {
        public Task<IEnumerable<Sobremesa>> GetAll();
        public Task<Sobremesa?> GetById(int idSobremesa);
        public Task<Sobremesa> Insert(NomePrecoDto sobremesa);
        public Task Update(Sobremesa Sobremesa);
        public Task Delete(Sobremesa sobremesa);
    }
}
