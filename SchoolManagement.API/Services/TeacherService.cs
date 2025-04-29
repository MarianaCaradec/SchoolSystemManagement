using Microsoft.EntityFrameworkCore;
using SchoolManagement.API.Data.Context;
using SchoolManagement.API.Interfaces;
using SchoolManagement.API.Models;

namespace SchoolManagement.API.Services
{
    public class TeacherService(SchoolSysDBContext context, IUserService userService) : ITeacherService
    {
        private readonly SchoolSysDBContext _context = context;
        private readonly IUserService _userService = userService;

        public async Task<IEnumerable<Teacher>> GetTeachersAsync(int userId)
        {
            string userRole = await _userService.GetUserRole(userId);

            if (userRole == "Student")
            {
                throw new UnauthorizedAccessException("You are not authorized to do this action.");
            }

            IEnumerable<Teacher> teachers;

            if(userRole == "Teacher")
            {
                teachers = await _context.Teachers
                .Include(t => t.User)
                .Include(t => t.Subjects)
                .Include(t => t.Classes)
                .Select(t => new Teacher
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
                }).ToListAsync();
            } else
            {
                teachers = await _context.Teachers
                .Include(t => t.User)
                .Include(t => t.Subjects)
                .Include(t => t.Classes)
                .Include(t => t.Attendances)
                .Select(t => new Teacher
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
                    Attendances = t.Attendances.ToList()
                }).ToListAsync();
            }

            return teachers;
        }

        public async Task<Teacher> GetTeacherByIdAsync(int id, int userId)
        {
            string userRole = await _userService.GetUserRole(userId);

            if (userRole == "Student")
            {
                throw new UnauthorizedAccessException("You are not authorized to do this action.");
            }

            Teacher teacher;

            if (userRole == "Teacher")
            {
                teacher = await _context.Teachers
                .Include(t => t.User)
                .Include(t => t.Subjects)
                .Include(t => t.Classes)
                .FirstOrDefaultAsync(t => t.Id == id);

                bool isTeacherData = teacher.Id == userId;

                if (isTeacherData)
                {
                    teacher = await _context.Teachers
                    .Include(t => t.User)
                    .Include(t => t.Subjects)
                    .Include(t => t.Classes)
                    .Include(t => t.Attendances)
                    .FirstOrDefaultAsync(t => t.Id == id);
                }
            } else
            {
                teacher = await _context.Teachers
                .Include(t => t.User)
                .Include(t => t.Subjects)
                .Include(t => t.Classes)
                .Include (t => t.Attendances)
                .FirstOrDefaultAsync(t => t.Id == id);
            }

            if (teacher == null) throw new KeyNotFoundException($"Teacher with ID {id} not found.");

            return teacher;
        }

        public async Task<Teacher> CreateTeacherAsync(Teacher teacherToBeCreated, int userId)
        {
            string userRole = await _userService.GetUserRole(userId);

            if (userRole == "Student" || userRole == "Teacher")
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

            if (createdTeacher.Id == null || createdTeacher.Id == 0)
            {
                Random random = new Random();
                createdTeacher.Id = random.Next(1, int.MaxValue);
            }

            _context.Add(createdTeacher);
            await _context.SaveChangesAsync();

            return createdTeacher;
        }

        public async Task<Teacher> UpdateTeacherAsync(int id, Teacher teacherToBeUpdated, int userId)
        {
            string userRole = await _userService.GetUserRole(userId);

            if (userRole == "Student")
            {
                throw new UnauthorizedAccessException("You are not authorized to do this action.");
            }

            Teacher updatedTeacher = await _context.Teachers
                .Include(t => t.User)
                .Include(t => t.Subjects)
                .Include(t => t.Classes)
                .Include(t => t.Attendances)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (updatedTeacher == null) throw new KeyNotFoundException($"Teacher with ID {id} not found.");

            if (userRole == "Teacher")
            {
                updatedTeacher.Name = teacherToBeUpdated.Name;
                updatedTeacher.Surname = teacherToBeUpdated.Surname;
                updatedTeacher.BirthDate = teacherToBeUpdated.BirthDate;
                updatedTeacher.MobileNumber = teacherToBeUpdated.MobileNumber;
            } else
            {
                updatedTeacher.Name = teacherToBeUpdated.Name;
                updatedTeacher.Surname = teacherToBeUpdated.Surname;
                updatedTeacher.BirthDate = teacherToBeUpdated.BirthDate;
                updatedTeacher.MobileNumber = teacherToBeUpdated.MobileNumber;
                updatedTeacher.Subjects = teacherToBeUpdated.Subjects ?? updatedTeacher.Subjects;
                updatedTeacher.Classes = teacherToBeUpdated.Classes ?? updatedTeacher.Classes;
                updatedTeacher.Attendances = teacherToBeUpdated.Attendances ?? updatedTeacher.Attendances;
            }

            _context.Update(updatedTeacher);
            await _context.SaveChangesAsync();

            return updatedTeacher;
        }

    public async Task<bool> DeleteTeacherAsync(int id, int userId)
        {
            string userRole = await _userService.GetUserRole(userId);

            if (userRole == "Student" || userRole == "Teacher")
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
