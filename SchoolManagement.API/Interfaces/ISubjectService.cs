using SchoolManagement.API.DTOs;
using SchoolManagement.API.Models;

namespace SchoolManagement.API.Interfaces
{
    public interface ISubjectService
    {
        Task<IEnumerable<SubjectResponseDto>> GetSubjectsAsync(int userId);
        Task<Subject> GetSubjectByIdAsync(int id, int userId);
        Task<Subject> CreateSubjectAsync(Subject subjectToBeCreated, int userId);
        Task<Subject> UpdateSubjectAsync(int id, Subject subjectToBeUpdated, int userId);
        Task<bool> DeleteSubjectAsync(int id, int userId);
    }
}
