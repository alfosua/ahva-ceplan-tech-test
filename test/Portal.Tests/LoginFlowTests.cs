using Aspire.Hosting;
using Aspire.Hosting.Testing;
using Microsoft.Playwright;

namespace Ahva.Ceplan.Portal.Tests;

public class LoginFlowTests : IAsyncLifetime
{
    private DistributedApplication _app = null!;
    private IPlaywright _playwright = null!;
    private IBrowser _browser = null!;
    private string _portalUrl = null!;

    public async Task InitializeAsync()
    {
        var builder = await DistributedApplicationTestingBuilder.CreateAsync<Projects.Ceplan_AppHost>();

        _app = await builder.BuildAsync();
        await _app.StartAsync();

        await _app.ResourceNotifications.WaitForResourceHealthyAsync("portal")
            .WaitAsync(TimeSpan.FromMinutes(5));

        _portalUrl = _app.GetEndpoint("portal", "https").ToString().TrimEnd('/');

        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new()
        {
            ExecutablePath = Environment.GetEnvironmentVariable("CEPLAN_E2E_BROWSER") is { Length: > 0 } path ? path : null,
        });
    }

    public async Task DisposeAsync()
    {
        if (_browser is not null)
            await _browser.DisposeAsync();
        _playwright?.Dispose();
        if (_app is not null)
            await _app.DisposeAsync();
    }

    private Task<IBrowserContext> NewContext() =>
        _browser.NewContextAsync(new() { IgnoreHTTPSErrors = true });

    [Fact]
    public async Task Login_WithValidCredentials_ShowsUserProfile()
    {
        await using var context = await NewContext();
        var page = await context.NewPageAsync();

        await page.GotoAsync($"{_portalUrl}/Login");
        await page.FillAsync("#login-user", "07079879");
        await page.FillAsync("#login-password", "Ceplan#2026");
        await page.ClickAsync("#login-submit");

        await page.WaitForURLAsync("**/Perfil");

        await Assertions.Expect(page.Locator(".profile-name")).ToHaveTextAsync("Mendoza Quispe, July Camila");
        await Assertions.Expect(page.Locator(".badge-active")).ToHaveTextAsync("Activo");
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_ShowsFieldErrors()
    {
        await using var context = await NewContext();
        var page = await context.NewPageAsync();

        await page.GotoAsync($"{_portalUrl}/Login");

        // The submit button only becomes enabled after both fields are filled (client-side JS).
        await Assertions.Expect(page.Locator("#login-submit")).ToBeDisabledAsync();

        await page.FillAsync("#login-user", "07079879");
        await page.FillAsync("#login-password", "wrong-password");
        await Assertions.Expect(page.Locator("#login-submit")).ToBeEnabledAsync();

        await page.ClickAsync("#login-submit");

        await Assertions.Expect(page.Locator(".field-error").First).ToContainTextAsync("Usuario incorrecto");
        await Assertions.Expect(page.Locator(".field-error").Last).ToContainTextAsync("Contraseña incorrecta");
    }
}
