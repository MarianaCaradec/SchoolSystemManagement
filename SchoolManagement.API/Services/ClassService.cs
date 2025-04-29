using Microsoft.EntityFrameworkCore;
using SchoolManagement.API.Data.Context;
using SchoolManagement.API.Interfaces;
using SchoolManagement.API.Models;

namespace SchoolManagement.API.Services
{
    public class ClassService(SchoolSysDBContext context, IUserService userService) : IClassService
    {
        private readonly SchoolSysDBContext _context = context;
        private readonly IUserService _userService = userService;

        public async Task<IEnumerable<Class>> GetClassesAsync(int userId)
        {
            string userRole = await _userService.GetUserRole(userId);

            if(userRole == "Student")
            {
                throw new UnauthorizedAccessException("You are not authorized to do this action.");
            }

            IEnumerable<Class> classes;

            if(userRole == "Teacher")
            {
               classes = await _context.Classes
                .Include(c => c.Teachers)
                .Include(c => c.Students)
                .Where(c => c.Teachers.Any(t => t.Id == userId))
                .Select(c => new Class
                {
                    Id = c.Id,
                    Course = c.Course,
                    Divition = c.Divition,
                    Capacity = c.Capacity,
                    Teachers = c.Teachers.ToList(),
                    Students = c.Students.ToList()
                }).ToListAsync();
            }

            classes = await _context.Classes
                .Include(c => c.Teachers)
                .Include(c => c.Students)
                .Select(c => new Class
                {
                    Id = c.Id,
                    Course = c.Course,
                    Divition = c.Divition,
                    Capacity = c.Capacity,
                    Teachers = c.Teachers.ToList(),
                    Students = c.Students.ToList()
                }).ToListAsync();

            return classes;
        }

        public async Task<Class> GetClassByIdAsync(int id, int userId)
        {
            string userRole = await _userService.GetUserRole(userId);

            Class classById;

            if (userRole == "Student" || userRole == "Teacher")
            {
                classById = await _context.Classes
                .Include(c => c.Teachers)
                .Include(c => c.Students)
                .Where(c => c.Students.Any(s => s.Id == userId) || c.Teachers.Any(t => t.Id == userId))
                .FirstOrDefaultAsync(c => c.Id == id);
            } else
            {
                classById = await _context.Classes
                    .Include(c => c.Teachers)
                    .Include(c => c.Students)
                    .FirstOrDefaultAsync(c => c.Id == id);
            }

            if (classById == null) throw new KeyNotFoundException($"Class with ID {id} not found.");

            return classById;
        }

        public async Task<Class> CreateClassAsync(Class classToBeCreated, int userId)
        {
            string userRole = await _userService.GetUserRole(userId);

            if (userRole == "Student" || userRole == "Teacher")
            {
                throw new UnauthorizedAccessException("You are not authorized to do this action.");
            }

            Class createdClass = new Class
            {
                Course = classToBeCreated.Course,
                Divition = classToBeCreated.Divition,
                Capacity = classToBeCreated.Capacity,
                Teachers = classToBeCreated.Teachers,
                Students = classToBeCreated.Students
            };

            if (createdClass.Id == null || createdClass.Id == 0)
            {
                Random random = new Random();
                createdClass.Id = random.Next(1, int.MaxValue);
            }

            _context.Classes.Add(createdClass);
            await _context.SaveChangesAsync();

            return createdClass;
        }

        public async Task<Class> UpdateClassAsync(int id, Class classToBeUpdated, int userId)
        {
            string userRole = await _userService.GetUserRole(userId);

            if (userRole == "Student" || userRole == "Teacher")
            {
                throw new UnauthorizedAccessException("You are not authorized to do this action.");
            }

            Class classById = await _context.Classes
                .Include(c => c.Teachers)
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (classById == null) throw new KeyNotFoundException($"Class with ID {id} not found.");

            classById.Course = classToBeUpdated.Course;
            classById.Divition = classToBeUpdated.Divition;
            classById.Capacity = classToBeUpdated.Capacity;
            classById.Teachers = classToBeUpdated.Teachers;
            classById.Students = classToBeUpdated.Students;

            _context.Classes.Update(classById);
            await _context.SaveChangesAsync();

            return classById;
        }

        public async Task<bool> DeleteClassAsync(int id, int userId)
        {
            string userRole = await _userService.GetUserRole(userId);

            if (userRole == "Student" || userRole == "Teacher")
            {
                throw new UnauthorizedAccessException("You are not authorized to do this action.");
            }

            Class classToBeDeleted = await _context.Classes
             .Include(c => c.Teachers)
             .Include(c => c.Students)
             .FirstOrDefaultAsync(c => c.Id == id);

            if (classToBeDeleted == null) throw new KeyNotFoundException($"Class with ID {id} not found.");

            _context.Classes.Remove(classToBeDeleted);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
