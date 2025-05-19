﻿using Microsoft.EntityFrameworkCore;
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

        public async Task<AttendanceDto> CreateAttendanceAsync(AttendanceDto attendanceToBeCreated, int userId)
        {
            UserRole userRole = await _userService.GetUserRole(userId);

            if(userRole == UserRole.Student)
            {
                throw new UnauthorizedAccessException("You are not authorized to do this action.");
            }

            bool hasStudent = attendanceToBeCreated.StudentId.HasValue && attendanceToBeCreated.StudentId != 0;
            bool hasTeacher = attendanceToBeCreated.TeacherId.HasValue && attendanceToBeCreated.TeacherId != 0;

            if (!hasStudent && !hasTeacher)
            {
                throw new ArgumentException("Either StudentId or TeacherId must be provided.");
            }

            if (hasStudent && hasTeacher)
            {
                throw new ArgumentException("Only one of StudentId or TeacherId must be provided.");
            }

            if (hasStudent)
            {
                Student studentWithClassAndTeachers = await _context.Students
                    .Include(s => s.Class.Teachers)
                    .FirstOrDefaultAsync(s => s.Id == attendanceToBeCreated.StudentId);

                if (studentWithClassAndTeachers == null)
                {
                    throw new ArgumentException("Invalid student or class information.");
                }

                if (userRole == UserRole.Teacher &&
                !studentWithClassAndTeachers.Class.Teachers.Any(t => t.UserId == userId))
                {
                    throw new UnauthorizedAccessException("You are not authorized to do this action.");
                }
            }

            if (hasTeacher)
            {
                if (userRole == UserRole.Teacher)
                {
                    throw new UnauthorizedAccessException("Teachers cannot create attendance records for other teachers.");
                }

                if (userRole == UserRole.Teacher && attendanceToBeCreated.TeacherId == userId)
                {
                    throw new UnauthorizedAccessException("Teachers cannot create attendance records for themselves.");
                }
            }


            Attendance createdAttendanceToBeSaved = new Attendance
            {
                Date = attendanceToBeCreated.Date,
                Present = attendanceToBeCreated.Present,
                StudentId = hasStudent ? attendanceToBeCreated.StudentId : null,
                TeacherId = hasTeacher ? attendanceToBeCreated.TeacherId : null
            };

            _context.Attendances.Add(createdAttendanceToBeSaved);
            await _context.SaveChangesAsync();

            return new AttendanceDto
            {
                Id = createdAttendanceToBeSaved.Id,
                Date = createdAttendanceToBeSaved.Date,
                Present = createdAttendanceToBeSaved.Present,
                StudentId = createdAttendanceToBeSaved.StudentId,
                TeacherId = createdAttendanceToBeSaved.TeacherId
            };
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
