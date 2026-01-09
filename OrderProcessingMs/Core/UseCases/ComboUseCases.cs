using Core.Dtos;
using Core.Entities;
using Core.Interfaces.Gateways;

namespace Core.UseCases
{
    public static class ComboUseCases
    {
        public static async Task<Combo> Insert(IComboGateway comboGateway, ComboDto comboDto)
        {
            return await comboGateway.Insert(comboDto);
        }

        public static async Task<Combo> GetCompleteComboById(IComboGateway comboGateway, int idCombo)
        {
            if (idCombo <= 0)
                throw new ArgumentException($"'{nameof(idCombo)}' deve ser maior que zero");

            return await comboGateway.GetCompleteComboById(idCombo)
                ?? throw new KeyNotFoundException($"Combo with ID {idCombo} not found.");
        }
    }
}
