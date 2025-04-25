using Microsoft.AspNetCore.Mvc;
using SchoolManagement.API.Interfaces;
using SchoolManagement.API.Models;

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
        public async Task<ActionResult<IEnumerable<Grade>>> GetAllGrades()
        {
            IEnumerable<Grade> grades = await _gradeService.GetGradesAsync();

            if (grades == null || !grades.Any()) return NoContent();

            return Ok(grades);
        }

        // GET api/<GradeController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Grade>> GetGrade(int id)
        {
            Grade grade = await _gradeService.GetGradeByIdAsync(id);

            return Ok(grade);
        }

        // POST api/<GradeController>
        [HttpPost]
        public async Task<ActionResult<Grade>> PostGrade(Grade gradeToBeCreated)
        {
            Grade createdGrade = await _gradeService.CreateGradeAsync(gradeToBeCreated);

            return CreatedAtAction("GetGrade", new { id = createdGrade.Id }, createdGrade);
        }

        // PUT api/<GradeController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Grade>> PutGrade(int id, Grade gradeToBeUpdated)
        {
            Grade updatedGrade = await _gradeService.UpdateGradeAsync(id, gradeToBeUpdated, gradeToBeUpdated.StudentId, gradeToBeUpdated.SubjectId);

            return Ok(updatedGrade);
        }

        // DELETE api/<GradeController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteGrade(int id)
        {
            await _gradeService.DeleteGradeAsync(id);

            return NoContent();
        }
    }
}
