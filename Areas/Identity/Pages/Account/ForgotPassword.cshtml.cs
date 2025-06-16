using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ITAssetManagement.Areas.Identity.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public ForgotPasswordModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required, EmailAddress]
            public string Email { get; set; }
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
                return RedirectToPage("./ShowResetLink"); // Generic response

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetUrl = Url.Page(
                "/Account/ResetPassword",
                null,
                new { area = "Identity", code = token, email = Input.Email },
                Request.Scheme);

            TempData["ResetLink"] = resetUrl;
            return RedirectToPage("./ShowResetLink");
        }
    }
}
