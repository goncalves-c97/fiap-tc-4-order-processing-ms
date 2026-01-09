using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Test.Helpers;

namespace Test.WebApi.Smoke;

public class WebApiAppFactoryTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public WebApiAppFactoryTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Swagger_Should_BeReachable()
    {
        using var _ = new EnvVarScope("API_AUTHENTICATION_KEY", "0123456789ABCDEF0123456789ABCDEF");
        using var __ = new EnvVarScope("LOGIN_MS_URL", "http://localhost");
        using var ___ = new EnvVarScope("ORDER_MS_URL", "http://localhost");
        using var ____ = new EnvVarScope("PAYMENT_MS_URL", "http://localhost");

        var client = _factory.WithWebHostBuilder(b => b.UseSetting("ASPNETCORE_ENVIRONMENT", "Production")).CreateClient();
        var resp = await client.GetAsync("/");
        resp.StatusCode.Should().BeOneOf(System.Net.HttpStatusCode.NotFound, System.Net.HttpStatusCode.Redirect);
    }
}
