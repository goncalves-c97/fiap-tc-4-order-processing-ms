using Core.Dtos;
using Core.Entities;
using Core.Interfaces.Gateways;

namespace Core.UseCases
{
    public static class LancheUseCases
    {
        public static Task<IEnumerable<Lanche>> GetAllLanches(ILancheGateway gateway)
            => gateway.GetAll();

        public static async Task<IEnumerable<Lanche>> GetAllLanchesComIngredientes(ILancheGateway lancheGateway, IIngredientesLancheGateway ingredientesLancheGateway, IIngredienteGateway ingredienteGateway)
        {
            IEnumerable<Lanche> lanches = await lancheGateway.GetAll();

            foreach (Lanche lanche in lanches)
            {
                lanche.IngredientesLanche = [.. await IngredientesLancheUseCases.GetAllByLancheId(ingredientesLancheGateway, lanche.IdLanche)];

                foreach (IngredientesLanche ingredienteLanche in lanche.IngredientesLanche)
                {
                    ingredienteLanche.IdIngredienteNavigation = await IngredienteUseCases.GetIngredienteById(ingredienteGateway, ingredienteLanche.IdIngrediente);
                }
            }

            return lanches;
        }

        public static Task<Lanche?> GetLancheById(ILancheGateway gateway, int id)
            => gateway.GetById(id);

        public static async Task<Lanche> InsertLanche(ILancheGateway lancheGateway, IIngredientesLancheGateway ingredientesLancheGateway, IIngredienteGateway ingredienteGateway, CadastroLancheDto dto)
        { 
            NomePrecoDto nomePrecoDto = new()
            {
                Nome = dto.Nome,
                Preco = dto.Preco
            };

            Lanche lanche = await lancheGateway.Insert(nomePrecoDto);

            List<CadastroIngredienteLancheDto> cadastroIngredienteLancheDtos = dto.IngredientesLanche
                .Select(ingrediente => new CadastroIngredienteLancheDto(
                    lanche.IdLanche,
                    ingrediente.IdIngrediente,
                    ingrediente.Quantidade))
                .ToList();
               
            lanche.IngredientesLanche = [..await IngredientesLancheUseCases.Insert(ingredientesLancheGateway, cadastroIngredienteLancheDtos)];

            foreach (IngredientesLanche ingredienteLanche in lanche.IngredientesLanche)
            {
                ingredienteLanche.IdIngredienteNavigation = await IngredienteUseCases.GetIngredienteById(ingredienteGateway, ingredienteLanche.IdIngrediente);
            }

            return lanche;
        }

        public static async Task UpdateLanche(ILancheGateway gateway, int id, NomePrecoDto dto)
        {
            var lanche = await gateway.GetById(id) ?? throw new KeyNotFoundException();
            lanche.Nome = dto.Nome;
            lanche.Preco = dto.Preco;
            await gateway.Update(lanche);
        }

        public static async Task UpdateLancheAndIngredientes(ILancheGateway gateway, IIngredientesLancheGateway ingredientesLancheGateway, int id, CadastroLancheDto dto)
        {
            await UpdateLanche(gateway, id, new NomePrecoDto { Nome = dto.Nome, Preco = dto.Preco });

            await ingredientesLancheGateway.UpdateByIdLanche(
                dto.IngredientesLanche.Select(ingrediente => new IngredientesLanche
                {
                    IdLanche = id,
                    IdIngrediente = ingrediente.IdIngrediente,
                    Quantidade = ingrediente.Quantidade
                })
            );
        }

        public static async Task DeleteLanche(ILancheGateway gateway, IIngredientesLancheGateway ingredientesLancheGateway, int id)
        {
            var lanche = await gateway.GetById(id);

            if (lanche != null)
            {
                // Deleta todos os relacionamentos de ingredientes com o lanche
                await ingredientesLancheGateway.DeleteAllByLancheId(lanche.IdLanche);

                // Deleta o lanche
                await gateway.Delete(lanche);
            }
        }
    }
}