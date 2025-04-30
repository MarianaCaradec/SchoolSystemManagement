using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.API.Interfaces;
using SchoolManagement.API.Models;
using static SchoolManagement.API.Models.User;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SchoolManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        // GET: api/<UserController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers(int userId)
        {
            IEnumerable<User> users = await _userService.GetUsersAsync(userId);

            if (users == null || !users.Any()) return NoContent();

            return Ok(users);
        }

        // GET api/<UserController>/5
        [HttpGet("{email}")]
        public async Task<ActionResult<User>> GetUser(string email, int userId)
        {
            User user = await _userService.GetUserByEmailAsync(email, userId);

            if (user == null) return NotFound($"User with the email {email} not found.");

            return Ok(user);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserRole>> GetUserRole(int id)
        {
            UserRole userRole = await _userService.GetUserRole(id);

            return Ok(userRole);
        }

        // POST api/<UserController>
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User userToBeCreated, int userId)
        {
            User createdUser = await _userService.CreateUserAsync(userToBeCreated, userId);

            return CreatedAtAction("GetUser", new { email = createdUser.Email}, createdUser);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> PutUser(int id, User userToBeUpdated, int userId)
        {
            User updatedUser = await _userService.UpdateUserAsync(id, userToBeUpdated, userId);

            return Ok(updatedUser);
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id, int useId)
        {
            await _userService.DeleteUserAsync(id, useId);

            return NoContent();
        }
    }
}
