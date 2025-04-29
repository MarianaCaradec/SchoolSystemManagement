using Microsoft.EntityFrameworkCore;
using SchoolManagement.API.Data.Context;
using SchoolManagement.API.Interfaces;
using SchoolManagement.API.Models;

namespace SchoolManagement.API.Services
{
    public class SubjectService(SchoolSysDBContext context, IUserService userService) : ISubjectService
    {
        private readonly SchoolSysDBContext _context = context;
        private readonly IUserService _userService = userService;

        public async Task<IEnumerable<Subject>> GetSubjectsAsync(int userId)
        {
            string userRole = await _userService.GetUserRole(userId);

            IEnumerable<Subject> subjects;

            if(userRole == "Teacher")
            {
                subjects = await _context.Subjects
                .Include(sub => sub.Teachers)
                .Where(sub => sub.Teachers.Any(t => t.Id == userId))
                .Select(sub => new Subject
                {
                    Id = sub.Id,
                    Title = sub.Title,
                    Teachers = sub.Teachers.ToList()
                }).ToListAsync();
            } else if(userRole == "Student")
            {
                subjects = await _context.Subjects
                .Include(sub => sub.Teachers)
                .Include(sub => sub.Grades)
                .Where(sub => sub.Grades.Any(g => g.StudentId == userId))
                .Select(sub => new Subject
                {
                    Id = sub.Id,
                    Title = sub.Title,
                    Teachers = sub.Teachers.ToList(),
                    Grades = sub.Grades.ToList()
                }).ToListAsync();
            } else
            {
                subjects = await _context.Subjects
                 .Include(sub => sub.Teachers)
                 .Include(sub => sub.Grades)
                  .Select(sub => new Subject
                  {
                     Id = sub.Id,
                     Title = sub.Title,
                     Teachers = sub.Teachers.ToList(),
                     Grades = sub.Grades.ToList()
                  }).ToListAsync();
            }

            return subjects;
        }

        public async Task<Subject> GetSubjectByIdAsync(int id, int userId)
        {
            string userRole = await _userService.GetUserRole(userId);

            Subject subject;

            if (userRole == "Teacher")
            {
                subject = await _context.Subjects
                 .Include(sub => sub.Teachers)
                 .Where(sub => sub.Teachers.Any(t => t.Id == userId))
                 .FirstOrDefaultAsync(sub => sub.Id == id);
            }
            else if (userRole == "Student")
            {
                subject = await _context.Subjects
                 .Include(sub => sub.Teachers)
                 .Include(sub => sub.Grades)
                 .Where(sub => sub.Grades.Any(g => g.StudentId == userId))
                 .FirstOrDefaultAsync(sub => sub.Id == id);
            }
            else
            {
                subject = await _context.Subjects
                .Include(sub => sub.Teachers)
                .Include(sub => sub.Grades)
                .FirstOrDefaultAsync(sub => sub.Id == id);
            }

            if (subject == null) throw new KeyNotFoundException($"Subject with ID {id} not found.");
            
            return subject;
        }

        public async Task<Subject> CreateSubjectAsync(Subject subjectToBeCreated, int userId)
        {
            string userRole = await _userService.GetUserRole(userId);

            if (userRole == "Student" || userRole == "Teacher")
            {
                throw new UnauthorizedAccessException("You are not authorized to do this action.");
            }

            Subject createdSubject = new Subject
            {
                Title = subjectToBeCreated.Title,
                Teachers = subjectToBeCreated.Teachers,
                Grades = subjectToBeCreated?.Grades
            };

            if (createdSubject.Id == null || createdSubject.Id == 0)
            {
                Random random = new Random();
                createdSubject.Id = random.Next(1, int.MaxValue);
            }

            _context.Subjects.Add(createdSubject);
            await _context.SaveChangesAsync();

            return createdSubject;
        }

        public async Task<Subject> UpdateSubjectAsync(int id, Subject subjectToBeUpdated, int userId)
        {
            string userRole = await _userService.GetUserRole(userId);

            if (userRole == "Student" || userRole == "Teacher")
            {
                throw new UnauthorizedAccessException("You are not authorized to do this action.");
            }

            Subject updatedSubject = await _context.Subjects
                .Include(sub => sub.Teachers)
                .Include(sub => sub.Grades)
                .FirstOrDefaultAsync(sub => sub.Id == id);

            if (updatedSubject == null) throw new KeyNotFoundException($"Subject with ID {id} not found.");
            
            updatedSubject.Title = subjectToBeUpdated.Title;
            updatedSubject.Teachers = subjectToBeUpdated.Teachers;
            updatedSubject.Grades = subjectToBeUpdated.Grades;
            
            _context.Subjects.Update(updatedSubject);
            await _context.SaveChangesAsync();

            return updatedSubject;
        }

        public async Task<bool> DeleteSubjectAsync(int id, int userId)
        {
            string userRole = await _userService.GetUserRole(userId);

            if (userRole == "Student" || userRole == "Teacher")
            {
                throw new UnauthorizedAccessException("You are not authorized to do this action.");
            }

            Subject subjectToBeDeleted = await _context.Subjects.FindAsync(id);

            if (subjectToBeDeleted == null) throw new KeyNotFoundException($"Subject with ID {id} not found.");

            _context.Subjects.Remove(subjectToBeDeleted);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
