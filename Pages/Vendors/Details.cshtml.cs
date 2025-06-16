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

namespace ITAssetManagement.Pages.Vendors
{
    [Authorize(Roles = "Admin,ITStaff")]
    public class DetailsModel : PageModel
    {
        private readonly ITAssetManagement.Data.ApplicationDbContext _context;

        public DetailsModel(ITAssetManagement.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Vendor Vendor { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendor = await _context.Vendors.FirstOrDefaultAsync(m => m.VendorID == id);

            if (vendor is not null)
            {
                Vendor = vendor;

                return Page();
            }

            return NotFound();
        }
    }
}
