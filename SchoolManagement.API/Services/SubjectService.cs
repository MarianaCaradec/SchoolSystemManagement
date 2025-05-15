using Microsoft.EntityFrameworkCore;
using SchoolManagement.API.Data.Context;
using SchoolManagement.API.DTOs;
using SchoolManagement.API.Interfaces;
using SchoolManagement.API.Models;
using static SchoolManagement.API.Models.User;

namespace SchoolManagement.API.Services
{
    public class SubjectService(SchoolSysDBContext context, IUserService userService) : ISubjectService
    {
        private readonly SchoolSysDBContext _context = context;
        private readonly IUserService _userService = userService;

        public async Task<IEnumerable<SubjectResponseDto>> GetSubjectsAsync(int userId)
        {
            UserRole userRole = await _userService.GetUserRole(userId);

            IQueryable<Subject> query = _context.Subjects.Include(sub => sub.Teachers);

            if (userRole == UserRole.Teacher)
            {
                bool isTeacherAssociated = await _context.Subjects
                    .Where(sub => sub.Teachers.Any(t => t.UserId == userId))
                    .AnyAsync();

                if (!isTeacherAssociated)
                {
                    throw new UnauthorizedAccessException("Sorry, you are not a teacher of this subject.");
                }
                else
                {
                    query = query.Where(sub => sub.Teachers.Any(t => t.UserId == userId));
                }
            }

            if (userRole == UserRole.Student)
            {
                query = query.Include(sub => sub.Grades)
                .Where(sub => sub.Grades.Any(g => g.Student.UserId == userId));
            }

            return await query.Select(sub => new SubjectResponseDto
            {
                Id = sub.Id,
                Title = sub.Title,
                Teachers = sub.Teachers.Select(t => new TeacherResponseDto 
                { 
                    Id = t.Id, 
                    Name = t.Name, 
                    Surname = t.Surname, 
                    MobileNumber = t.MobileNumber 
                }).ToList(),
                Grades = userRole == UserRole.Admin || userRole == UserRole.Student ? 
                sub.Grades.Select(g => new Grade
                {
                    Id = g.Id,
                    Value = g.Value,
                    Date = g.Date,
                    StudentId = g.StudentId
                }).ToList() 
                : null,
            }).ToListAsync();
        }

        public async Task<SubjectResponseDto> GetSubjectByIdAsync(int id, int userId)
        {
            UserRole userRole = await _userService.GetUserRole(userId);

            IQueryable<Subject> query = _context.Subjects.Include(sub => sub.Teachers);

            if (userRole == UserRole.Teacher)
            {
                bool isTeacherAssociated = await _context.Subjects
                    .Where(sub => sub.Id == id && sub.Teachers.Any(t => t.UserId == userId))
                    .AnyAsync();

                if (!isTeacherAssociated)
                {
                    throw new UnauthorizedAccessException("Sorry, you are not a teacher of this subject.");
                } else
                {
                    query = query.Where(sub => sub.Teachers.Any(t => t.UserId == userId));
                }
            }

            SubjectResponseDto subject = await query.Select(sub => new SubjectResponseDto
            {
                Id = sub.Id,
                Title = sub.Title,
                Teachers = sub.Teachers.Select(t => new TeacherResponseDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Surname = t.Surname,
                    MobileNumber = t.MobileNumber
                }).ToList(),
                Grades = userRole == UserRole.Admin || (userRole == UserRole.Student &&
                  query.Where(sub => sub.Grades.Any(g => g.Student.UserId == userId)).Any()) ?
                sub.Grades.Select(g => new Grade
                {
                    Id = g.Id,
                    Value = g.Value,
                    Date = g.Date,
                    SubjectId = g.SubjectId,
                }).ToList()
                : null,
            }).FirstOrDefaultAsync(sub => sub.Id == id);

            if (subject == null) throw new KeyNotFoundException($"Subject with ID {id} not found.");
            
            return subject;
        }

        public async Task<SubjectInputDto> CreateSubjectAsync(SubjectInputDto subjectToBeCreated, int userId)
        {
            UserRole userRole = await _userService.GetUserRole(userId);

            if (userRole != UserRole.Admin)
            {
                throw new UnauthorizedAccessException("You are not authorized to do this action.");
            }

            Subject createdSubjectToBeSaved = new Subject
            {
                Title = subjectToBeCreated.Title,
                Teachers = subjectToBeCreated.Teachers?.Select(t => new Teacher
                {
                    Id = t.Id,
                    Name = t.Name,
                    Surname = t.Surname,
                    MobileNumber = t.MobileNumber
                }).ToList(),
                Grades = subjectToBeCreated.Grades?.Select(g => new Grade
                {
                    Id = g.Id,
                    Value = g.Value,
                    Date = g.Date,
                    StudentId = g.StudentId
                }).ToList()
            };

            _context.Subjects.Add(createdSubjectToBeSaved);
            await _context.SaveChangesAsync();

            SubjectInputDto createdSubject = new SubjectInputDto
            {
                Id = createdSubjectToBeSaved.Id,
                Title = createdSubjectToBeSaved.Title,
                Teachers = createdSubjectToBeSaved.Teachers?.Select(t => new TeacherResponseDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Surname = t.Surname,
                    MobileNumber = t.MobileNumber
                }).ToList(),
                Grades = createdSubjectToBeSaved.Grades?.Select(g => new GradeDto
                {
                    Id = g.Id,
                    Value = g.Value,
                    Date = g.Date,
                    StudentId = g.StudentId,
                    SubjectId = g.SubjectId
                }).ToList()
            };

            return createdSubject;
        }

        public async Task<SubjectInputDto> UpdateSubjectAsync(int id, SubjectInputDto subjectToBeUpdated, int userId)
        {
            UserRole userRole = await _userService.GetUserRole(userId);

            if (userRole != UserRole.Admin)
            {
                throw new UnauthorizedAccessException("You are not authorized to do this action.");
            }

            Subject subject = await _context.Subjects
                .Include(sub => sub.Teachers)
                .Include(sub => sub.Grades)
                .FirstOrDefaultAsync(sub => sub.Id == id);

            if (subject == null) throw new KeyNotFoundException($"Subject with ID {id} not found.");

            subject.Title = subjectToBeUpdated.Title;
            subject.Teachers = subjectToBeUpdated.Teachers?.Select(t => new Teacher
            {
                Id = t.Id,
                Name = t.Name,
                Surname = t.Surname,
                MobileNumber = t.MobileNumber,
                Address = t.Address,
                UserId = t.UserId
            }).ToList();
            subject.Grades = subject.Grades?.Select(g => new Grade
            {
                Id = g.Id,
                Value = g.Value,
                Date = g.Date,
                StudentId = g.StudentId,
                SubjectId = g.SubjectId
            }).ToList();

            _context.Update(subject);
            await _context.SaveChangesAsync();

            return new SubjectInputDto
            {
                Title = subject.Title,
                Teachers = subject.Teachers.Select(t => new TeacherResponseDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Surname = t.Surname,
                    MobileNumber = t.MobileNumber,
                    Address = t.Address,
                    UserId = t.UserId
                }).ToList(),
                Grades = subject.Grades.Select(g => new GradeDto
                {
                    Id = g.Id,
                    Value = g.Value,
                    Date = g.Date,
                    StudentId = g.StudentId,
                    SubjectId = g.SubjectId
                }).ToList()
            };
        }

        public async Task<bool> DeleteSubjectAsync(int id, int userId)
        {
            UserRole userRole = await _userService.GetUserRole(userId);

            if (userRole != UserRole.Admin)
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
