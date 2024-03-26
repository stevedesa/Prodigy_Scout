using ProdigyScout.Models;
using ProdigyScout.ViewModels;

namespace ProdigyScout.Interfaces
{
    public interface IStudentRepository : IDisposable
    {
        Task<IList<Prospect>> GetStudents(string firstName, string lastName, string gpa, string gradYear, string sortOrder);
        Task<Prospect> GetStudentByID(int studentId);
        Task<Prospect> InsertStudent(StudentViewModel studentViewModel);
        Task<Prospect> UpdateStudent(StudentViewModel studentViewModel);
        Task DeleteStudent(int studentID);
    }
}
