using System.Net;
using System.Security.Claims;
using Ahva.Ceplan.WebApi.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Refit;

namespace Ahva.Ceplan.Portal.Pages;

public class LoginModel(ICeplanAuthApi authApi, IOptions<SecurityOptions> security) : PageModel
{
    public class InputModel
    {
        public string DocumentType { get; set; } = "DNI";

        public string DocumentNumber { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public bool UserError { get; private set; }

    public bool PasswordError { get; private set; }

    public bool SessionExpired { get; private set; }

    public void OnGet(bool expired = false)
    {
        SessionExpired = expired;
    }

    public async Task<IActionResult> OnPost()
    {
        if (string.IsNullOrWhiteSpace(Input.DocumentNumber) || string.IsNullOrWhiteSpace(Input.Password))
        {
            UserError = string.IsNullOrWhiteSpace(Input.DocumentNumber);
            PasswordError = string.IsNullOrWhiteSpace(Input.Password);
            return Page();
        }

        try
        {
            var response = await authApi.Login(new()
            {
                DocumentType = Input.DocumentType,
                DocumentNumber = Input.DocumentNumber.Trim(),
                Password = Input.Password,
            });

            var user = response.Data.User;
            var token = response.Data.Token;

            var identity = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.FullName ?? user.Email),
                    new Claim(SessionClaims.JobTitle, user.JobTitle ?? string.Empty),
                    new Claim(SessionClaims.AccessToken, token.AccessToken),
                ],
                CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(new ClaimsPrincipal(identity), new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(security.Value.SessionExpirationMinutes),
            });

            return RedirectToPage("/Perfil");
        }
        catch (ApiException exception) when (exception.StatusCode == HttpStatusCode.Locked)
        {
            return RedirectToPage("/Bloqueado");
        }
        catch (ApiException exception) when (exception.StatusCode == HttpStatusCode.Unauthorized)
        {
            UserError = true;
            PasswordError = true;
            return Page();
        }
    }
}
