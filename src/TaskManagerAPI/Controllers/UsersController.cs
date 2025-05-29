using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskManagerAPI.Dtos;
using TaskManagerAPI.Models;
using TaskManagerAPI.Models.Enums;
using TaskManagerAPI.Repositories.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET api/<UsersController>/5
        [HttpGet]
        public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAllUsers(cancellationToken);

            if (user == null)
                return NotFound();

            var result = user.Select(user => new UserResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            });
            return Ok(result);
        }

        // GET: api/<UsersController>
        [HttpGet("id")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserById(id, cancellationToken);

            if (user == null)
                return NotFound();

            return Ok(new UserResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            });
        }

        // POST api/<UsersController>
        [HttpPost("register")]
        public async Task<IActionResult> Post([FromBody] RegisterUserDto userDto, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                PasswordHash = userDto.Password,
                Role = Role.User
            };

            var result = await _userRepository.CreateUser(user, cancellationToken);
            if (result == null)
                return BadRequest(result);

            var response = new UserResponseDto
            {
                Id = result.Id,
                Email = result.Email,
                Role = result.Role
            };

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, response);
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserDto userDto, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Id = id,
                Name = userDto.Name,
                Email = userDto.Email,
                PasswordHash = userDto.Password,
                Role = userDto.Role
            };

            var updated = await _userRepository.UpdateUser(user, cancellationToken);

            if (updated == null)
                return NotFound();

            return Ok(new UserResponseDto
            {
                Id = updated.Id,
                Name = updated.Name,
                Email = updated.Email,
                Role = updated.Role
            });
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id, CancellationToken cancellationToken)
        {
            var remove = await _userRepository.DeleteUser(id, cancellationToken);

            if (remove == null)
                return NotFound();

            return NoContent();
        }
    }
}