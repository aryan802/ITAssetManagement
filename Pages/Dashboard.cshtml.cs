using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ITAssetManagement.Data;
using ITAssetManagement.Models;

namespace ITAssetManagement.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly ApplicationDbContext _context;



        public DashboardModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public int TotalAssets { get; set; }
        public int AssignedAssets { get; set; }
        public int UnassignedAssets { get; set; }

        public List<string> StatusLabels { get; set; } = new();
        public List<int> StatusCounts { get; set; } = new();
        public List<Asset> ExpiringSoonAssets { get; set; }

        public async Task OnGetAsync()
        {
            TotalAssets = await _context.Assets.CountAsync();
            AssignedAssets = await _context.Assets.CountAsync(a => a.AssignedTo != null);
            UnassignedAssets = TotalAssets - AssignedAssets;

            var groupedStatus = await _context.Assets
                .GroupBy(a => a.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync();

            foreach (var status in groupedStatus)
            {
                StatusLabels.Add(status.Status ?? "Unknown");
                StatusCounts.Add(status.Count);
            }
            var today = DateTime.Today;
            var threshold = today.AddDays(30);

            ExpiringSoonAssets = await _context.Assets
                .Where(a => a.WarrantyExpiry <= threshold && a.WarrantyExpiry >= today)
                .OrderBy(a => a.WarrantyExpiry)
                .ToListAsync();
        }
    }
}

