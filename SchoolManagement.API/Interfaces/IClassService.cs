using SchoolManagement.API.Models;

namespace SchoolManagement.API.Interfaces
{
    public interface IClassService
    {
        Task<IEnumerable<Class>> GetClassesAsync(int userId);
        Task<Class> GetClassByIdAsync(int id, int userId);
        Task<Class> CreateClassAsync(Class classToBeCreated, int userId);
        Task<Class> UpdateClassAsync(int id, Class classToBeUpdated, int userId);
        Task<bool> DeleteClassAsync(int id, int userId);
    }
}
