using SchoolManagement.API.Models;

namespace SchoolManagement.API.Interfaces
{
    public interface IStudentService
    {
        Task<IEnumerable<Student>> GetStudentsAsync();
        Task<Student> GetStudentByIdAsync(int id);
        Task<Student> CreateStudentAsync(Student studentToBeCreated);
        Task<Student> UpdateStudentAsync(int id, Student studentToBeUpdated);
        Task<bool> DeleteStudentAsync(int id);
    }
}
