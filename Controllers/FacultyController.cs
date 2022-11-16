using Microsoft.AspNetCore.Mvc;
using DemoMvc.Data;
using DemoMvc.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DemoMvc.Controllers
{
    public class FacultyController : Controller
    {
        private readonly  ApplicationDbContext _context;

        public FacultyController (ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _context.Faculty.ToListAsync();
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Faculty std)
        {
            if(ModelState.IsValid)
            {
                _context.Add(std);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            return View(std);
        }

        public bool FacultyExists(string id)
        {
            return _context.Faculty.Any(e => e.FacultyID == id);
        }


        // delete
            public async Task<IActionResult> Delete(string id)
        {
           if (id == null)
           {
               //return NotFound();
               return View("NotFound");
           }
           var std = await _context.Faculty
               .FirstOrDefaultAsync(m => m.FacultyID == id);
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
           var std = await _context.Faculty.FindAsync(id);
           _context.Faculty.Remove(std);
           await _context.SaveChangesAsync();
           return RedirectToAction(nameof(Index));
        }

    }
}