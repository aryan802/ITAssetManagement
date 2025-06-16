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
    public class IndexModel : PageModel
    {
        private readonly ITAssetManagement.Data.ApplicationDbContext _context;

        public IndexModel(ITAssetManagement.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Vendor> Vendor { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Vendor = await _context.Vendors.ToListAsync();
        }
    }
}
