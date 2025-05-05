using Microsoft.EntityFrameworkCore;
using SchoolManagement.API.Data.Context;
using SchoolManagement.API.Interfaces;
using SchoolManagement.API.Models;
using static SchoolManagement.API.Models.User;

namespace SchoolManagement.API.Services
{
    public class TeacherService(SchoolSysDBContext context, IUserService userService) : ITeacherService
    {
        private readonly SchoolSysDBContext _context = context;
        private readonly IUserService _userService = userService;

        public async Task<IEnumerable<Teacher>> GetTeachersAsync(int userId)
        {
            UserRole userRole = await _userService.GetUserRole(userId);

            if (userRole == UserRole.Student)
            {
                throw new UnauthorizedAccessException("You are not authorized to do this action.");
            }

            IQueryable<Teacher> query = _context.Teachers
                .Include(t => t.User)
                .Include(t => t.Subjects)
                .Include(t => t.Classes);

            return await query.Select(t => new Teacher
            {
                Id = t.Id,
                Name = t.Name,
                Surname = t.Surname,
                BirthDate = t.BirthDate,
                Address = t.Address,
                MobileNumber = t.MobileNumber,
                User = t.User,
                Subjects = t.Subjects.ToList(),
                Classes = t.Classes.ToList(),
                Attendances = userRole == UserRole.Admin ? t.Attendances.ToList() : null,
            }).ToListAsync(); ;
        }

        public async Task<Teacher> GetTeacherByIdAsync(int id, int userId)
        {
            UserRole userRole = await _userService.GetUserRole(userId);

            if (userRole == UserRole.Student)
            {
                throw new UnauthorizedAccessException("You are not authorized to do this action.");
            }

            IQueryable<Teacher> query = _context.Teachers
                .Include(t => t.User)
                .Include(t => t.Subjects)
                .Include(t => t.Classes);

            if (userRole == UserRole.Teacher && id != userId)
            {
                throw new UnauthorizedAccessException("You are not authorized to do this action.");
            }

            Teacher teacher = await query.Select(t => new Teacher
            {
                Id = t.Id,
                Name = t.Name,
                Surname = t.Surname,
                BirthDate = t.BirthDate,
                Address = t.Address,
                MobileNumber = t.MobileNumber,
                User = t.User,
                Subjects = t.Subjects.ToList(),
                Classes = t.Classes.ToList(),
                Attendances = userRole == UserRole.Admin ? t.Attendances.ToList() : null,
            }).FirstOrDefaultAsync(t => t.Id == id);

            if (teacher == null) throw new KeyNotFoundException($"Teacher with ID {id} not found.");

            return teacher;
        }

        public async Task<Teacher> CreateTeacherAsync(Teacher teacherToBeCreated, int userId)
        {
            UserRole userRole = await _userService.GetUserRole(userId);

            if (userRole != UserRole.Admin)
            {
                throw new UnauthorizedAccessException("You are not authorized to do this action.");
            }

            Teacher createdTeacher = new Teacher
            {
                Name = teacherToBeCreated.Name,
                Surname = teacherToBeCreated.Surname,
                BirthDate = teacherToBeCreated.BirthDate,
                Address = teacherToBeCreated.Address,
                MobileNumber = teacherToBeCreated.MobileNumber,
                UserId = teacherToBeCreated.UserId,
            };

            _context.Add(createdTeacher);
            await _context.SaveChangesAsync();

            return createdTeacher;
        }

        public async Task<Teacher> UpdateTeacherAsync(int id, Teacher teacherToBeUpdated, int userId)
        {
            UserRole userRole = await _userService.GetUserRole(userId);

            if (userRole == UserRole.Student)
            {
                throw new UnauthorizedAccessException("You are not authorized to do this action.");
            }

            Teacher teacher = await _context.Teachers
                .Include(t => t.User)
                .Include(t => t.Subjects)
                .Include(t => t.Classes)
                .Include(t => t.Attendances)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (teacher == null) throw new KeyNotFoundException($"Teacher with ID {id} not found.");

            if (userRole == UserRole.Teacher)
            {
                if (id != userId)
                {
                    throw new UnauthorizedAccessException("You are not authorized to do this action.");
                }

                teacher.Name = teacherToBeUpdated.Name;
                teacher.Surname = teacherToBeUpdated.Surname;
                teacher.BirthDate = teacherToBeUpdated.BirthDate;
                teacher.MobileNumber = teacherToBeUpdated.MobileNumber;
            } 

            teacher.Name = teacherToBeUpdated.Name;
            teacher.Surname = teacherToBeUpdated.Surname;
            teacher.BirthDate = teacherToBeUpdated.BirthDate;
            teacher.MobileNumber = teacherToBeUpdated.MobileNumber;
            teacher.Subjects = teacherToBeUpdated.Subjects ?? teacher.Subjects;
            teacher.Classes = teacherToBeUpdated.Classes ?? teacher.Classes;
            teacher.Attendances = teacherToBeUpdated.Attendances ?? teacher.Attendances;

            _context.Update(teacher);
            await _context.SaveChangesAsync();

            return teacher;
        }

    public async Task<bool> DeleteTeacherAsync(int id, int userId)
        {
            UserRole userRole = await _userService.GetUserRole(userId);

            if (userRole != UserRole.Admin)
            {
                throw new UnauthorizedAccessException("You are not authorized to do this action.");
            }

            var teacherToBeDeleted = await _context.Teachers.FindAsync(id);

            if (teacherToBeDeleted == null) throw new KeyNotFoundException($"Teacher with ID {id} not found.");

            _context.Teachers.Remove(teacherToBeDeleted);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
