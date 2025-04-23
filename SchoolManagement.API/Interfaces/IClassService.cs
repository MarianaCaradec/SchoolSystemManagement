using SchoolManagement.API.Models;

namespace SchoolManagement.API.Interfaces
{
    public interface IClassService
    {
        Task<IEnumerable<Class>> GetClassesAsync();
        Task<Class> GetClassByIdAsync(int id);
        Task<Class> CreateClassAsync(Class classToBeCreated);
        Task<Class> UpdateClassAsync(int id, Class classToBeUpdated);
        Task<bool> DeleteClassAsync(int id);
    }
}
