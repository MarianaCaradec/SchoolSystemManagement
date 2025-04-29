using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using SchoolManagement.API.Data.Context;
using SchoolManagement.API.Interfaces;
using SchoolManagement.API.Models;

namespace SchoolManagement.API.Services
{
    public class AttendanceService(SchoolSysDBContext context, IUserService userService) : IAttendanceService
    {
        private readonly SchoolSysDBContext _context = context;
        private readonly IUserService _userService = userService;

        public async Task<IEnumerable<Attendance>> GetAttendancesAsync(int userId)
        {
            string userRole = await _userService.GetUserRole(userId);

            IQueryable<Attendance> query = _context.Attendances.Include(a => a.Student);

            if (userRole == "Student")
            {
               query = query.Where(a => a.Student.Id == userId);

            }
            else
            {
               query = query.Include(a => a.Teacher);
            }

            return await query.Select(a => new Attendance
            {
                 Id = a.Id,
                 Date = a.Date,
                 Present = a.Present,
                 Student = a.Student,
            }).ToListAsync();
        }

        public async Task<Attendance> GetAttendanceByIdAsync(int id, int userId)
        {
            string userRole = await _userService.GetUserRole(userId);

            IQueryable<Attendance> query = _context.Attendances.Include(a => a.Student);

            if (userRole == "Teacher" || userRole == "Admin")
            {
                query = query.Include(a => a.Teacher);
            }

            Attendance attendance = await query.FirstOrDefaultAsync(a => a.Id == id);

            if (attendance == null)
            {
                throw new KeyNotFoundException($"Attendance with ID {id} not found.");
            }

            if (userRole == "Student" && attendance.StudentId != userId)
            {
                throw new UnauthorizedAccessException("You are not authorized to do this action.");
            }

            return attendance;
        }

        public async Task<Attendance> CreateAttendanceAsync(Attendance attendanceToBeCreated, int? studentId, int? teacherId, int userId)
        {
            string userRole = await _userService.GetUserRole(userId);

            if(userRole == "Student")
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

            if(createdAttendance.Id == null || createdAttendance.Id == 0)
            {
                Random random = new Random();
                createdAttendance.Id = random.Next(1, int.MaxValue);
            }

            _context.Attendances.Add(createdAttendance);
            await _context.SaveChangesAsync();

            return createdAttendance;
        }

        public async Task<Attendance> UpdateAttendanceAsync(int id, Attendance attendanceToBeUpdated, int? studentId, int? teacherId, int userId)
        {
            string userRole = await _userService.GetUserRole(userId);

            if (userRole == "Student")
            {
                throw new UnauthorizedAccessException("You are not authorized to do this action.");
            }

            IQueryable<Attendance> query = _context.Attendances.Include(a => a.Student);

            Attendance updatedAttendance = query.FirstOrDefault(a => a.Id == id);

            if (updatedAttendance == null) throw new KeyNotFoundException($"Attendance with ID {id} not found.");

            if (userRole == "Teacher")
            {
                DateTime currentDate = DateTime.Now;
                DateTime attendanceDate = attendanceToBeUpdated.Date.ToDateTime(TimeOnly.MinValue);


                if ((currentDate - attendanceDate).TotalDays >= 21)
                {
                    throw new InvalidOperationException("Attendance update is not allowed after three weeks.");
                }

                if(!updatedAttendance.Student.Class.Teachers.Any(t => t.Id == userId))
                {
                    throw new UnauthorizedAccessException("You are not authorized to do this action.");

                }
            } 

            if(userRole == "Admin")
            {
                updatedAttendance.TeacherId = teacherId;
            }

            updatedAttendance.Date = attendanceToBeUpdated.Date;
            updatedAttendance.Present = attendanceToBeUpdated.Present;
            updatedAttendance.StudentId = studentId;

            _context.Attendances.Update(updatedAttendance);
            await _context.SaveChangesAsync();

            return updatedAttendance;
        }

        public async Task<bool> DeleteAttendanceAsync(int id, int userId)
        {
            string userRole = await _userService.GetUserRole(userId);

            if(userRole != "Admin")
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
