using SchoolManagement.API.Models;

namespace SchoolManagement.API.Interfaces
{
    public interface ITeacherService
    {
        Task<IEnumerable<Teacher>> GetTeachersAsync(int userId);
        Task<Teacher> GetTeacherByIdAsync(int id, int userId);
        Task<Teacher> CreateTeacherAsync(Teacher teacherToBeCreated, int userId);
        Task<Teacher> UpdateTeacherAsync(int id, Teacher teacherToBeUpdated, int userId);
        Task<bool> DeleteTeacherAsync(int id, int userId);
    }
}
