using SchoolManagement.API.Models;

namespace SchoolManagement.API.Interfaces
{
    public interface IStudentService
    {
        Task<IEnumerable<Student>> GetStudentsAsync(int userId);
        Task<Student> GetStudentByIdAsync(int id, int userId);
        Task<Student> CreateStudentAsync(Student studentToBeCreated, int userId);
        Task<Student> UpdateStudentAsync(int id, Student studentToBeUpdated, int userId);
        Task<bool> DeleteStudentAsync(int id, int userId);
    }
}
