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
        public async Task<IActionResult> Index(StudentViewModel studentViewModel, string sortOrder)
        {
            string FirstName = studentViewModel.FirstNameSearch;
            string LastName = studentViewModel.LastNameSearch;
            string GPA = studentViewModel.GradePointSearch;
            string GradYear = studentViewModel.GradYearSearch;

            ViewData["FirstNameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "FirstName" : "";
            ViewData["LastNameSortParm"] = sortOrder == "LastName" ? "LastName_desc" : "LastName";
            ViewData["GPASortParm"] = sortOrder == "GPA" ? "GPA_desc" : "GPA";
            ViewData["GraduationDateSortParm"] = sortOrder == "GraduationDate" ? "GraduationDate_desc" : "GraduationDate";

            var studentQuery = _context.Prospect.AsQueryable();

            if (!String.IsNullOrEmpty(FirstName))
            {
                studentQuery = studentQuery.Where(s => s.FirstName!.Contains(FirstName));
            }
            if (!String.IsNullOrEmpty(LastName))
            {
                studentQuery = studentQuery.Where(s => s.LastName!.Contains(LastName));
            }
            if (!String.IsNullOrEmpty(GPA))
            {
                studentQuery = studentQuery.Where(s => s.GPA > float.Parse(GPA));
                studentQuery = studentQuery.OrderByDescending(s => s.GPA);
            }
            if (!String.IsNullOrEmpty(GradYear))
            {
                studentQuery = studentQuery.Where(s => s.GraduationDate > DateTime.Parse(GradYear));
                studentQuery = studentQuery.OrderBy(s => s.GraduationDate);
            }

            switch (sortOrder)
            {
                case "FirstName":
                    studentQuery = studentQuery.OrderBy(s => s.FirstName);
                    break;
                case "LastName_desc":
                    studentQuery = studentQuery.OrderByDescending(s => s.LastName);
                    break;
                case "LastName":
                    studentQuery = studentQuery.OrderBy(s => s.LastName);
                    break;
                case "GPA_desc":
                    studentQuery = studentQuery.OrderByDescending(s => s.GPA);
                    break;
                case "GPA":
                    studentQuery = studentQuery.OrderBy(s => s.GPA);
                    break;
                case "GraduationDate_desc":
                    studentQuery = studentQuery.OrderByDescending(s => s.GraduationDate);
                    break;
                case "GraduationDate":
                    studentQuery = studentQuery.OrderBy(s => s.GraduationDate);
                    break;
                default:
                    studentQuery = studentQuery.OrderByDescending(s => s.GPA);
                    break;
            }

            var studentsVM = new StudentViewModel
            {
                Students = await studentQuery.ToListAsync()
            };

            ViewData["CurrentSort"] = sortOrder;

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
