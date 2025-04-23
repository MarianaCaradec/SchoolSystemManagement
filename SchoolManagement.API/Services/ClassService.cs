using Microsoft.EntityFrameworkCore;
using SchoolManagement.API.Data.Context;
using SchoolManagement.API.Interfaces;
using SchoolManagement.API.Models;

namespace SchoolManagement.API.Services
{
    public class ClassService(SchoolSysDBContext context) : IClassService
    {
        private readonly SchoolSysDBContext _context = context;

        public async Task<IEnumerable<Class>> GetClassesAsync()
        {
            return await _context.Classes
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
        }

        public async Task<Class> GetClassByIdAsync(int id)
        {
            Class classById = await _context.Classes
                .Include(c => c.Teachers)
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (classById == null) throw new KeyNotFoundException($"Class with ID {id} not found.");

            return classById;
        }

        public async Task<Class> CreateClassAsync(Class classToBeCreated)
        {
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

        public async Task<Class> UpdateClassAsync(int id, Class classToBeUpdated)
        {
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

        public async Task<bool> DeleteClassAsync(int id)
        {
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
