using Microsoft.AspNetCore.Mvc;
using SchoolManagement.API.DTOs;
using SchoolManagement.API.Interfaces;
using SchoolManagement.API.Models;
using System.Security.Cryptography.Pkcs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SchoolManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradeController(IGradeService gradeService) : ControllerBase
    {
        private readonly IGradeService _gradeService = gradeService;

        // GET: api/<GradeController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GradeDto>>> GetAllGrades(int userId)
        {
            IEnumerable<GradeDto> grades = await _gradeService.GetGradesAsync(userId);

            if (grades == null || !grades.Any()) return NoContent();

            return Ok(grades);
        }

        // GET api/<GradeController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GradeResponseDto>> GetGrade(int id, int userId)
        {
            GradeResponseDto grade = await _gradeService.GetGradeByIdAsync(id, userId);

            return Ok(grade);
        }

        // POST api/<GradeController>
        [HttpPost]
        public async Task<ActionResult<GradeDto>> PostGrade(GradeDto gradeToBeCreated, int userId)
        {
            GradeDto createdGrade = await _gradeService.CreateGradeAsync(gradeToBeCreated, userId);

            return CreatedAtAction("GetGrade", new { id = createdGrade.Id }, createdGrade);
        }

        // PUT api/<GradeController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<GradeDto>> PutGrade(int id, GradeDto gradeToBeUpdated, int userId)
        {

            GradeDto updatedGrade = await _gradeService.UpdateGradeAsync(id, gradeToBeUpdated, userId);

            return Ok(updatedGrade);

        }

        // DELETE api/<GradeController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteGrade(int id, int userId)
        {
            await _gradeService.DeleteGradeAsync(id, userId);

            return NoContent();
        }
    }
}
