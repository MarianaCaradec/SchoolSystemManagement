﻿using Microsoft.EntityFrameworkCore;
using SchoolManagement.API.Data.Context;
using SchoolManagement.API.DTOs;
using SchoolManagement.API.Interfaces;
using SchoolManagement.API.Models;
using static SchoolManagement.API.Models.User;

namespace SchoolManagement.API.Services
{
    public class ClassService(SchoolSysDBContext context, IUserService userService) : IClassService
    {
        private readonly SchoolSysDBContext _context = context;
        private readonly IUserService _userService = userService;

        public async Task<IEnumerable<ClassResponseDto>> GetClassesAsync(int userId)
        {
            UserRole userRole = await _userService.GetUserRole(userId);

            if (userRole == UserRole.Student)
            {
                throw new UnauthorizedAccessException("You are not authorized to do this action.");
            }

            IQueryable<Class> query = _context.Classes.Include(c => c.Teachers).Include(c => c.Students);

            if(userRole == UserRole.Teacher)
            {
                query = query.Where(c => c.Teachers.Any(t => t.UserId == userId));
            }

            return await query.Select(c => new ClassResponseDto
            {
                Id = c.Id,
                Course = c.Course,
                Divition = c.Divition,
                Capacity = c.Capacity,
                Teachers = c.Teachers.Select(t => new TeacherResponseDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Surname = t.Surname,
                }).ToList(),
                Students = c.Students.Select(s => new StudentDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Surname = s.Surname,
                }).ToList()
            }).ToListAsync();
        }

        public async Task<ClassResponseDto> GetClassByIdAsync(int id, int userId)
        {
            UserRole userRole = await _userService.GetUserRole(userId);

            IQueryable<Class> query = _context.Classes.Include(c => c.Teachers).Include(c => c.Students);

            Class classById = await query.FirstOrDefaultAsync(c => c.Id == id);

            if (classById == null) throw new KeyNotFoundException($"Class with ID {id} not found.");

            if (userRole == UserRole.Teacher && !classById.Teachers.Any(t => t.UserId == userId))
            {
                throw new UnauthorizedAccessException("You are not authorized to do this action.");
            }

            if(userRole == UserRole.Student && !classById.Students.Any(s => s.UserId == userId))
            {
                throw new UnauthorizedAccessException("You are not authorized to do this action.");
            }

            return new ClassResponseDto
            {
                Id = classById.Id,
                Course = classById.Course,
                Divition = classById.Divition,
                Capacity = classById.Capacity,
                Teachers = classById.Teachers.Select(t => new TeacherResponseDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Surname = t.Surname,
                    MobileNumber = t.MobileNumber,
                    Address = t.Address,
                    UserId = t.UserId,
                }).ToList(),
                Students = classById.Students.Select(s => new StudentDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Surname = s.Surname,
                }).ToList()
            };
        }

        public async Task<ClassInputDto> CreateClassAsync(ClassInputDto classToBeCreated, int userId)
        {
            UserRole userRole = await _userService.GetUserRole(userId);

            if (userRole != UserRole.Admin)
            {
                throw new UnauthorizedAccessException("You are not authorized to do this action.");
            }

            Class createdClass = new Class
            {
                Course = classToBeCreated.Course,
                Divition = classToBeCreated.Divition,
                Capacity = classToBeCreated.Capacity,
                Teachers = classToBeCreated.TeacherId != 0
                ? new List<Teacher> { await _context.Teachers.FindAsync(classToBeCreated.TeacherId) }
                : null,
                Students = classToBeCreated.StudentId != 0
                ? new List<Student> { await _context.Students.FindAsync(classToBeCreated.StudentId) }
                : null

            };

            _context.Classes.Add(createdClass);
            await _context.SaveChangesAsync();

            return new ClassInputDto
            {
                Id = createdClass.Id,
                Course = createdClass.Course,
                Divition = createdClass.Divition,
                Capacity = createdClass.Capacity,
                TeacherId = classToBeCreated.TeacherId,
                StudentId = classToBeCreated.StudentId
            };
        }

        public async Task<ClassInputDto> UpdateClassAsync(int id, ClassInputDto classToBeUpdated, int userId)
        {
            UserRole userRole = await _userService.GetUserRole(userId);

            if (userRole != UserRole.Admin)
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
            classById.Teachers = classToBeUpdated.TeacherId != 0
            ? new List<Teacher> { await _context.Teachers.FindAsync(classToBeUpdated.TeacherId) }
            : null;
            classById.Students = classToBeUpdated.StudentId != 0
            ? new List<Student> { await _context.Students.FindAsync(classToBeUpdated.StudentId) }
            : null;


            _context.Classes.Update(classById);
            await _context.SaveChangesAsync();

            return new ClassInputDto
            {
                Id = classById.Id,
                Course = classById.Course,
                Divition = classById.Divition,
                Capacity = classById.Capacity,
                TeacherId = classToBeUpdated.TeacherId,
                StudentId = classToBeUpdated.StudentId
            };
        }

        public async Task<bool> DeleteClassAsync(int id, int userId)
        {
            UserRole userRole = await _userService.GetUserRole(userId);

            if (userRole != UserRole.Admin)
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
