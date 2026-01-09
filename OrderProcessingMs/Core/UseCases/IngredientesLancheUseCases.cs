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
    public static class IngredientesLancheUseCases
    {
        public static Task<IEnumerable<IngredientesLanche>> GetAllByLancheId(IIngredientesLancheGateway ingredientesLancheGateway, int lancheId)
        {
            ArgumentNullException.ThrowIfNull(ingredientesLancheGateway);

            if (lancheId <= 0)
                throw new ArgumentException("ID do lanche deve ser maior que zero.", nameof(lancheId));

            return ingredientesLancheGateway.GetAllByLancheId(lancheId);
        }

        public static Task<IEnumerable<IngredientesLanche>> Insert(IIngredientesLancheGateway ingredientesLancheGateway, IEnumerable<CadastroIngredienteLancheDto> ingredientesLanche)
        {
            ArgumentNullException.ThrowIfNull(ingredientesLancheGateway);
            ArgumentNullException.ThrowIfNull(ingredientesLanche);

            if (!ingredientesLanche.Any())
                throw new ArgumentException("A lista de ingredientes não pode estar vazia.", nameof(ingredientesLanche));

            return ingredientesLancheGateway.Insert(ingredientesLanche);
        }

        /// <summary>
        /// Remove todas as relações de um ingrediente específico com lanches.
        /// </summary>
        /// <param name="ingredientesLancheGateway">Gateway para operações de IngredientesLanche.</param>
        /// <param name="idIngrediente">ID do ingrediente a ser removido das relações.</param>
        /// <returns>Task representando a operação assíncrona.</returns>
        /// <exception cref="ArgumentException">Lançada se o ID do ingrediente for inválido.</exception>
        /// </summary>
        public static Task DeleteAllByIngredienteId(IIngredientesLancheGateway ingredientesLancheGateway, int idIngrediente)
        {
            ArgumentNullException.ThrowIfNull(ingredientesLancheGateway);

            if (idIngrediente <= 0)
                throw new ArgumentException("ID do ingrediente deve ser maior que zero.", nameof(idIngrediente));

            return ingredientesLancheGateway.DeleteAllByIngredienteId(idIngrediente);
        }
    }
}
