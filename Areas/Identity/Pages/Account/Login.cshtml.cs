using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using ITAssetManagement.Models; // ⬅ Make sure this is included

namespace ITAssetManagement.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LoginModel(SignInManager<ApplicationUser> signInManager)
            => _signInManager = signInManager;

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [EmailAddress, Required]
            public string Email { get; set; }

            [DataType(DataType.Password), Required]
            public string Password { get; set; }

            public bool RememberMe { get; set; }
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var result = await _signInManager.PasswordSignInAsync(
                Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
                return RedirectToPage("/Dashboard");

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }
    }
}


