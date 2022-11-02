using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NguyenVanToanBTH2.Data;
using NguyenVanToanBTH2.Models.Process;
using NguyenVanToanBTH2.Models;

namespace NguyenVanToanBTH2.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        private ExcelProcess _excelProcess = new ExcelProcess();
        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Employees.ToListAsync());
        }
        private bool EmployeeExists(string id)
        {
            return _context.Employees.Any(e => e.EmployeeId== id);
        }

        public async Task<IActionResult> Upload()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file != null)
            {
                string fileExtension = Path.GetExtension(file.FileName);
                if(fileExtension != "xls" && fileExtension != "xlsx")
                {
                    ModelState.AddModelError("", "Please choose ecexl file to upload!");

                }
                else
                {
                    var fileName = DateTime.Now.ToShortTimeString() + fileExtension;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory() + "/Uploads" ,fileName);
                    var fileLocation = new FileInfo(filePath).ToString();
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);

                        var dt = _excelProcess.ExcelToDataTable(fileLocation);

                        for (int i = 0; i< dt.Rows.Count; i++)
                        {
                            var emp = new Employee();

                            emp.EmployeeId = dt.Rows[i][0].ToString();
                            emp.EmployeeName = dt.Rows[i][1].ToString();
                            emp.Address = dt.Rows[i][2].ToString();

                            _context.Employees.Add(emp);
                        }
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            return View();

        }
    }
}

