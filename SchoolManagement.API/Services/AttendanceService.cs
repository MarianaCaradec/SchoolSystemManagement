using Microsoft.EntityFrameworkCore;
using SchoolManagement.API.Data.Context;
using SchoolManagement.API.Interfaces;
using SchoolManagement.API.Models;

namespace SchoolManagement.API.Services
{
    public class AttendanceService(SchoolSysDBContext context) : IAttendanceService
    {
        private readonly SchoolSysDBContext _context = context;

        public async Task<IEnumerable<Attendance>> GetAttendancesAsync()
        {
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

        public async Task<Attendance> GetAttendanceByIdAsync(int id)
        {
            Attendance attendance = await _context.Attendances
                .Include(a => a.Student)
                .Include(a => a.Teacher)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (attendance == null) throw new KeyNotFoundException($"Attendance with ID {id} not found.");

            return attendance;
        }

        public async Task<Attendance> CreateAttendanceAsync(Attendance attendanceToBeCreated, int? studentId, int? teacherId)
        {
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

        public async Task<Attendance> UpdateAttendanceAsync(int id, Attendance attendanceToBeUpdated, int? studentId, int? teacherId)
        {
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

        public async Task<bool> DeleteAttendanceAsync(int id)
        {
            Attendance attendanceToBeDeleted = await _context.Attendances.FindAsync(id);

            if (attendanceToBeDeleted == null) throw new KeyNotFoundException($"Attendance with ID {id} not found.");

            _context.Attendances.Remove(attendanceToBeDeleted);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
