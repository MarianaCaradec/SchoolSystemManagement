using SchoolManagement.API.DTOs;
using SchoolManagement.API.Models;

namespace SchoolManagement.API.Interfaces
{
    public interface IClassService
    {
        Task<IEnumerable<ClassResponseDto>> GetClassesAsync(int userId);
        Task<ClassResponseDto> GetClassByIdAsync(int id, int userId);
        Task<ClassInputDto> CreateClassAsync(ClassInputDto classToBeCreated, int userId);
        Task<ClassInputDto> UpdateClassAsync(int id, ClassInputDto classToBeUpdated, int userId);
        Task<bool> DeleteClassAsync(int id, int userId);
    }
}
