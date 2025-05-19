using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using SchoolManagement.API.Data.Context;
using SchoolManagement.API.DTOs;
using SchoolManagement.API.Interfaces;
using SchoolManagement.API.Models;
using static SchoolManagement.API.Models.User;

namespace SchoolManagement.API.Services
{
    public class AttendanceService(SchoolSysDBContext context, IUserService userService) : IAttendanceService
    {
        private readonly SchoolSysDBContext _context = context;
        private readonly IUserService _userService = userService;

        public async Task<IEnumerable<AttendanceDto>> GetAttendancesAsync(int userId)
        {
            UserRole userRole = await _userService.GetUserRole(userId);

            IQueryable<Attendance> query = _context.Attendances.Include(a => a.Student);

            if (userRole == UserRole.Student)
            {
               query = query.Where(a => a.StudentId == userId);

            }

            return await query.Select(a => new AttendanceDto
            {
                 Id = a.Id,
                 Date = a.Date,
                 Present = a.Present,
                 StudentId = a.Student != null ? a.StudentId : null,
                 TeacherId = userRole != UserRole.Student && a.Teacher != null ? a.TeacherId : null,
            }).ToListAsync();
        }

        public async Task<AttendanceResponseDto> GetAttendanceByIdAsync(int id, int userId)
        {
            UserRole userRole = await _userService.GetUserRole(userId);

            Attendance attendance = await _context.Attendances
                .Include(a => a.Student)
                .Include(a => a.Teacher)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (attendance == null)
            {
                throw new KeyNotFoundException($"Attendance with ID {id} not found.");
            }

            if (userRole == UserRole.Student)
            {
                if (attendance.Student == null || attendance.Student.UserId != userId)
                {
                    throw new UnauthorizedAccessException("You are not authorized to do this action.");
                }
            }

            if (userRole == UserRole.Teacher)
            {
                if (attendance.Teacher != null && attendance.Teacher.UserId != userId)
                {
                    throw new UnauthorizedAccessException("You are not authorized to do this action.");
                }
            }

            return new AttendanceResponseDto
            {
                Id = attendance.Id,
                Date = attendance.Date,
                Present = attendance.Present,
                Student = attendance.Student != null ? new StudentInputDto
                {
                    Id = attendance.Student.Id,
                    Name = attendance.Student.Name,
                    Surname = attendance.Student.Surname
                } : null,
                Teacher = userRole != UserRole.Student && attendance.Teacher != null ? new TeacherInputDto
                {
                    Id = attendance.Teacher.Id,
                    Name = attendance.Teacher.Name,
                    Surname = attendance.Teacher.Surname
                } : null
            };
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
