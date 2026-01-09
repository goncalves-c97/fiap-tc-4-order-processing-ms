using Core.Enums;
using Core.Interfaces.Gateways.Microservices;
using Core.UseCases.Microservices;
using FluentAssertions;
using Moq;

namespace Test.Core.UseCases.Microservices;

public class OrderMsUseCasesTests
{
    [Fact]
    public async Task IniciaPedido_Should_ValidateArgs()
    {
        Func<Task> act1 = async () => await OrderMsUseCases.IniciaPedido(null!, "t");
        await act1.Should().ThrowAsync<ArgumentNullException>();

        var gw = new Mock<IOrderMsGateway>();
        Func<Task> act2 = async () => await OrderMsUseCases.IniciaPedido(gw.Object, "");
        await act2.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task IniciaPedido_Should_CallGateway()
    {
        var gw = new Mock<IOrderMsGateway>(MockBehavior.Strict);
        gw.Setup(g => g.IniciaPedido("t")).ReturnsAsync(10);

        var result = await OrderMsUseCases.IniciaPedido(gw.Object, "t");
        result.Should().Be(10);
        gw.VerifyAll();
    }

    [Fact]
    public async Task UpdateStatusPedido_Should_ValidateArgs()
    {
        var gw = new Mock<IOrderMsGateway>();

        Func<Task> act1 = async () => await OrderMsUseCases.UpdateStatusPedido(null!, 1, StatusPedidoEnum.Recebido, "t");
        await act1.Should().ThrowAsync<ArgumentNullException>();

        Func<Task> act2 = async () => await OrderMsUseCases.UpdateStatusPedido(gw.Object, 0, StatusPedidoEnum.Recebido, "t");
        await act2.Should().ThrowAsync<ArgumentException>();

        Func<Task> act3 = async () => await OrderMsUseCases.UpdateStatusPedido(gw.Object, 1, StatusPedidoEnum.Recebido, "");
        await act3.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task UpdateStatusPedido_Should_CallGateway()
    {
        var gw = new Mock<IOrderMsGateway>(MockBehavior.Strict);
        gw.Setup(g => g.UpdateStatusPedido(1, StatusPedidoEnum.Pronto, "t")).Returns(Task.CompletedTask);

        await OrderMsUseCases.UpdateStatusPedido(gw.Object, 1, StatusPedidoEnum.Pronto, "t");
        gw.VerifyAll();
    }
}
