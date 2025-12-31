using Core.Dtos;
using Core.Entities;

namespace Core.Interfaces.Gateways
{
    public interface IAcompanhamentoGateway
    {
        public Task<IEnumerable<Acompanhamento>> GetAll();
        public Task<Acompanhamento?> GetById(int idAcompanhamento);
        public Task<Acompanhamento> Insert(NomePrecoDto nomePrecoDto);
        public Task Update(Acompanhamento acompanhamento);
        public Task Delete(Acompanhamento acompanhamento);
    }
}
