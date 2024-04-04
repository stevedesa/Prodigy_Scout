using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProdigyScout.Interfaces;
using ProdigyScout.ViewModels;

namespace ProdigyScout.Controllers
{
    [Authorize]
    public class ProspectsController : Controller
    {
        private readonly IStudentRepository _studentRepository;

        public ProspectsController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        // GET: Prospects
        public async Task<IActionResult> Index(string filterBy, string searchTerm, string sortOrder)
        {
            ViewData["FirstNameSortParm"] = sortOrder == "FirstName" ? "FirstName_desc" : "FirstName";
            ViewData["LastNameSortParm"] = sortOrder == "LastName" ? "LastName_desc" : "LastName";
            ViewData["GPASortParm"] = sortOrder == "GPA" ? "GPA_desc" : "GPA";
            ViewData["GraduationDateSortParm"] = sortOrder == "GraduationDate" ? "GraduationDate_desc" : "GraduationDate";

            var students = await _studentRepository.GetStudents(filterBy, searchTerm, sortOrder);

            var studentsVM = new StudentViewModel
            {
                Students = students,
                FilterBy = filterBy,
                SearchTerm = searchTerm,
                CurrentSort = sortOrder
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

            var studentViewModel = new StudentViewModel
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                EmailID = student.Email,
                GPA = student.GPA,
                Gender = student.Gender,
                GraduationDate = student.GraduationDate
            };

            return View(studentViewModel);
        }

        // GET: Prospects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Prospects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,EmailID,Gender,GPA,GraduationDate")] StudentViewModel studentViewModel)
        {
            if (ModelState.IsValid)
            {
                if (!studentViewModel.EmailID.EndsWith(".com"))
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

            var studentViewModel = new StudentViewModel
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                EmailID = student.Email,
                GPA = student.GPA,
                Gender = student.Gender,
                GraduationDate = student.GraduationDate
            };

            return View(studentViewModel);
        }

        // POST: Prospects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,EmailID,Gender,GPA,GraduationDate")] StudentViewModel studentViewModel)
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

            var studentViewModel = new StudentViewModel
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                EmailID = student.Email,
                GPA = student.GPA,
                Gender = student.Gender,
                GraduationDate = student.GraduationDate
            };

            return View(studentViewModel);
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
