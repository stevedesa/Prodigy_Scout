using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProdigyScout.Data;
using ProdigyScout.Interfaces;
using ProdigyScout.Models;
using ProdigyScout.ViewModels;
using SendGrid.Helpers.Mail;

namespace ProdigyScout.Controllers
{
    [Authorize]
    public class ProspectsController : Controller
    {
        //private readonly ProdigyScoutContext _context;
        /*public ProspectsController(ProdigyScoutContext context)
        {
            _context = context;
        }*/

        private readonly IStudentRepository _studentRepository;

        public ProspectsController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
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

            var students = await _studentRepository.GetStudents(FirstName, LastName, GPA, GradYear, sortOrder);

            var studentsVM = new StudentViewModel
            {
                Students = students
            };

            ViewData["CurrentSort"] = sortOrder;

            return View(studentsVM);
        }

        // GET: Prospects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _studentRepository.GetStudentByID(id.Value);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Prospects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Prospects/Create
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

                var student = await _studentRepository.InsertStudent(studentViewModel);

                if (student == null)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(studentViewModel);
        }

        // GET: Prospects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _studentRepository.GetStudentByID(id.Value);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Prospects/Edit/5
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
                var student = await _studentRepository.UpdateStudent(studentViewModel);

                if (student == null)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(studentViewModel);
        }

        // GET: Prospects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _studentRepository.GetStudentByID(id.Value);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Prospects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _studentRepository.DeleteStudent(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
