using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITAssetManagement.Data;
using ITAssetManagement.Models;
using Microsoft.AspNetCore.Authorization;

namespace ITAssetManagement.Pages.Assets
{
    [Authorize(Roles = "Admin,ITStaff")]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Asset Asset { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var asset = await _context.Assets.FirstOrDefaultAsync(m => m.AssetID == id);

            if (asset == null)
                return NotFound();

            Asset = asset;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var asset = await _context.Assets.FindAsync(id);
            if (asset != null)
            {
                // Delete related vendor links
                var vendorLinks = _context.AssetVendors.Where(av => av.AssetID == asset.AssetID);
                _context.AssetVendors.RemoveRange(vendorLinks);

                // Delete the asset
                _context.Assets.Remove(asset);

                // Log deletion
                _context.ActivityLogs.Add(new ActivityLog
                {
                    Action = "Deleted Asset",
                    UserName = User.Identity?.Name,
                    AssetID = asset.AssetID,
                    Details = $"Asset '{asset.Name}' deleted"
                });

                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

