﻿using SchoolManagement.API.DTOs;
using SchoolManagement.API.Models;

namespace SchoolManagement.API.Interfaces
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentResponseDto>> GetStudentsAsync(int userId);
        Task<StudentResponseDto> GetStudentByIdAsync(int id, int userId);
        Task<StudentInputDto> CreateStudentAsync(StudentInputDto studentToBeCreated, int userId);
        Task<StudentInputDto> UpdateStudentAsync(int id, StudentInputDto studentToBeUpdated, int userId);
        Task<bool> DeleteStudentAsync(int id, int userId);
    }
}
