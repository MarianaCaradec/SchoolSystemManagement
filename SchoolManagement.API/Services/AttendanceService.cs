using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using SchoolManagement.API.Data.Context;
using SchoolManagement.API.Interfaces;
using SchoolManagement.API.Models;
using static SchoolManagement.API.Models.User;

namespace SchoolManagement.API.Services
{
    public class AttendanceService(SchoolSysDBContext context, IUserService userService) : IAttendanceService
    {
        private readonly SchoolSysDBContext _context = context;
        private readonly IUserService _userService = userService;

        public async Task<IEnumerable<Attendance>> GetAttendancesAsync(int userId)
        {
            UserRole userRole = await _userService.GetUserRole(userId);

            IQueryable<Attendance> query = _context.Attendances.Include(a => a.Student);

            if (userRole == UserRole.Student)
            {
               query = query.Where(a => a.StudentId == userId);

            }

            return await query.Select(a => new Attendance
            {
                 Id = a.Id,
                 Date = a.Date,
                 Present = a.Present,
                 Student = a.Student,
                 Teacher = userRole != UserRole.Student ? a.Teacher : null,
            }).ToListAsync();
        }

        public async Task<Attendance> GetAttendanceByIdAsync(int id, int userId)
        {
            UserRole userRole = await _userService.GetUserRole(userId);

            IQueryable<Attendance> query = _context.Attendances.Include(a => a.Student);

            if (userRole != UserRole.Student)
            {
                query = query.Include(a => a.Teacher);
            }

            Attendance attendance = await query.FirstOrDefaultAsync(a => a.Id == id);

            if (attendance == null)
            {
                throw new KeyNotFoundException($"Attendance with ID {id} not found.");
            }

            if (userRole == UserRole.Student && attendance.StudentId != userId)
            {
                throw new UnauthorizedAccessException("You are not authorized to do this action.");
            }

            return attendance;
        }

        public async Task<Attendance> CreateAttendanceAsync(Attendance attendanceToBeCreated, int? studentId, int? teacherId, int userId)
        {
            UserRole userRole = await _userService.GetUserRole(userId);

            if(userRole == UserRole.Student)
            {
                throw new UnauthorizedAccessException("You are not authorized to do this action.");
            }

            Attendance createdAttendance = new Attendance
            {
                Date = attendanceToBeCreated.Date,
                Present = attendanceToBeCreated.Present,
                StudentId = studentId,
                TeacherId = teacherId
            };

            _context.Attendances.Add(createdAttendance);
            await _context.SaveChangesAsync();

            return createdAttendance;
        }

        public async Task<Attendance> UpdateAttendanceAsync(int id, Attendance attendanceToBeUpdated, int? studentId, int? teacherId, int userId)
        {
            UserRole userRole = await _userService.GetUserRole(userId);

            if (userRole == UserRole.Student)
            {
                throw new UnauthorizedAccessException("You are not authorized to do this action.");
            }

            IQueryable<Attendance> query = _context.Attendances.Include(a => a.Student);

            Attendance attendance = query.FirstOrDefault(a => a.Id == id);

            if (attendance == null) throw new KeyNotFoundException($"Attendance with ID {id} not found.");

            if (userRole == UserRole.Teacher)
            {
                if (!attendance.Student.Class.Teachers.Any(t => t.Id == userId))
                {
                    throw new UnauthorizedAccessException("You are not authorized to do this action.");

                }

                DateTime currentDate = DateTime.Now;
                DateTime attendanceDate = attendance.Date.ToDateTime(TimeOnly.MinValue);

                if ((currentDate - attendanceDate).TotalDays >= 21)
                {
                    throw new InvalidOperationException("Attendance update is not allowed after three weeks.");
                }
            } 

            if(userRole == UserRole.Admin)
            {
                attendance.TeacherId = teacherId;
            }

            attendance.Date = attendanceToBeUpdated.Date;
            attendance.Present = attendanceToBeUpdated.Present;
            attendance.StudentId = studentId;

            _context.Attendances.Update(attendance);
            await _context.SaveChangesAsync();

            return attendance;
        }

        public async Task<bool> DeleteAttendanceAsync(int id, int userId)
        {
            UserRole userRole = await _userService.GetUserRole(userId);

            if(userRole != UserRole.Admin)
            {
                throw new UnauthorizedAccessException("You are not authorized to do this action.");
            }

            Attendance attendanceToBeDeleted = await _context.Attendances.FindAsync(id);

            if (attendanceToBeDeleted == null) throw new KeyNotFoundException($"Attendance with ID {id} not found.");

            _context.Attendances.Remove(attendanceToBeDeleted);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
