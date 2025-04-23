using SchoolManagement.API.Models;

namespace SchoolManagement.API.Interfaces
{
    public interface ISubjectService
    {
        Task<IEnumerable<Subject>> GetSubjectsAsync();
        Task<Subject> GetSubjectByIdAsync(int id);
        Task<Subject> CreateSubjectAsync(Subject subjectToBeCreated);
        Task<Subject> UpdateSubjectAsync(int id, Subject subjectToBeUpdated);
        Task<bool> DeleteSubjectAsync(int id);
    }
}
