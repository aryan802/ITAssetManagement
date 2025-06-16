using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ITAssetManagement.Models;

namespace ITAssetManagement.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class ManageUsersModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ManageUsersModel(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public List<UserWithRole> Users { get; set; }

        [BindProperty]
        public string SelectedRole { get; set; }

        [BindProperty]
        public string UserId { get; set; }

        public class UserWithRole
        {
            public string Id { get; set; }
            public string Email { get; set; }
            public string Role { get; set; }
        }

        public async Task OnGetAsync()
        {
            Users = new();
            var allUsers = _userManager.Users.ToList();

            foreach (var user in allUsers)
            {
                var roles = await _userManager.GetRolesAsync(user);
                Users.Add(new UserWithRole
                {
                    Id = user.Id,
                    Email = user.Email,
                    Role = roles.FirstOrDefault() ?? "None"
                });
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null) return NotFound();

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (!await _roleManager.RoleExistsAsync(SelectedRole))
                await _roleManager.CreateAsync(new IdentityRole(SelectedRole));

            await _userManager.AddToRoleAsync(user, SelectedRole);
            return RedirectToPage();
        }
    }
}


