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

            if(userRole == "Student")
            {
                return await _context.Attendances
                    .Include(a => a.Student)
                    .Select(a => new Attendance
                    {
                       Id = a.Id,
                       Date = a.Date,
                       Present = a.Present,
                       Student = a.Student,
                    }).ToListAsync();
            } 

            return await _context.Attendances
                .Include(a => a.Student)
                .Include(a => a.Teacher)
                .Select(a => new Attendance
                {
                    Id = a.Id,
                    Date = a.Date,
                    Present = a.Present,
                    Student = a.Student,
                    Teacher = a.Teacher
                }).ToListAsync();
        }

        public async Task<Attendance> GetAttendanceByIdAsync(int id, int userId)
        {
            string userRole = await _userService.GetUserRole(userId);

            if (userRole == "Student")
            {
                Attendance studentAttendance = await _context.Attendances
                    .Include(a => a.Student)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (studentAttendance == null) throw new KeyNotFoundException($"Attendance with ID {id} not found.");

                return studentAttendance;
            }

            Attendance attendance = await _context.Attendances
                .Include(a => a.Student)
                .Include(a => a.Teacher)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (attendance == null) throw new KeyNotFoundException($"Attendance with ID {id} not found.");

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
            } else if (userRole == "Teacher")
            {
                Attendance updatedStudentAttendance = await _context.Attendances
                    .Include(a => a.Student)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (updatedStudentAttendance == null) throw new KeyNotFoundException($"Attendance with ID {id} not found.");

                updatedStudentAttendance.Date = attendanceToBeUpdated.Date;
                updatedStudentAttendance.Present = attendanceToBeUpdated.Present;
                updatedStudentAttendance.StudentId = studentId;

                _context.Attendances.Update(updatedStudentAttendance);
                await _context.SaveChangesAsync();

                return updatedStudentAttendance;
            }

            Attendance updatedAttendance = await _context.Attendances
                .Include(a => a.Student)
                .Include(a => a.Teacher)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (updatedAttendance == null) throw new KeyNotFoundException($"Attendance with ID {id} not found.");
            
            updatedAttendance.Date = attendanceToBeUpdated.Date;
            updatedAttendance.Present = attendanceToBeUpdated.Present;
            updatedAttendance.StudentId = studentId;
            updatedAttendance.TeacherId = teacherId;

            _context.Attendances.Update(updatedAttendance);
            await _context.SaveChangesAsync();

            return updatedAttendance;
        }

        public async Task<bool> DeleteAttendanceAsync(int id, int userId)
        {
            string userRole = await _userService.GetUserRole(userId);

            if(userRole == "Teacher" || userRole == "Student")
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
