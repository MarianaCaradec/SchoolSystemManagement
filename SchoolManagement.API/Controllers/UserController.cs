using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.API.Interfaces;
using SchoolManagement.API.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SchoolManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [Authorize(Roles = "Admin")]
        // GET: api/<UserController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            IEnumerable<User> users = await _userService.GetUsersAsync();

            if (users == null || !users.Any()) return NoContent();

            return Ok(users);
        }

        // GET api/<UserController>/5
        [HttpGet("{email}")]
        public async Task<ActionResult<User>> GetUser(string email)
        {
            User user = await _userService.GetUserByEmailAsync(email);

            if (user == null) return NotFound($"User with the email {email} not found.");

            return Ok(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserRole(int id)
        {
            string userRole = await _userService.GetUserRole(id);

            return Ok(userRole);
        }

        // POST api/<UserController>
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User userToBeCreated)
        {
            User createdUser = await _userService.CreateUserAsync(userToBeCreated);

            return CreatedAtAction("GetUser", new { email = createdUser.Email}, createdUser);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> PutUser(int id, User userToBeUpdated)
        {
            User updatedUser = await _userService.UpdateUserAsync(id, userToBeUpdated);

            return Ok(updatedUser);
        }

        [Authorize(Roles = "Admin")]
        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _userService.DeleteUserAsync(id);

            return NoContent();
        }
    }
}
