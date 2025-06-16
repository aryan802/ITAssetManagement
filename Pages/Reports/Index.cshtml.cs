using ClosedXML.Excel;
using ITAssetManagement.Data;
using ITAssetManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ITAssetManagement.Pages.Reports
{
    [Authorize(Roles = "Admin,ITStaff")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public FilterModel Filter { get; set; } = new();

        public List<Asset> FilteredAssets { get; set; } = new();

        public SelectList EmployeeList { get; set; }

        public class FilterModel
        {
            public int? EmployeeId { get; set; }
            public string? Type { get; set; }
            public string? Status { get; set; }
        }

        public async Task OnGetAsync()
        {
            EmployeeList = new SelectList(await _context.Employees.ToListAsync(), "EmployeeID", "FullName");
        }

        public async Task<IActionResult> OnPostAsync(string? export)
        {
            var query = _context.Assets
                .Include(a => a.AssignedEmployee)
                .AsQueryable();

            if (Filter.EmployeeId != null)
                query = query.Where(a => a.AssignedTo == Filter.EmployeeId);
            if (!string.IsNullOrEmpty(Filter.Type))
                query = query.Where(a => a.Type.Contains(Filter.Type));
            if (!string.IsNullOrEmpty(Filter.Status))
                query = query.Where(a => a.Status.Contains(Filter.Status));

            FilteredAssets = await query.ToListAsync();

            EmployeeList = new SelectList(await _context.Employees.ToListAsync(), "EmployeeID", "FullName");

            if (export == "excel")
            {
                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Assets");

                worksheet.Cell(1, 1).Value = "Name";
                worksheet.Cell(1, 2).Value = "Type";
                worksheet.Cell(1, 3).Value = "Status";
                worksheet.Cell(1, 4).Value = "Purchase Date";
                worksheet.Cell(1, 5).Value = "Assigned To";

                for (int i = 0; i < FilteredAssets.Count; i++)
                {
                    var a = FilteredAssets[i];
                    worksheet.Cell(i + 2, 1).Value = a.Name;
                    worksheet.Cell(i + 2, 2).Value = a.Type;
                    worksheet.Cell(i + 2, 3).Value = a.Status;
                    worksheet.Cell(i + 2, 4).Value = a.PurchaseDate.ToShortDateString();
                    worksheet.Cell(i + 2, 5).Value = a.AssignedEmployee?.FullName ?? "";
                }

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0;
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AssetsReport.xlsx");
            }

            return Page();
        }
    }
}

