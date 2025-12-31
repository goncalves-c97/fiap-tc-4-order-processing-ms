using Core.Dtos;
using Core.Entities;

namespace Core.Interfaces.Gateways
{
    public interface IAlteracaoIngredienteComboGateway
    {
        Task<IEnumerable<AlteracaoIngredienteCombo>> GetAllByIdCombo(int idCombo);
        Task AddAlteracaoIngredienteOnCombo(int idCombo, IngredienteLancheDto alteracaoIngredienteDto);
        Task DeleteAlteracoesIngredienteOnCombo(int idCombo);
    }
}
