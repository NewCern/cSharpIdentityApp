using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace IdentityApp.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Credential Credential { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            //verify credential
            if(Credential.UserName == "admin" && Credential.Password == "password")
            {
                //create security context
                // Create Claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "admin"),
                    new Claim(ClaimTypes.Email, "admin@mywebsite.com"),
                    new Claim("Department", "HR"),
                    new Claim("Admin", "true"),
                    new Claim("Manager", "true"),
                };
                // this is the security context
                // Add "claims" to identity
                // specify authentication type "MyCookieAuth"
                var identity = new ClaimsIdentity(claims, "MyCookieAuth");

                //security context is now within the claims principle
                ClaimsPrincipal claimsPrinciple = new ClaimsPrincipal(identity);

                // 1.next we are going to serialize the context with "HttpContext.SignInAsync"
                // 2.Provide Authentication type "MyCookieAuth"
                // 3.And Claims principle, which encapsulates the "identity"

                // ON Sign In --> "HttpContext" is triggered --> Talks to service Injected in line 9 of Program.cs
                // Incrypted / serialized Cookie is sent to the browser
                await HttpContext.SignInAsync("MyCookieAuth", claimsPrinciple);
                return RedirectToPage("/Index");
            }
            return Page();
        }
    }

    public class Credential
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
