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
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProspectsController(IStudentRepository studentRepository, IWebHostEnvironment hostingEnvironment)
        {
            _studentRepository = studentRepository;
            _hostingEnvironment = hostingEnvironment;
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
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,EmailID,Gender,GPA,GraduationDate,Degree,ResumeFile,ResumePath")] StudentViewModel studentViewModel)
        {
            if (ModelState.IsValid)
            {
                if (!studentViewModel.EmailID.EndsWith(".com"))
                {
                    ModelState.AddModelError(string.Empty, "Only Email IDs from .com Domains are allowed.");
                    return View(studentViewModel);
                }

                var existingStudent = await _studentRepository.GetStudentByEmail(studentViewModel.EmailID);
                if (existingStudent != null)
                {
                    ModelState.AddModelError("EmailID", "A student with this email ID already exists.");
                    return View(studentViewModel);
                }

                if (studentViewModel.ResumeFile != null && studentViewModel.ResumeFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "resumes");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + studentViewModel.ResumeFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await studentViewModel.ResumeFile.CopyToAsync(fileStream);
                    }
                    studentViewModel.ResumePath = "/resumes/" + uniqueFileName;
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,EmailID,Gender,GPA,GraduationDate,Degree,ResumeFile,ResumePath")] StudentViewModel studentViewModel)
        {
            if (id != studentViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (studentViewModel.ResumeFile != null && studentViewModel.ResumeFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "resumes");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + studentViewModel.ResumeFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await studentViewModel.ResumeFile.CopyToAsync(fileStream);
                    }
                    studentViewModel.ResumePath = "/resumes/" + uniqueFileName;
                }

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
            var student = await _studentRepository.GetStudentByID(id);
            if (!string.IsNullOrEmpty(student.ResumePath))
            {
                var filePath = Path.Combine(_hostingEnvironment.WebRootPath, student.ResumePath.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
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

        public IActionResult Download(string path)
        {
            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, path.TrimStart('/'));

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/octet-stream", Path.GetFileName(filePath));
        }
    }
}
