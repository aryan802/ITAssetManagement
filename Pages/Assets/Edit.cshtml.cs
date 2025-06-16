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
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Asset Asset { get; set; } = default!;

        [BindProperty]
        public List<int> SelectedVendorIds { get; set; } = new();

        public SelectList EmployeeList { get; set; } = default!;
        public MultiSelectList VendorList { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            Asset = await _context.Assets.FindAsync(id);
            if (Asset == null) return NotFound();

            EmployeeList = new SelectList(
                await _context.Employees.ToListAsync(),
                "EmployeeID", "FullName", Asset.AssignedTo
            );

            SelectedVendorIds = await _context.AssetVendors
                .Where(av => av.AssetID == id)
                .Select(av => av.VendorID)
                .ToListAsync();

            VendorList = new MultiSelectList(
                await _context.Vendors.ToListAsync(),
                "VendorID", "Name", SelectedVendorIds
            );

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                EmployeeList = new SelectList(
                    await _context.Employees.ToListAsync(),
                    "EmployeeID", "FullName", Asset.AssignedTo
                );

                VendorList = new MultiSelectList(
                    await _context.Vendors.ToListAsync(),
                    "VendorID", "Name", SelectedVendorIds
                );

                return Page();
            }

            _context.Attach(Asset).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            // Clear old links
            var existingLinks = _context.AssetVendors.Where(av => av.AssetID == Asset.AssetID);
            _context.AssetVendors.RemoveRange(existingLinks);

            // Add new links
            foreach (var vendorId in SelectedVendorIds)
            {
                _context.AssetVendors.Add(new AssetVendor
                {
                    AssetID = Asset.AssetID,
                    VendorID = vendorId
                });
            }

            await _context.SaveChangesAsync();
            _context.Assets.Add(Asset);
            await _context.SaveChangesAsync();

            // Log creation
            _context.ActivityLogs.Add(new ActivityLog
            {
                Action = "Created Asset",
                UserName = User.Identity?.Name,
                AssetID = Asset.AssetID,
                Details = $"Asset '{Asset.Name}' created"
            });
            await _context.SaveChangesAsync();


            return RedirectToPage("./Index");
        }
    }
}
