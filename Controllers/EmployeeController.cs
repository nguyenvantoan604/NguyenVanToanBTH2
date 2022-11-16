
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DemoMvc.Data;
// using DemoMvc.Models.Process;
using DemoMvc.Models;

namespace DemoMvc.Controllers
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
            var model = await _context.Employees.ToListAsync();
            return View(model);
        }
         public IActionResult Create()
        {
            return View();
        }
         [HttpPost]

         //Kiem tra xem ton tai database theo id khong
        private bool EmployeeExists(string id)
        {
            return _context.Employees.Any(e => e.EmpID== id);
        }
//delete
public async Task<IActionResult> Delete(string id)
        {
           if (id == null)
           {
               //return NotFound();
               return View("NotFound");
           }
           var std = await _context.Employees
               .FirstOrDefaultAsync(m => m.EmpID == id);
           if (std == null)
           {
               //return NotFound();
               return View("NotFound");
           }
              return View(std);
           }


        //POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> DeleteConfirmed(string id)
        {
           var std = await _context.Employees.FindAsync(id);
           _context.Employees.Remove(std);
           await _context.SaveChangesAsync();
           return RedirectToAction(nameof(Index));
        }
//Edit
public async Task<IActionResult> Edit(string id)
        {
           if (id == null)
           {
            //    return NotFound();
               return View("NotFound");
           }

           var employee = await _context.Employees.FindAsync(id);
           if (employee == null)
           {
            //    return NotFound();
               return View("NotFound");

           }
           return View(employee);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("EmpID,EmpName")] Employee std)
        {
           if (id != std.EmpID)
           {
            //    return NotFound();
               return View("NotFound");
           }
           if (ModelState.IsValid)
           {
               try
               {
                   _context.Update(std);
                   await _context.SaveChangesAsync();
               }
               catch (DbUpdateConcurrencyException)
               {
                   if (!EmployeeExists(std.EmpID))
                   {
                    //    return NotFound();
                       return View("NotFound");
                   }
                   else
                   {
                       throw;
                   }
               }
               return RedirectToAction(nameof(Index));
           }
           return View(std);
        }
//Upload
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
                if(fileExtension != ".xls" && fileExtension != ".xlsx")
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

                            emp.EmpID = dt.Rows[i][0].ToString();
                            emp.EmpName = dt.Rows[i][1].ToString();
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

