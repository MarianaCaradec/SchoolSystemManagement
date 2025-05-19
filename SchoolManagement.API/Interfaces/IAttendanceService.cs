using SchoolManagement.API.DTOs;
using SchoolManagement.API.Models;

namespace SchoolManagement.API.Interfaces
{
    public interface IAttendanceService
    {
        Task<IEnumerable<AttendanceDto>> GetAttendancesAsync(int userId);
        Task<AttendanceResponseDto> GetAttendanceByIdAsync(int id, int userId);
        Task<AttendanceDto> CreateAttendanceAsync(AttendanceDto attendanceToBeCreated, int userId);
        Task<AttendanceDto> UpdateAttendanceAsync(int id, AttendanceDto attendanceToBeUpdated, int userId);
        Task<bool> DeleteAttendanceAsync(int id, int userId);
    }
}
