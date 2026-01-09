using Core.Dtos;
using Core.Entities;
using Core.UseCases.Microservices;
using FluentAssertions;
using Moq;
using Core.Interfaces.Gateways.Microservices;

namespace Test.Core.UseCases.Microservices;

public class PaymentMsUseCasesTests
{
    [Fact]
    public async Task CheckoutPedido_Should_CalculateTotalInCents_AndCallGateway()
    {
        var gw = new Mock<IPaymentMsGateway>(MockBehavior.Strict);
        gw.Setup(g => g.CheckoutPedido(10, 1500, "t"))
        .ReturnsAsync(new QrCodePagamentoDto("10", "prov", "qr"));

        var combo = new Combo
        {
            IdLanche = 1,
            IdLancheNavigation = new Lanche { Nome = "L", Preco = 15m, IngredientesLanche = new List<IngredientesLanche>() },
            AlteracoesIngredientes = new List<AlteracaoIngredienteCombo>()
        };

        var result = await PaymentMsUseCases.CheckoutPedido(gw.Object, 10, new[] { combo }, "t");
        result.QrCodeValue.Should().Be("qr");
        gw.VerifyAll();
    }
}
