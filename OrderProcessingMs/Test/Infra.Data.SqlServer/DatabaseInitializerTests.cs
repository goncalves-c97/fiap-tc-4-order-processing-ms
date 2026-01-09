using FluentAssertions;
using Infra.Data.SqlServer;

namespace Test.Infra.Data.SqlServer;

public class DatabaseInitializerTests
{
    [Fact]
    public void EnsureDatabaseExists_Should_Throw_ForInvalidConnectionString()
    {
        var act = () => DatabaseInitializer.EnsureDatabaseExists("Server=invalid-host;Database=master;User Id=x;Password=y;TrustServerCertificate=True;", "OrderProcessingDb");
        act.Should().Throw<Exception>();
    }
}
