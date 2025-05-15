using Microsoft.EntityFrameworkCore;
using SchoolManagement.API.Data.Context;
using SchoolManagement.API.DTOs;
using SchoolManagement.API.Interfaces;
using SchoolManagement.API.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static SchoolManagement.API.Models.User;

namespace SchoolManagement.API.Services
{
    public class GradeService(SchoolSysDBContext context, IUserService userService) : IGradeService
    {
        private readonly SchoolSysDBContext _context = context;
        private readonly IUserService _userService = userService;

        public async Task<IEnumerable<GradeDto>> GetGradesAsync(int userId)
        {
            UserRole userRole = await _userService.GetUserRole(userId);

            IQueryable<Grade> query = _context.Grades.Include(g => g.Student).Include(g => g.Subject);

            if (userRole == UserRole.Teacher)
            {
                if (!query.Where(g => g.Subject.Teachers.Any(t => t.UserId == userId)).Any())
                {
                    throw new UnauthorizedAccessException("You are not authorized to do this action.");
                }

                query = query.Where(g => g.Subject.Teachers.Any(t => t.UserId == userId));
            }

            if (userRole == UserRole.Student)
            {
                query = query.Where(g => g.StudentId == userId);
            }

            return await query
                .Select(g => new GradeDto
            {
                Id = g.Id,
                Value = g.Value,
                Date = g.Date,
                StudentId = g.StudentId,
                SubjectId = g.SubjectId
            }).ToListAsync(); ;
        }

        public async Task<GradeResponseDto> GetGradeByIdAsync(int id, int userId)
        {
            UserRole userRole = await _userService.GetUserRole(userId);

            IQueryable<Grade> query = _context.Grades
                .Include(g => g.Student)
                .Include(g => g.Subject);

            if (userRole == UserRole.Teacher)
            {
                if (!query.Where(g => g.Subject.Teachers.Any(t => t.UserId == userId)).Any())
                {
                    throw new UnauthorizedAccessException("You are not authorized to do this action.");
                }
                query = query.Where(g => g.Subject.Teachers.Any(t => t.UserId == userId));
            }

            GradeResponseDto grade = await query.Select( g => new GradeResponseDto
            {
                Id = g.Id,
                Value = g.Value,
                Date = g.Date,
                Student = new StudentDto
                {
                    Id = g.Student.Id,
                    Name = g.Student.Name,
                    Surname = g.Student.Surname,
                },
                Subject = new SubjectDto
                {
                    Id = g.Subject.Id,
                    Title = g.Subject.Title
                }
            }).FirstOrDefaultAsync(g => g.Id == id);

            if (grade == null) throw new KeyNotFoundException($"Grade with ID {id} not found");

            if (userRole == UserRole.Student && grade.Student.Id != userId)
            {
                throw new UnauthorizedAccessException("You are not authorized to do this action.");
            }

            return grade;
        }

        public async Task<GradeDto> CreateGradeAsync(GradeDto gradeToBeCreated, int userId)
        {
            UserRole userRole = await _userService.GetUserRole(userId);

            if (userRole == UserRole.Student)
            {
                throw new UnauthorizedAccessException("You are not authorized  to do this action.");
            }

            Grade createdGradeToBeSaved = new Grade
            {
                Value = gradeToBeCreated.Value,
                Date = gradeToBeCreated.Date,
                StudentId = gradeToBeCreated.StudentId,
                SubjectId = gradeToBeCreated.SubjectId
            };

            //Trying eager loading for perfomance
            Student studentWithClassAndTeachers = await _context.Students
                    .Include(s => s.Class.Teachers)
                    .ThenInclude(t => t.Subjects)
                    .FirstOrDefaultAsync(s => s.Id == createdGradeToBeSaved.StudentId);

            if (studentWithClassAndTeachers == null)
            {
                throw new ArgumentException("Invalid student or class information.");
            }

            if (userRole == UserRole.Teacher &&
                !studentWithClassAndTeachers.Class.Teachers.Any(t => t.UserId == userId &&
                t.Subjects.Any(s => s.Id == createdGradeToBeSaved.SubjectId)))
            {
                throw new UnauthorizedAccessException("You are not authorized to do this action.");
            }

            _context.Grades.Add(createdGradeToBeSaved);
            await _context.SaveChangesAsync();

            return new GradeDto
            {
                Id = createdGradeToBeSaved.Id,
                Value = createdGradeToBeSaved.Value,
                Date = createdGradeToBeSaved.Date,
                StudentId = createdGradeToBeSaved.StudentId,
                SubjectId = createdGradeToBeSaved.SubjectId
            };
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
