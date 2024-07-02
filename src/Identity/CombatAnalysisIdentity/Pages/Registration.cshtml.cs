using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CombatAnalysisIdentity.Pages;

public class RegistrationModel : PageModel
{
    public int Step { get; set; }

    public void OnGet()
    {
    }

    public void HandleClick()
    {
        Step++;
    }
}
