using Core.Dtos;
using Core.Entities;

namespace Core.Interfaces.Gateways
{
    public interface IIngredientesLancheGateway
    {
        public Task<IEnumerable<IngredientesLanche>> GetAllByLancheId(int idLanche);
        public Task<IEnumerable<IngredientesLanche>> Insert(IEnumerable<CadastroIngredienteLancheDto> ingredientesLanche);
        public Task UpdateByIdLanche(IEnumerable<IngredientesLanche> ingredientesLanches);
        public Task DeleteAllByLancheId(int idLanche);
        public Task DeleteAllByIngredienteId(int idIngrediente);
        public Task DeleteByLancheIdAndIngredienteId(int idLanche, int idIngrediente);
    }
}
