using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ProdigyScout.Interfaces;
using ProdigyScout.Models;
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
            var students = await _studentRepository.GetStudents(filterBy, searchTerm, sortOrder);
            var complexData = await _studentRepository.GetComplexData();

            var studentsVM = new StudentViewModel
            {
                Students = students,
                ComplexData = complexData,
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
                Degree = student.Degree,
                GraduationDate = student.GraduationDate.Date
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

                var NewStudentemail = await _studentRepository.InsertStudent(studentViewModel);

                if (NewStudentemail != null)
                {
                    ModelState.AddModelError(string.Empty, "This Email Already Exists in our Database");
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
                Degree = student.Degree,
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
                Degree = student.Degree,
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

        // POST: Prospects/MarkAsWatch/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsWatch(int id)
        {
            var success = await _studentRepository.MarkAsWatch(id);

            if (!success)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Prospects/MarkAsUnwatch/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsUnwatch(int id)
        {
            var success = await _studentRepository.MarkAsUnwatch(id);

            if (!success)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Prospects/MarkAsPipeline/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsPipeline(int id)
        {
            var success = await _studentRepository.MarkAsPipeline(id);

            if (!success)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Prospects/MarkAsNonPipeline/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsNonPipeline(int id)
        {
            var success = await _studentRepository.MarkAsNonPipeline(id);

            if (!success)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Prospects/AddOrUpdateComment/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrUpdateComment(int id, string comment)
        {
            var success = await _studentRepository.AddOrUpdateComment(id, comment);

            if (!success)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
