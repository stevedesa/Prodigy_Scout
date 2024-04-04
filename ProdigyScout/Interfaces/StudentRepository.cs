using Microsoft.EntityFrameworkCore;
using ProdigyScout.Data;
using ProdigyScout.Models;
using ProdigyScout.ViewModels;

namespace ProdigyScout.Interfaces
{
    public class StudentRepository : IStudentRepository, IDisposable
    {
        private readonly ProdigyScoutContext _context;

        public StudentRepository(ProdigyScoutContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }

        public async Task<IList<Prospect>> GetStudents(string filterBy, string searchTerm, string sortOrder)
        {
            var students = _context.Prospect.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                switch (filterBy)
                {
                    case "First Name":
                        students = students.Where(s => s.FirstName.Contains(searchTerm));
                        break;
                    case "Last Name":
                        students = students.Where(s => s.LastName.Contains(searchTerm));
                        break;
                    case "Email":
                        students = students.Where(s => s.Email.Contains(searchTerm));
                        break;
                    case "Gender":
                        students = students.Where(s => s.Gender.Contains(searchTerm));
                        break;
                    case "GPA":
                        if (float.TryParse(searchTerm, out float gpaValue))
                        {
                            students = students.Where(s => s.GPA >= gpaValue);
                        }
                        break;
                    case "Graduation Date":
                        if (DateTime.TryParse(searchTerm, out DateTime graduationDate))
                        {
                            students = students.Where(s => s.GraduationDate.Date >= graduationDate.Date);
                        }
                        break;
                }
            }

            students = sortOrder switch
            {
                "FirstName" => students.OrderBy(s => s.FirstName),
                "FirstName_desc" => students.OrderByDescending(s => s.FirstName),
                "LastName" => students.OrderBy(s => s.LastName),
                "LastName_desc" => students.OrderByDescending(s => s.LastName),
                "GPA" => students.OrderBy(s => s.GPA),
                "GPA_desc" => students.OrderByDescending(s => s.GPA),
                "GraduationDate" => students.OrderBy(s => s.GraduationDate),
                "GraduationDate_desc" => students.OrderByDescending(s => s.GraduationDate),
                _ => students.OrderByDescending(s => s.GPA)
            };

            return await students.ToListAsync();
        }

        public async Task<Prospect> GetStudentByID(int studentId)
        {
            return await _context.Prospect.FirstOrDefaultAsync(s => s.Id == studentId);
        }

        public async Task<Prospect> InsertStudent(StudentViewModel studentViewModel)
        {
            if (studentViewModel == null)
            {
                throw new ArgumentNullException(nameof(studentViewModel));
            }

            var prospect = new Prospect
            {
                FirstName = studentViewModel.FirstName?.Trim(),
                LastName = studentViewModel.LastName?.Trim(),
                Email = studentViewModel.EmailID?.Trim(),
                Gender = studentViewModel.Gender?.Trim(),
                GPA = studentViewModel.GPA,
                GraduationDate = studentViewModel.GraduationDate.Date
            };

            _context.Add(prospect);
            await _context.SaveChangesAsync();

            return prospect;
        }

        public async Task<Prospect> UpdateStudent(StudentViewModel studentViewModel)
        {
            var prospect = await _context.Prospect.FindAsync(studentViewModel.Id);

            if (prospect == null)
            {
                return null;
            }

            prospect.FirstName = studentViewModel.FirstName?.Trim();
            prospect.LastName = studentViewModel.LastName?.Trim();
            prospect.Email = studentViewModel.EmailID?.Trim();
            prospect.Gender = studentViewModel.Gender?.Trim();
            prospect.GPA = studentViewModel.GPA;
            prospect.GraduationDate = studentViewModel.GraduationDate;

            _context.Update(prospect);
            await _context.SaveChangesAsync();

            return prospect;
        }

        public async Task DeleteStudent(int studentID)
        {
            var prospect = await _context.Prospect.FindAsync(studentID);
            if (prospect != null)
            {
                _context.Prospect.Remove(prospect);
                await _context.SaveChangesAsync();
            }
        }
    }
}
