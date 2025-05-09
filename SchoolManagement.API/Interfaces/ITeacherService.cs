﻿using SchoolManagement.API.DTOs;
using SchoolManagement.API.Models;

namespace SchoolManagement.API.Interfaces
{
    public interface ITeacherService
    {
        Task<IEnumerable<TeacherDto>> GetTeachersAsync(int userId);
        Task<TeacherDto> GetTeacherByIdAsync(int id, int userId);
        Task<TeacherInputDto> CreateTeacherAsync(TeacherInputDto teacherToBeCreated, int userId);
        Task<Teacher> UpdateTeacherAsync(int id, Teacher teacherToBeUpdated, int userId);
        Task<bool> DeleteTeacherAsync(int id, int userId);
    }
}
