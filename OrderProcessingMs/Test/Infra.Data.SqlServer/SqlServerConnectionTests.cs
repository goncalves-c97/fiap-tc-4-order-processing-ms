using FluentAssertions;
using Test.Helpers;

namespace Test.Infra.Data.SqlServer;

public class SqlServerConnectionTests
{
    [Fact]
    public void Ctor_Should_SetConnectionString()
    {
        var conn = new SqlServerConnection("Server=(localdb)\\MSSQLLocalDB;Database=master;Trusted_Connection=True;");
        conn.ConnectionString.Should().Contain("Database=master");
    }

    [Fact]
    public async Task ExecuteRawSql_Should_Throw_WhenServerUnavailable()
    {
        // Use an invalid host to keep the test deterministic and fast.
        var conn = new SqlServerConnection("Server=invalid-host;Database=master;User Id=x;Password=y;TrustServerCertificate=True;");

        var act = () => conn.ExecuteRawSql("SELECT1");
        await act.Should().ThrowAsync<Exception>();
    }
}
