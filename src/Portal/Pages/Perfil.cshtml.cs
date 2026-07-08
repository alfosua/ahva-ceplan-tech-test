using System.Net;
using System.Security.Claims;
using Ahva.Ceplan.Contracts.Users;
using Ahva.Ceplan.WebApi.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Refit;

namespace Ahva.Ceplan.Portal.Pages;

[Microsoft.AspNetCore.Authorization.Authorize]
public class PerfilModel(ICeplanAuthApi authApi) : PageModel
{
    public UserOutput Profile { get; private set; } = null!;

    public async Task<IActionResult> OnGet()
    {
        var token = User.FindFirstValue(SessionClaims.AccessToken);
        if (token is null)
            return await SignOutToLogin();

        try
        {
            var response = await authApi.Me(token);
            Profile = response.Data;

            return Page();
        }
        catch (ApiException exception) when (exception.StatusCode == HttpStatusCode.Unauthorized)
        {
            return await SignOutToLogin();
        }
    }

    private async Task<IActionResult> SignOutToLogin()
    {
        await HttpContext.SignOutAsync();

        return RedirectToPage("/Login", new { expired = true });
    }
}
