using SchoolManagement.API.DTOs;
using SchoolManagement.API.Models;

namespace SchoolManagement.API.Interfaces
{
    public interface IGradeService
    {
        Task<IEnumerable<GradeDto>> GetGradesAsync(int userId);
        Task<GradeResponseDto> GetGradeByIdAsync(int id, int userId);
        Task<GradeDto> CreateGradeAsync(GradeDto gradeToBeCreated, int userId);
        Task<Grade> UpdateGradeAsync(int id, Grade gradeToBeUpdated, int studentId, int subjectId, int userId);
        Task<bool> DeleteGradeAsync(int id, int userId);
    }
}
