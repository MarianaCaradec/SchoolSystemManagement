using SchoolManagement.API.Models;

namespace SchoolManagement.API.Interfaces
{
    public interface IAttendanceService
    {
        Task<IEnumerable<Attendance>> GetAttendancesAsync();
        Task<Attendance> GetAttendanceByIdAsync(int id);
        Task<Attendance> CreateAttendanceAsync(Attendance attendanceToBeCreated);
        Task<Attendance> UpdateAttendanceAsync(int id, Attendance attendanceToBeUpdated);
        Task<bool> DeleteAttendanceAsync(int id);
    }
}
