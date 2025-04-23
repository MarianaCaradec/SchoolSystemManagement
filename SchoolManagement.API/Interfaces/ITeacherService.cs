using SchoolManagement.API.Models;

namespace SchoolManagement.API.Interfaces
{
    public interface ITeacherService
    {
        Task<IEnumerable<Teacher>> GetTeachersAsync();
        Task<Teacher> GetTeacherByIdAsync(int id);
        Task<Teacher> CreateTeacherAsync(Teacher teacherToBeCreated);
        Task<Teacher> UpdateTeacherAsync(int id, Teacher teacherToBeUpdated);
        Task<bool> DeleteTeacherAsync(int id);
    }
}
