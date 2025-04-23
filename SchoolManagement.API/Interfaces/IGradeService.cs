using SchoolManagement.API.Models;

namespace SchoolManagement.API.Interfaces
{
    public interface IGradeService
    {
        Task<IEnumerable<Grade>> GetGradesAsync();
        Task<Grade> GetGradeByIdAsync(int id);
        Task<Grade> CreateGradeAsync(Grade gradeToBeCreated);
        Task<Grade> UpdateGradeAsync(int id, Grade gradeToBeUpdated);
        Task<bool> DeleteGradeAsync(int id);
    }
}
