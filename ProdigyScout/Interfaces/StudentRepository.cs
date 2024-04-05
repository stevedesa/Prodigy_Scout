﻿using Microsoft.EntityFrameworkCore;
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
                    case "Name":
                        students = students.Where(s => (s.FirstName + " " + s.LastName).Contains(searchTerm));
                        break;
                    case "Min GPA":
                        if (float.TryParse(searchTerm, out float gpaValue))
                        {
                            students = students.Where(s => s.GPA >= gpaValue);
                        }
                        break;
                    case "Min Grad Date":
                        if (DateTime.TryParse(searchTerm, out DateTime graduationDate))
                        {
                            students = students.Where(s => s.GraduationDate.Date >= graduationDate.Date);
                        }
                        break;
                }
            }

            students = sortOrder switch
            {
                "Name" => students.OrderBy(s => (s.FirstName + " " + s.LastName)),
                "Name_desc" => students.OrderByDescending(s => (s.FirstName + " " + s.LastName)),
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

        public async Task<IEnumerable<ComplexDetails>> GetComplexData()
        {
            return await _context.ComplexDetails.ToListAsync();
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
                GraduationDate = studentViewModel.GraduationDate.Date,
            };

            var complexDetails = new ComplexDetails
            {
                IsWatched = studentViewModel.IsWatched,
                IsPipeline = studentViewModel.IsPipeline,
                Prospect = prospect
            };

            prospect.ComplexDetails = complexDetails;

            _context.Add(prospect);
            await _context.SaveChangesAsync();

            return prospect;
        }

        public async Task<Prospect> UpdateStudent(StudentViewModel studentViewModel)
        {
            var prospect = await _context.Prospect.Include(p => p.ComplexDetails).FirstOrDefaultAsync(p => p.Id == studentViewModel.Id);

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

            if (prospect.ComplexDetails == null)
            {
                prospect.ComplexDetails = new ComplexDetails { 
                    ProspectId = studentViewModel.Id, 
                    IsWatched = studentViewModel.IsWatched,
                    IsPipeline= studentViewModel.IsPipeline,
                };
            }
            else
            {
                prospect.ComplexDetails.IsWatched = studentViewModel.IsWatched;
                prospect.ComplexDetails.IsPipeline = studentViewModel.IsPipeline;
            }

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

        public async Task<bool> MarkAsWatch(int studentId)
        {
            return await MarkAsWatchUnwatch(studentId, true);
        }

        public async Task<bool> MarkAsUnwatch(int studentId)
        {
            return await MarkAsWatchUnwatch(studentId, false);
        }

        public async Task<bool> MarkAsWatchUnwatch(int studentId, bool isWatched)
        {
            var prospect = await _context.Prospect.Include(p => p.ComplexDetails).FirstOrDefaultAsync(p => p.Id == studentId);

            if (prospect == null)
            {
                return false;
            }

            if (prospect.ComplexDetails == null)
            {
                prospect.ComplexDetails = new ComplexDetails { ProspectId = studentId, IsWatched = isWatched };
            }
            else
            {
                prospect.ComplexDetails.IsWatched = isWatched;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkAsPipeline(int studentId)
        {
            return await MarkAsPipelineNonPipeline(studentId, true);
        }

        public async Task<bool> MarkAsNonPipeline(int studentId)
        {
            return await MarkAsPipelineNonPipeline(studentId, false);
        }

        public async Task<bool> MarkAsPipelineNonPipeline(int studentId, bool isPipeline)
        {
            var prospect = await _context.Prospect.Include(p => p.ComplexDetails).FirstOrDefaultAsync(p => p.Id == studentId);

            if (prospect == null)
            {
                return false;
            }

            if (prospect.ComplexDetails == null)
            {
                prospect.ComplexDetails = new ComplexDetails { ProspectId = studentId, IsPipeline = isPipeline };
            }
            else
            {
                prospect.ComplexDetails.IsPipeline = isPipeline;
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
