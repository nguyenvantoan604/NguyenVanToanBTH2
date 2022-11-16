using Microsoft.AspNetCore.Mvc;
using DemoMvc.Data;
using DemoMvc.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
// using DemoMvc.Models.Process;

namespace DemoMvc.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;
        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var model = await _context.Students.ToListAsync();
            return View(model);
        }
        public IActionResult Create()
        {

            var newStudentID = "STD001";
            var countStudent = _context.Students.Count();
            // if(countStudent>0){
            //     var studentID = _context.Students.OrderDescending<m => m.
            // }

            ViewData["FacultyID"] = new SelectList(_context.Faculty, "FacultyID", "FacultyName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentID,StudentName,FacultyID")]Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // return View(student);
        
        ViewData["FacultyID"] = new SelectList(_context.Faculty,"FacultyID", "FacultyName",student.FacultyID);
        return View(student);
        }

        //Edit
          public async Task<IActionResult> Edit(string id)
        {
           if (id == null)
           {
            //    return NotFound();
               return View("NotFound");
           }

           var student = await _context.Students.FindAsync(id);
           if (student == null)
           {
            //    return NotFound();
               return View("NotFound");

           }
           return View(student);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("StudentID,StudentName")] Student std)
        {
           if (id != std.StudentID)
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
                //    return RedirectToAction(nameof(Index));
               }
               catch (DbUpdateConcurrencyException)
               {
                   if (!StudentExists(std.StudentID))
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

        private bool StudentExists(string id)
        {
           return _context.Students.Any(e => e.StudentID == id);
        }




        //GET: Product/Delete/5

        public async Task<IActionResult> Delete(string id)
        {
           if (id == null)
           {
               //return NotFound();
               return View("NotFound");
           }
           var std = await _context.Students
               .FirstOrDefaultAsync(m => m.StudentID == id);
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
           var std = await _context.Students.FindAsync(id);
           _context.Students.Remove(std);
           await _context.SaveChangesAsync();
           return RedirectToAction(nameof(Index));
        }

    }
    
    
}