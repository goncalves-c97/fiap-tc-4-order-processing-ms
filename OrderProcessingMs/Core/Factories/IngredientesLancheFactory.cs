using Core.Dtos;
using Core.Entities;

namespace Core.Factories
{
    public static class IngredientesLancheFactory
    {
        public static IngredientesLanche GetByCadastroDto(CadastroIngredienteLancheDto cadastroIngredienteLancheDto)
        {
            return new IngredientesLanche()
            {
                IdIngrediente = cadastroIngredienteLancheDto.IdIngrediente,
                IdLanche = cadastroIngredienteLancheDto.IdLanche,
                Quantidade = cadastroIngredienteLancheDto.Quantidade
            };
        }

        public static CadastroIngredienteLancheDto GetDtoByIngredientesLanche(IngredientesLanche ingredientesLanche)
        {
            return new CadastroIngredienteLancheDto(
                ingredientesLanche.IdLanche,
                ingredientesLanche.IdIngrediente,
                ingredientesLanche.Quantidade);
        }
    }
}
