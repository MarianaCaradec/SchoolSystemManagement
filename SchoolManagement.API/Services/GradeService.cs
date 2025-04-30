using Microsoft.EntityFrameworkCore;
using SchoolManagement.API.Data.Context;
using SchoolManagement.API.Interfaces;
using SchoolManagement.API.Models;
using static SchoolManagement.API.Models.User;

namespace SchoolManagement.API.Services
{
    public class GradeService(SchoolSysDBContext context, IUserService userService) : IGradeService
    {
        private readonly SchoolSysDBContext _context = context;
        private readonly IUserService _userService = userService;

        public async Task<IEnumerable<Grade>> GetGradesAsync(int userId)
        {
            UserRole userRole = await _userService.GetUserRole(userId);

            IQueryable<Grade> query = _context.Grades.Include(g => g.Student).Include(g => g.Subject);

            if (userRole == UserRole.Student)
            {
                query = query.Where(g => g.StudentId == userId);
            }

            return await query
                .Select(g => new Grade
            {
                Id = g.Id,
                Value = g.Value,
                Date = g.Date,
                Student = g.Student,
                Subject = g.Subject
            }).ToListAsync(); ;
        }

        public async Task<Grade> GetGradeByIdAsync(int id, int userId)
        {
            UserRole userRole = await _userService.GetUserRole(userId);

            Grade grade = await _context.Grades
                .Include(g => g.Student)
                .Include(g => g.Subject)
                .FirstOrDefaultAsync(g => g.Id == id);

            if(grade == null) throw new KeyNotFoundException($"Grade with ID {id} not found");

            if (userRole == UserRole.Student && grade.StudentId != userId)
            {
                throw new UnauthorizedAccessException("You are not authorized to do this action.");
            }

            return grade;
        }

        public async Task<Grade> CreateGradeAsync(Grade gradeToBeCreated, int userId)
        {
            UserRole userRole = await _userService.GetUserRole(userId);

            if (userRole == UserRole.Student)
            {
                throw new UnauthorizedAccessException("You are not authorized to do this action.");
            }
            //Trying eager loading for perfomance
            Student studentWithClassAndTeachers = await _context.Students
                    .Include(s => s.Class.Teachers)
                    .FirstOrDefaultAsync(s => s.Id == gradeToBeCreated.StudentId);

            if (studentWithClassAndTeachers == null)
            {
                throw new ArgumentException("Invalid student or class information.");
            }

            if (userRole == UserRole.Teacher && !studentWithClassAndTeachers.Class.Teachers.Any(t => t.Id == userId))
            {
                throw new UnauthorizedAccessException("You are not authorized to do this action.");
            }

            Grade createdGrade = new Grade
            {
                Value = gradeToBeCreated.Value,
                Date = gradeToBeCreated.Date,
                StudentId = gradeToBeCreated.StudentId,
                SubjectId = gradeToBeCreated.SubjectId
            };

            _context.Grades.Add(createdGrade);
            await _context.SaveChangesAsync();

            return createdGrade;
        }

        public async Task<Grade> UpdateGradeAsync(int id, Grade gradeToBeUpdated, int studentId, int subjectId, int userId)
        {
            UserRole userRole = await _userService.GetUserRole(userId);

            if (userRole == UserRole.Student)
            {
                throw new UnauthorizedAccessException("You are not authorized to do this action.");
            }

            Grade grade = await _context.Grades
                .Include(g => g.Student)
                .Include(g => g.Subject)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (grade == null) throw new KeyNotFoundException($"Grade with ID {id} not found");

            if (userRole == UserRole.Teacher)
            {
                if(!grade.Student.Class.Teachers.Any(t => t.Id == userId))
                {
                    throw new UnauthorizedAccessException("You are not authorized to do this action.");

                }

                DateTime currentDate = DateTime.Now;
                DateTime gradeDate = grade.Date.ToDateTime(TimeOnly.MinValue);


                if ((currentDate - gradeDate).TotalDays >= 21)
                {
                    throw new InvalidOperationException("Grade update is not allowed after three weeks.");
                }
            }

            grade.Value = gradeToBeUpdated.Value;
            grade.Date = gradeToBeUpdated.Date;
            grade.StudentId = studentId;
            grade.SubjectId = subjectId;

            _context.Grades.Update(grade);
            await _context.SaveChangesAsync();

            return grade;
        }

        public async Task<bool> DeleteGradeAsync(int id, int userId)
        {
            UserRole userRole = await _userService.GetUserRole(userId);

            if (userRole != UserRole.Admin)
            {
                throw new UnauthorizedAccessException("You are not authorized to do this action.");
            }

            Grade gradeToBeDeleted = await _context.Grades.FindAsync(id);

            if (gradeToBeDeleted == null) throw new KeyNotFoundException($"Grade with ID {id} not found");
            
            _context.Grades.Remove(gradeToBeDeleted);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
