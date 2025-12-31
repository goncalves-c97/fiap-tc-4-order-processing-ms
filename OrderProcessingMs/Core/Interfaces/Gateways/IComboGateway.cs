using Core.Dtos;
using Core.Entities;

namespace Core.Interfaces.Gateways
{
    public interface IComboGateway
    {
        Task<Combo?> GetById(int idCombo);
        Task<Combo> Insert(ComboDto comboDto);

        /// <summary>
        /// Obtém o combo completo, incluindo ingredientes e alterações de ingredientes.
        /// </summary>
        /// <param name="idCombo"></param>
        /// <returns></returns>
        Task<Combo?> GetCompleteComboById(int idCombo);
    }
}
