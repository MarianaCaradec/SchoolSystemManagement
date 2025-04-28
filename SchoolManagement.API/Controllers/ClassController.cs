using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SchoolManagement.API.Data.Seeds;
using SchoolManagement.API.Interfaces;
using SchoolManagement.API.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SchoolManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController(IClassService classService) : ControllerBase
    {

        private readonly IClassService _classService = classService;

        // GET: api/<ClassController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Class>>> GetAllClasses(int userId)
        {
            IEnumerable<Class> classes = await _classService.GetClassesAsync(userId);

            if (classes == null || !classes.Any()) return NoContent();

            return Ok(classes);
        }

        // GET api/<ClassController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Class>> GetClass(int id, int userId)
        {
            Class classById = await _classService.GetClassByIdAsync(id, userId);

            return Ok(classById);
        }

        // POST api/<ClassController>
        [HttpPost]
        public async Task<ActionResult<Class>> PostClass(Class classToBeCreated, int userId)
        {
            Class createdClass = await _classService.CreateClassAsync(classToBeCreated, userId);

            return CreatedAtAction("GetClass", new { id = createdClass.Id }, createdClass);
        }

        // PUT api/<ClassController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Class>> PutClass(int id, Class classToBeUpdated, int userId)
        {
            Class updatedClass = await _classService.UpdateClassAsync(id, classToBeUpdated, userId);
            
            return Ok(updatedClass);
        }

        // DELETE api/<ClassController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteClass(int id, int userId)
        {
            await _classService.DeleteClassAsync(id, userId);

            return NoContent();
        }
    }
}
