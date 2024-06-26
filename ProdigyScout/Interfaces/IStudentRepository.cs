﻿using ProdigyScout.Models;
using ProdigyScout.ViewModels;

namespace ProdigyScout.Interfaces
{
    public interface IStudentRepository : IDisposable
    {
        Task<IList<Prospect>> GetStudents(string filterBy, string searchTerm, string sortOrder);
        Task<Prospect> GetStudentByID(int studentId);
        Task<IEnumerable<ComplexDetails>> GetComplexData();
        Task<Prospect> InsertStudent(StudentViewModel studentViewModel);
        Task<Prospect> UpdateStudent(StudentViewModel studentViewModel);
        Task DeleteStudent(int studentID);
        Task<bool> MarkAsWatch(int studentId);
        Task<bool> MarkAsUnwatch(int studentId);
        Task<bool> MarkAsWatchUnwatch(int studentId, bool isWatched);
        Task<bool> MarkAsPipeline(int studentId);
        Task<bool> MarkAsNonPipeline(int studentId);
        Task<bool> MarkAsPipelineNonPipeline(int studentId, bool isPipeline);
        Task<bool> AddOrUpdateComment(int studentId, string comment);
        Task<Prospect> GetStudentByEmail(string email);
    }
}
