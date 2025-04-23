using SchoolManagement.API.Models;

namespace SchoolManagement.API.Interfaces
{
    public interface IAttendanceService
    {
        Task<IEnumerable<Attendance>> GetAttendancesAsync();
        Task<Attendance> GetAttendanceByIdAsync(int id);
        Task<Attendance> CreateAttendanceAsync(Attendance attendanceToBeCreated, int? studentId, int? teacherId);
        Task<Attendance> UpdateAttendanceAsync(int id, Attendance attendanceToBeUpdated, int? studentId, int? teacherId);
        Task<bool> DeleteAttendanceAsync(int id);
    }
}
