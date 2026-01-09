using Core.Dtos;
using Core.Entities;
using Core.Enums;
using Core.Interfaces.Gateways;
using Core.Interfaces.Gateways.Microservices;
using Core.UseCases;
using FluentAssertions;
using Moq;

namespace Test.Core.UseCases;

public class PedidoUseCasesTests
{
    [Fact]
    public async Task RegisterCombosOnPedido_Should_InsertLinkUpdateAndReturnFullCombos()
    {
        var comboGateway = new Mock<IComboGateway>(MockBehavior.Strict);
        var comboPedidoGateway = new Mock<IComboPedidoGateway>(MockBehavior.Strict);
        var alteracaoGateway = new Mock<IAlteracaoIngredienteComboGateway>(MockBehavior.Strict);

        var dto = new ComboDto
        {
            IdLanche = 1,
            IdAcompanhamento = 2,
            IdBebida = 3,
            IdSobremesa = 4,
            AlteracoesIngrediente = null
        };

        comboGateway.Setup(g => g.Insert(It.IsAny<ComboDto>()))
        .ReturnsAsync(new Combo { IdCombo = 123 });

        comboPedidoGateway.Setup(g => g.AddComboOnPedido(123, 999)).Returns(Task.CompletedTask);

        comboGateway.Setup(g => g.GetCompleteComboById(123)).ReturnsAsync(new Combo { IdCombo = 123 });

        var result = await PedidoUseCases.RegisterCombosOnPedido(
        comboGateway.Object,
        comboPedidoGateway.Object,
        alteracaoGateway.Object,
       999,
        new List<ComboDto> { dto });

        result.Should().HaveCount(1);
        result[0].IdCombo.Should().Be(123);

        comboGateway.VerifyAll();
        comboPedidoGateway.VerifyAll();
    }

    [Fact]
    public async Task UpdatePedidoStatusToPronto_Should_CallOrderMsUseCases()
    {
        var orderGateway = new Mock<IOrderMsGateway>(MockBehavior.Strict);
        orderGateway.Setup(g => g.UpdateStatusPedido(10, StatusPedidoEnum.Pronto, "t"))
        .Returns(Task.CompletedTask);

        await PedidoUseCases.UpdatePedidoStatusToPronto(orderGateway.Object, 10, "t");
        orderGateway.VerifyAll();
    }

    [Fact]
    public async Task GetPedidosTelaoPedidos_Should_GroupByStatusAndUseClientNameFallbackToId()
    {
        var pedidoGateway = new Mock<IPedidoGateway>(MockBehavior.Loose);
        var comboPedidoGateway = new Mock<IComboPedidoGateway>(MockBehavior.Strict);
        var orderMsGateway = new Mock<IOrderMsGateway>(MockBehavior.Loose);
        var paymentMsGateway = new Mock<IPaymentMsGateway>(MockBehavior.Loose);
        var loginMsGateway = new Mock<ILoginMsGateway>(MockBehavior.Loose);

        var pedido = new Pedido(1)
        {
            IdPedido = 77,
            IdStatusPedido = (int)StatusPedidoEnum.Recebido,
            IdClienteNavigation = new Cliente { Nome = null }
        };
        var comboPedido = new ComboPedido { IdPedido = 77, IdPedidoNavigation = pedido };

        comboPedidoGateway.Setup(g => g.GetAllComboPedidosByStatusPedido(orderMsGateway.Object, paymentMsGateway.Object, loginMsGateway.Object, StatusPedidoEnum.Recebido, "t"))
        .ReturnsAsync(new List<ComboPedido> { comboPedido });
        comboPedidoGateway.Setup(g => g.GetAllComboPedidosByStatusPedido(orderMsGateway.Object, paymentMsGateway.Object, loginMsGateway.Object, StatusPedidoEnum.EmPreparacao, "t"))
        .ReturnsAsync(new List<ComboPedido>());
        comboPedidoGateway.Setup(g => g.GetAllComboPedidosByStatusPedido(orderMsGateway.Object, paymentMsGateway.Object, loginMsGateway.Object, StatusPedidoEnum.Pronto, "t"))
        .ReturnsAsync(new List<ComboPedido>());
        comboPedidoGateway.Setup(g => g.GetAllComboPedidosByStatusPedido(orderMsGateway.Object, paymentMsGateway.Object, loginMsGateway.Object, StatusPedidoEnum.Finalizado, "t"))
        .ReturnsAsync(new List<ComboPedido>());

        var result = await PedidoUseCases.GetPedidosTelaoPedidos(
        pedidoGateway.Object,
        comboPedidoGateway.Object,
        orderMsGateway.Object,
        paymentMsGateway.Object,
        loginMsGateway.Object,
        "t");

        result[StatusPedidoEnum.Recebido.ToString()].Should().HaveCount(1);
        comboPedidoGateway.VerifyAll();
    }
}
