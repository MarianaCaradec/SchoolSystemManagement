using Microsoft.EntityFrameworkCore;
using SchoolManagement.API.Data.Context;
using SchoolManagement.API.Interfaces;
using SchoolManagement.API.Models;

namespace SchoolManagement.API.Services
{
    public class GradeService(SchoolSysDBContext context) : IGradeService
    {
        private readonly SchoolSysDBContext _context = context;

        public async Task<IEnumerable<Grade>> GetGradesAsync()
        {
            return await _context.Grades
                .Include(g => g.Student)
                .Select(g => new Grade
                {
                    Id = g.Id,
                    Value = g.Value,
                    Date = g.Date,
                    Student = g.Student,
                    Subject = g.Subject

                }) .ToListAsync();
        }

        public async Task<Grade> GetGradeByIdAsync(int id)
        {
            Grade grade = await _context.Grades
                .Include(g => g.Student)
                .FirstOrDefaultAsync(g => g.Id == id);

            if(grade == null) throw new KeyNotFoundException($"Grade with ID {id} not found");

            return grade;
        }

        public async Task<Grade> CreateGradeAsync(Grade gradeToBeCreated)
        {
            Grade createdGrade = new Grade
            {
                Value = gradeToBeCreated.Value,
                Date = gradeToBeCreated.Date,
                StudentId = gradeToBeCreated.StudentId,
                SubjectId = gradeToBeCreated.SubjectId
            };

            if(createdGrade.Id == null || createdGrade.Id == 0)
            {
                Random random = new Random();
                createdGrade.Id = random.Next(1, int.MaxValue);
            }

            _context.Grades.Add(createdGrade);
            await _context.SaveChangesAsync();

            return createdGrade;
        }

        public async Task<Grade> UpdateGradeAsync(int id, Grade gradeToBeUpdated, int studentId, int subjectId)
        {
            Grade grade = await _context.Grades.FindAsync(id);

            if (grade == null) throw new KeyNotFoundException($"Grade with ID {id} not found");

            grade.Value = gradeToBeUpdated.Value;
            grade.Date = gradeToBeUpdated.Date;
            grade.StudentId = studentId;
            grade.SubjectId = subjectId;

            _context.Grades.Update(grade);
            await _context.SaveChangesAsync();

            return grade;
        }

        public async Task<bool> DeleteGradeAsync(int id)
        {
            Grade gradeToBeDeleted = await _context.Grades.FindAsync(id);

            if (gradeToBeDeleted == null) throw new KeyNotFoundException($"Grade with ID {id} not found");
            
            _context.Grades.Remove(gradeToBeDeleted);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
