using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Dtos;
using TaskManagerAPI.Models;
using TaskManagerAPI.Repositories.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskManagerAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;

        public TasksController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        // GET: api/<TasksController>
        [HttpGet]
        public async Task<IActionResult> GetAllTasksByUser(Guid userId, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetAllTasksByUser(userId, cancellationToken);

            if (task == null || !task.Any())
            {
                return NotFound($"No tasks found for user with ID: {userId}");
            }

            var result = task.Select(task => new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description!,
                Priority = task.Priority,
                Status = task.Status,
            });

            return Ok(result);
        }

        // GET api/<TasksController>/5
        [HttpGet("{taskId}")]
        public async Task<IActionResult> GetById(Guid taskId, Guid userId, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetTaskById(taskId, userId, cancellationToken);

            if (task == null)
                return NotFound();

            return Ok(new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description!,
                Priority = task.Priority,
                Status = task.Status,
            });
        }

        // POST api/<TasksController>
        [HttpPost("register")]
        public async Task<IActionResult> Post([FromBody] Tasks task, Guid userId, CancellationToken cancellationToken)
        {

            task = new Tasks
            {
                Title = task.Title,
                Description = task.Description,
                Priority = task.Priority,
                Status = task.Status,
                DueTime = task.DueTime,
                UserId = task.UserId
            };

            var result = await _taskRepository.CreateTask(task, userId, cancellationToken);

            if (result == null)
                return BadRequest(result);


            var response = new TaskResponseDto
            {
                Title = task.Title,
                Description = task.Description!,
            };

            return Ok(response);
        }

        // PUT api/<TasksController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TasksController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}