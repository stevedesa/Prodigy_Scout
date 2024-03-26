using Microsoft.EntityFrameworkCore;
using ProdigyScout.Models;
using ProdigyScout.Data;
using ProdigyScout.ViewModels;
using static ProdigyScout.Helpers.Enums;

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

        public async Task<IList<Prospect>> GetStudents(string firstName, string lastName, string gpa, string gradYear, string sortOrder)
            {
                var students = _context.Prospect.AsQueryable();

                if (!string.IsNullOrEmpty(firstName))
                {
                    students = students.Where(s => s.FirstName.Contains(firstName));
                }

                if (!string.IsNullOrEmpty(lastName))
                {
                    students = students.Where(s => s.LastName.Contains(lastName));
                }

                if (!string.IsNullOrEmpty(gpa))
                {
                    float gpaValue;
                    if (float.TryParse(gpa, out gpaValue))
                    {
                        students = students.Where(s => s.GPA > gpaValue);
                    }
                }

                if (!string.IsNullOrEmpty(gradYear))
                {
                    DateTime gradYearValue;
                    if (DateTime.TryParse(gradYear, out gradYearValue))
                    {
                        students = students.Where(s => s.GraduationDate > gradYearValue);
                    }
                }

                students = sortOrder switch
                {
                    "FirstName" => students.OrderBy(s => s.FirstName),
                    "LastName_desc" => students.OrderByDescending(s => s.LastName),
                    "LastName" => students.OrderBy(s => s.LastName),
                    "GPA_desc" => students.OrderByDescending(s => s.GPA),
                    "GPA" => students.OrderBy(s => s.GPA),
                    "GraduationDate_desc" => students.OrderByDescending(s => s.GraduationDate),
                    "GraduationDate" => students.OrderBy(s => s.GraduationDate),
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
                var prospect = new Prospect
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

                return prospect;
            }

            public async Task<Prospect> UpdateStudent(StudentViewModel studentViewModel)
            {
                var prospect = await _context.Prospect.FindAsync(studentViewModel.Id);

                if (prospect == null)
                {
                    return null;
                }

                prospect.FirstName = studentViewModel.FirstName.Trim();
                prospect.LastName = studentViewModel.LastName.Trim();
                prospect.email = studentViewModel.email.Trim();
                prospect.Gender = studentViewModel.Gender.Trim();
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
