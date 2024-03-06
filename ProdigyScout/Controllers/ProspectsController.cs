using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProdigyScout.Data;
using ProdigyScout.Models;
using ProdigyScout.ViewModels;

namespace ProdigyScout.Controllers
{
    [Authorize]
    public class ProspectsController : Controller
    {
        private readonly ProdigyScoutContext _context;

        public ProspectsController(ProdigyScoutContext context)
        {
            _context = context;
        }

        // GET: Prospects
        public async Task<IActionResult> Index(string First, string Last, string GradePoint, string GradYear)
        {
            if (_context.Prospect == null)
            {
                return Problem("Entity set 'MvcMovieContext.Movie'  is null.");
            }

            var Student = from m in _context.Prospect
                         select m;

            //Allows the user to filter by First Name, Last Name, and GPA Floor
            if (!String.IsNullOrEmpty(First) && !String.IsNullOrEmpty(Last) && !String.IsNullOrEmpty(GradePoint))
            {
                Student = Student.Where(s => s.FirstName!.Contains(First) && s.LastName!.Contains(Last) && s.GPA > float.Parse(GradePoint));
                Student = Student.OrderByDescending(s => s.GPA);
            }
            else
            {
                if (!String.IsNullOrEmpty(First))
                {
                    Student = Student.Where(s => s.FirstName!.Contains(First));
                }
                if (!String.IsNullOrEmpty(Last))
                {
                    Student = Student.Where(s => s.LastName!.Contains(Last));
                }
                if (!String.IsNullOrEmpty(GradePoint))
                {
                    Student = Student.Where(s => s.GPA > float.Parse(GradePoint));
                    Student = Student.OrderByDescending(s => s.GPA);
                }
            }

            //Search By Graduation Date
            if (!String.IsNullOrEmpty(GradYear))
            {
                Student = Student.Where(s => s.GraduationDate > DateTime.Parse(GradYear));
                Student = Student.OrderBy(s => s.GraduationDate);
            }

            var studentsVM = new StudentViewModel
            {
                Students = await Student.ToListAsync()
            };

            return View(studentsVM);
        }

        // GET: Prospects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            StudentViewModel studentViewModel;

            if (id == null || _context.Prospect == null)
            {
                return NotFound();
            }

            var prospect = await _context.Prospect.FirstOrDefaultAsync(m => m.Id == id);

            if (prospect == null)
            {
                return NotFound();
            }
            else
            {
                studentViewModel = new(prospect);
            }

            return View(studentViewModel);
        }

        // GET: Prospects/Create
        public IActionResult Create()
        {
            StudentViewModel studentViewModel = new();

            return View(studentViewModel);
        }

        // POST: Prospects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,email,Gender,GPA,GraduationDate")] StudentViewModel studentViewModel)
        {
            if (ModelState.IsValid)
            {
                if (!studentViewModel.email.EndsWith(".com"))
                {
                    ModelState.AddModelError(string.Empty, "Only Email IDs from .com Domains are allowed.");
                    return View(studentViewModel);
                }

                Prospect prospect = new()
                {
                    FirstName = studentViewModel.FirstName.Trim(),
                    LastName = studentViewModel.LastName.Trim(),
                    email = studentViewModel.email.Trim(),
                    Gender = studentViewModel.Gender.Trim(),
                    GPA = studentViewModel.GPA,
                    GraduationDate = studentViewModel.GraduationDate.Date
                };
                _context.Add(prospect);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(studentViewModel);
        }

        // GET: Prospects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            StudentViewModel studentViewModel;

            if (id == null)
            {
                return NotFound();
            }

            var prospect = await _context.Prospect.FindAsync(id);

            if (prospect == null)
            {
                return NotFound();
            }
            else
            {
                studentViewModel = new(prospect);
            }

            return View(studentViewModel);
        }

        // POST: Prospects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,email,Gender,GPA,GraduationDate")] StudentViewModel studentViewModel)
        {
            if (id != studentViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Prospect prospect = await _context.Prospect.FindAsync(studentViewModel.Id);

                    if (prospect == null)
                    {
                        return NotFound();
                    }

                    if (!studentViewModel.email.EndsWith(".com"))
                    {
                        ModelState.AddModelError(string.Empty, "Only Email IDs from .com Domains are allowed.");
                        return View(studentViewModel);
                    }

                    prospect.FirstName = studentViewModel.FirstName.Trim();
                    prospect.LastName = studentViewModel.LastName.Trim();
                    prospect.email = studentViewModel.email.Trim();
                    prospect.Gender = studentViewModel.Gender.Trim();
                    prospect.GPA = studentViewModel.GPA;
                    prospect.GraduationDate = studentViewModel.GraduationDate;

                    _context.Update(prospect);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProspectExists(studentViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(studentViewModel);
        }

        // GET: Prospects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            StudentViewModel studentViewModel;

            if (id == null)
            {
                return NotFound();
            }

            var prospect = await _context.Prospect.FirstOrDefaultAsync(m => m.Id == id);

            if (prospect == null)
            {
                return NotFound();
            }
            else
            {
                studentViewModel = new(prospect);
            }

            return View(studentViewModel);
        }

        // POST: Prospects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prospect = await _context.Prospect.FindAsync(id);
            if (prospect != null)
            {
                _context.Prospect.Remove(prospect);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProspectExists(int id)
        {
            return _context.Prospect.Any(e => e.Id == id);
        }
    }
}
