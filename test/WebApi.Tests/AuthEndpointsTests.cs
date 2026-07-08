using System.Net;
using System.Net.Http.Json;
using Ahva.Ceplan.Contracts.Auth;
using Aspire.Hosting;
using Aspire.Hosting.Testing;

namespace Ahva.Ceplan.WebApi.Tests;

public class AuthEndpointsTests : IAsyncLifetime
{
    private DistributedApplication _app = null!;
    private HttpClient _client = null!;

    public async Task InitializeAsync()
    {
        var builder = await DistributedApplicationTestingBuilder.CreateAsync<Projects.Ceplan_AppHost>();

        _app = await builder.BuildAsync();
        await _app.StartAsync();

        await _app.ResourceNotifications.WaitForResourceHealthyAsync("webapi")
            .WaitAsync(TimeSpan.FromMinutes(5));

        _client = _app.CreateHttpClient("webapi");
    }

    public async Task DisposeAsync()
    {
        _client?.Dispose();
        if (_app is not null)
            await _app.DisposeAsync();
    }

    [Fact]
    public async Task Login_WithSeededUser_ReturnsTokenAndProfile()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/login", new
        {
            documentType = "DNI",
            documentNumber = "07079879",
            password = "Ceplan#2026",
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<LoginEnvelope>();

        Assert.NotNull(body);
        Assert.True(body.Ok);
        Assert.NotEmpty(body.Data.Token.AccessToken);
        Assert.Equal("Mendoza Quispe, July Camila", body.Data.User.FullName);
    }

    [Fact]
    public async Task Login_WithWrongPassword_ReturnsUnauthorizedError()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/login", new
        {
            documentType = "DNI",
            documentNumber = "07079879",
            password = "definitely-wrong",
        });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    private sealed class LoginEnvelope
    {
        public bool Ok { get; set; }

        public required LoginOutput Data { get; set; }
    }
}
