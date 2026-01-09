using Core.Constants;
using FluentAssertions;

namespace Test.Core.Constants;

public class UsuarioRolesTests
{
    [Fact]
    public void Roles_Should_BeNonEmpty()
    {
        UsuarioRoles.Administrador.Should().NotBeNullOrWhiteSpace();
        UsuarioRoles.Cozinheiro.Should().NotBeNullOrWhiteSpace();
        UsuarioRoles.Atendente.Should().NotBeNullOrWhiteSpace();
        UsuarioRoles.ClienteIdentificado.Should().NotBeNullOrWhiteSpace();
        UsuarioRoles.ClienteAnonimo.Should().NotBeNullOrWhiteSpace();
    }
}
