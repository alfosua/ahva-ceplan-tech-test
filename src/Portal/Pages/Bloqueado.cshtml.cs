using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace Ahva.Ceplan.Portal.Pages;

public class BloqueadoModel(IOptions<SecurityOptions> security) : PageModel
{
    public int MaxRetries => security.Value.MaxRetries;

    public int BanMinutes => security.Value.BanTimeMinutes;

    public void OnGet()
    {
    }
}
