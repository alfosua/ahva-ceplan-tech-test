using System.Security.Claims;
using Ahva.Ceplan.Portal;
using Ahva.Ceplan.WebApi.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

var securitySection = builder.Configuration.GetSection(SecurityOptions.SectionName);
builder.Services.Configure<SecurityOptions>(securitySection);

var security = securitySection.Get<SecurityOptions>() ?? new SecurityOptions();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.AccessDeniedPath = "/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(security.SessionExpirationMinutes);
        options.SlidingExpiration = false;
    });
builder.Services.AddAuthorization();

builder.Services.AddCeplanApiClient(client => client.BaseAddress = new Uri("https+http://webapi"));

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.MapPost("/session/extend", async (HttpContext context, ICeplanAuthApi authApi) =>
{
    var token = context.User.FindFirstValue(SessionClaims.AccessToken);
    if (token is null)
        return Results.Unauthorized();

    var response = await authApi.Extend(token);

    var identity = new ClaimsIdentity(context.User.Claims, CookieAuthenticationDefaults.AuthenticationScheme);
    identity.RemoveClaim(identity.FindFirst(SessionClaims.AccessToken));
    identity.AddClaim(new Claim(SessionClaims.AccessToken, response.Data.AccessToken));

    await context.SignInAsync(new ClaimsPrincipal(identity));

    return Results.Json(new { ok = true, expiresIn = response.Data.ExpiresIn });
}).RequireAuthorization();

app.MapPost("/session/logout", async (HttpContext context) =>
{
    await context.SignOutAsync();

    return Results.Json(new { ok = true });
});

app.MapDefaultEndpoints();

app.Run();
