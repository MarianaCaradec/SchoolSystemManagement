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

            IQueryable<Subject> query = _context.Subjects.Include(sub => sub.Teachers);

            if(userRole == "Teacher")
            {
                query = query.Where(sub => sub.Teachers.Any(t => t.Id == userId));
            }
            
            if(userRole == "Student")
            {
                query = query.Include(sub => sub.Grades)
                .Where(sub => sub.Grades.Any(g => g.StudentId == userId));
            }

            return await query.Select(sub => new Subject
            {
                Id = sub.Id,
                Title = sub.Title,
                Teachers = sub.Teachers.ToList(),
                Grades = sub.Grades.ToList(),
            }).ToListAsync(); ;
        }

        public async Task<Subject> GetSubjectByIdAsync(int id, int userId)
        {
            string userRole = await _userService.GetUserRole(userId);

            IQueryable<Subject> query = _context.Subjects
                .Include(sub => sub.Teachers)
                .Include(sub => sub.Grades);

            if (userRole == "Teacher")
            {
                query = query.Where(sub => sub.Teachers.Any(t => t.Id == userId));
            }
            
            if (userRole == "Student")
            {
                query = query.Where(sub => sub.Grades.Any(g => g.StudentId == userId));
            }

            Subject subject = await query.FirstOrDefaultAsync(sub => sub.Id == id);

            if (subject == null) throw new KeyNotFoundException($"Subject with ID {id} not found.");
            
            return subject;
        }

        public async Task<Subject> CreateSubjectAsync(Subject subjectToBeCreated, int userId)
        {
            string userRole = await _userService.GetUserRole(userId);

            if (userRole != "Admin")
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

            if (userRole != "Admin")
            {
                throw new UnauthorizedAccessException("You are not authorized to do this action.");
            }

            Subject subject = await _context.Subjects
                .Include(sub => sub.Teachers)
                .Include(sub => sub.Grades)
                .FirstOrDefaultAsync(sub => sub.Id == id);

            if (subject == null) throw new KeyNotFoundException($"Subject with ID {id} not found.");
            
            subject.Title = subjectToBeUpdated.Title;
            subject.Teachers = subjectToBeUpdated.Teachers;
            subject.Grades = subjectToBeUpdated.Grades;
            
            _context.Subjects.Update(subject);
            await _context.SaveChangesAsync();

            return subject;
        }

        public async Task<bool> DeleteSubjectAsync(int id, int userId)
        {
            string userRole = await _userService.GetUserRole(userId);

            if (userRole != "Admin")
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
