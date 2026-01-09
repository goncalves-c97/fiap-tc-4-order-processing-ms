using Core.Dtos;
using Core.Entities;
using Core.Factories;
using FluentAssertions;

namespace Test.Core.Factories;

public class IngredientesLancheFactoryTests
{
    [Fact]
    public void GetByCadastroDto_Should_MapDtoToEntity()
    {
        var dto = new CadastroIngredienteLancheDto(idLanche: 20, idIngrediente: 10, quantidade: 2);

        var entity = IngredientesLancheFactory.GetByCadastroDto(dto);

        entity.IdIngrediente.Should().Be(10);
        entity.IdLanche.Should().Be(20);
        entity.Quantidade.Should().Be(2);
    }

    [Fact]
    public void GetDtoByIngredientesLanche_Should_MapEntityToDto()
    {
        var entity = new IngredientesLanche { IdLanche = 20, IdIngrediente = 10, Quantidade = 2 };

        var dto = IngredientesLancheFactory.GetDtoByIngredientesLanche(entity);

        dto.IdLanche.Should().Be(20);
        dto.IdIngrediente.Should().Be(10);
        dto.Quantidade.Should().Be(2);
    }
}
