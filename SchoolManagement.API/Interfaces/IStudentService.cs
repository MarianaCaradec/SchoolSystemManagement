using SchoolManagement.API.DTOs;
using SchoolManagement.API.Models;

namespace SchoolManagement.API.Interfaces
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentResponseDto>> GetStudentsAsync(int userId);
        Task<StudentResponseDto> GetStudentByIdAsync(int id, int userId);
        Task<Student> CreateStudentAsync(Student studentToBeCreated, int userId);
        Task<Student> UpdateStudentAsync(int id, Student studentToBeUpdated, int userId);
        Task<bool> DeleteStudentAsync(int id, int userId);
    }
}
