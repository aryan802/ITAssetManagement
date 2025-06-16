using ITAssetManagement.Data;
using ITAssetManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITAssetManagement.Pages.ActivityLogs
{
    [Authorize(Roles = "Admin,ITStaff")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<ActivityLog> Logs { get; set; }

        public async Task OnGetAsync()
        {
            Logs = await _context.ActivityLogs
                        .OrderByDescending(l => l.Timestamp)
                        .ToListAsync();
        }
    }
}

