using SchoolManagement.API.DTOs;
using SchoolManagement.API.Models;

namespace SchoolManagement.API.Interfaces
{
    public interface ISubjectService
    {
        Task<IEnumerable<SubjectResponseDto>> GetSubjectsAsync(int userId);
        Task<SubjectResponseDto> GetSubjectByIdAsync(int id, int userId);
        Task<SubjectInputDto> CreateSubjectAsync(SubjectInputDto subjectToBeCreated, int userId);
        Task<SubjectInputDto> UpdateSubjectAsync(int id, SubjectInputDto subjectToBeUpdated, int userId);
        Task<bool> DeleteSubjectAsync(int id, int userId);
    }
}
