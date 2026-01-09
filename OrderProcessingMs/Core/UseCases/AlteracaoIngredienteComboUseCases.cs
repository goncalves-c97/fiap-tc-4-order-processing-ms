using Core.Dtos;
using Core.Entities;
using Core.Interfaces.Gateways;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UseCases
{
    public static class AlteracaoIngredienteComboUseCases
    {
        public static async Task<IEnumerable<AlteracaoIngredienteCombo>> GetAllByIdCombo(IAlteracaoIngredienteComboGateway alteracaoIngredienteComboGateway, int idCombo)
        {
            if (alteracaoIngredienteComboGateway == null)
                throw new ArgumentNullException(nameof(alteracaoIngredienteComboGateway), "AlteracaoIngredienteCombo não pode ser null.");

            if (idCombo <= 0)
                throw new ArgumentOutOfRangeException(nameof(idCombo), "IdCombo deve ser maior que zero.");

            return await alteracaoIngredienteComboGateway.GetAllByIdCombo(idCombo);
        }

        public static async Task AddAlteracaoIngredienteOnCombo(IAlteracaoIngredienteComboGateway alteracaoIngredienteComboGateway, int idCombo, IngredienteLancheDto alteracaoIngredienteDto)
        {
            if (alteracaoIngredienteComboGateway == null)
                throw new ArgumentNullException(nameof(alteracaoIngredienteComboGateway), "AlteracaoIngredienteCombo não pode ser null.");

            if (idCombo <= 0)
                throw new ArgumentOutOfRangeException(nameof(idCombo), "IdCombo deve ser maior que zero.");

            await alteracaoIngredienteComboGateway.AddAlteracaoIngredienteOnCombo(idCombo, alteracaoIngredienteDto);
        }

        public static async Task DeleteAlteracoesIngredienteOnCombo(IAlteracaoIngredienteComboGateway alteracaoIngredienteComboGateway, int idCombo)
        {
            if (alteracaoIngredienteComboGateway == null)
                throw new ArgumentNullException(nameof(alteracaoIngredienteComboGateway), "AlteracaoIngredienteCombo não pode ser null.");

            if (idCombo <= 0)
                throw new ArgumentOutOfRangeException(nameof(idCombo), "IdCombo deve ser maior que zero.");

            await alteracaoIngredienteComboGateway.DeleteAlteracoesIngredienteOnCombo(idCombo);
        }

        public static async Task UpdateAlteracoesIngredienteOnCombo(IAlteracaoIngredienteComboGateway alteracaoIngredienteComboGateway, int idCombo, IEnumerable<IngredienteLancheDto> ingredienteLancheDtos)
        {
            if (alteracaoIngredienteComboGateway == null)
                throw new ArgumentNullException(nameof(alteracaoIngredienteComboGateway), "AlteracaoIngredienteCombo não pode ser null.");

            if (idCombo <= 0)
                throw new ArgumentOutOfRangeException(nameof(idCombo), "IdCombo deve ser maior que zero.");

            // Delete as alterações de ingredientes, caso haja
            await DeleteAlteracoesIngredienteOnCombo(alteracaoIngredienteComboGateway, idCombo);

            // Adiciona as novas alterações de ingrediente ao banco, caso haja
            foreach (IngredienteLancheDto ingrediente in ingredienteLancheDtos)
            {
                await AddAlteracaoIngredienteOnCombo(alteracaoIngredienteComboGateway, idCombo, ingrediente);
            }
        }
    }
}
