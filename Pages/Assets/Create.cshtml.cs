using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITAssetManagement.Data;
using ITAssetManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace ITAssetManagement.Pages.Assets
{
    [Authorize(Roles = "Admin,ITStaff")]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Asset Asset { get; set; } = default!;

        [BindProperty]
        public List<int> SelectedVendorIds { get; set; } = new List<int>();

        public SelectList EmployeeList { get; set; } = default!;
        public MultiSelectList VendorList { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            EmployeeList = new SelectList(
                await _context.Employees.OrderBy(e => e.FullName).ToListAsync(),
                "EmployeeID", "FullName"
            );

            VendorList = new MultiSelectList(
                await _context.Vendors.OrderBy(v => v.Name).ToListAsync(),
                "VendorID", "Name"
            );

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Repopulate dropdowns if validation fails
                EmployeeList = new SelectList(
                    await _context.Employees.OrderBy(e => e.FullName).ToListAsync(),
                    "EmployeeID", "FullName"
                );

                VendorList = new MultiSelectList(
                    await _context.Vendors.OrderBy(v => v.Name).ToListAsync(),
                    "VendorID", "Name"
                );

                return Page();
            }

            // ✅ Save the Asset (without manually setting AssetID)
            _context.Assets.Add(Asset);
            await _context.SaveChangesAsync(); // AssetID is auto-generated here

            // ✅ Add associated vendors
            foreach (var vendorId in SelectedVendorIds)
            {
                var assetVendor = new AssetVendor
                {
                    AssetID = Asset.AssetID,
                    VendorID = vendorId
                };
                _context.AssetVendors.Add(assetVendor);
            }

            // ✅ Log creation
            _context.ActivityLogs.Add(new ActivityLog
            {
                Action = "Created Asset",
                UserName = User.Identity?.Name,
                AssetID = Asset.AssetID,
                Details = $"Asset '{Asset.Name}' created"
            });

            await _context.SaveChangesAsync(); // Save vendors + logs

            return RedirectToPage("./Index");
        }
    }
}



