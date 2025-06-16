using System;
using System.Collections.Generic;
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
    public class DetailsModel : PageModel
    {
        private readonly ITAssetManagement.Data.ApplicationDbContext _context;

        public DetailsModel(ITAssetManagement.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Asset Asset { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asset = await _context.Assets.FirstOrDefaultAsync(m => m.AssetID == id);

            ViewData["Vendors"] = await _context.AssetVendors
                .Where(av => av.AssetID == Asset.AssetID)
                .Include(av => av.Vendor)
                .Select(av => av.Vendor.Name)
                .ToListAsync();


            if (asset is not null)
            {
                Asset = asset;

                return Page();
            }

            return NotFound();
        }
    }
}
