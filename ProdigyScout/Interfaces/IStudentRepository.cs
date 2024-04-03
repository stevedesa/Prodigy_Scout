using ProdigyScout.Models;
using ProdigyScout.ViewModels;

namespace ProdigyScout.Interfaces
{
    public interface IStudentRepository : IDisposable
    {
        Task<IList<Prospect>> GetStudents(string filterBy, string searchTerm, string sortOrder);
        Task<Prospect> GetStudentByID(int studentId);
        Task<Prospect> InsertStudent(StudentViewModel studentViewModel);
        Task<Prospect> UpdateStudent(StudentViewModel studentViewModel);
        Task DeleteStudent(int studentID);
    }
}
