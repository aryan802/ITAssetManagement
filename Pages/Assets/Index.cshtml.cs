using System.Collections.Generic;
using System.Threading.Tasks;
using ITAssetManagement.Data;
using ITAssetManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ITAssetManagement.Pages.Assets
{
    [Authorize(Roles = "Admin,ITStaff")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public IndexModel(ApplicationDbContext context) => _context = context;

        // Renamed to 'Assets' for clarity
        public IList<Asset> Assets { get; set; } = new List<Asset>();

        public async Task OnGetAsync()
        {
            // Load assets with their assigned employee
            Assets = await _context.Assets
                                   .Include(a => a.AssignedEmployee)
                                   .ToListAsync();
        }
    }
}

