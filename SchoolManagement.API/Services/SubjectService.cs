using Microsoft.EntityFrameworkCore;
using SchoolManagement.API.Data.Context;
using SchoolManagement.API.Interfaces;
using SchoolManagement.API.Models;

namespace SchoolManagement.API.Services
{
    public class SubjectService(SchoolSysDBContext context) : ISubjectService
    {
        private readonly SchoolSysDBContext _context = context;

        public async Task<IEnumerable<Subject>> GetAllSubjectsAsync()
        {
            return await _context.Subjects
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

        public async Task<Subject> GetSubjectByIdAsync(int id)
        {
            Subject subjectById = await _context.Subjects
                .Include(sub => sub.Teachers)
                .Include(sub => sub.Grades)
                .FirstOrDefaultAsync(sub => sub.Id == id);

            if (subjectById == null) throw new KeyNotFoundException($"Subject with ID {id} not found.");
            
            return subjectById;
        }

        public async Task<Subject> CreateSubjectAsync(Subject subjectToBeCreated)
        {
            Subject createdSubject = new Subject
            {
                Title = subjectToBeCreated.Title,
                Teachers = subjectToBeCreated.Teachers,
                Grades = subjectToBeCreated.Grades
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

        public async Task<Subject> UpdateSubjectAsync(int id, Subject subjectToBeUpdated)
        {
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

        public async Task<bool> DeleteSubjectAsync(int id)
        {
            Subject subjectToBeDeleted = await _context.Subjects.FindAsync(id);

            if (subjectToBeDeleted == null) throw new KeyNotFoundException($"Subject with ID {id} not found.");

            _context.Subjects.Remove(subjectToBeDeleted);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
