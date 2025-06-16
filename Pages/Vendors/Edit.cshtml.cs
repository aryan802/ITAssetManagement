using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ITAssetManagement.Data;
using ITAssetManagement.Models;
using Microsoft.AspNetCore.Authorization;

namespace ITAssetManagement.Pages.Vendors
{
    [Authorize(Roles = "Admin,ITStaff")]
    public class EditModel : PageModel
    {
        private readonly ITAssetManagement.Data.ApplicationDbContext _context;

        public EditModel(ITAssetManagement.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Vendor Vendor { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendor =  await _context.Vendors.FirstOrDefaultAsync(m => m.VendorID == id);
            if (vendor == null)
            {
                return NotFound();
            }
            Vendor = vendor;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Vendor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VendorExists(Vendor.VendorID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool VendorExists(int id)
        {
            return _context.Vendors.Any(e => e.VendorID == id);
        }
    }
}
