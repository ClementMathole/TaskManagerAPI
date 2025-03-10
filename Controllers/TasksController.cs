using System.Security.Claims;
using CloudBased_TaskManagementAPI.DTOs;
using CloudBased_TaskManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CloudBased_TaskManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly TaskService _taskService;

        public TaskController(TaskService taskService)
        {
            // Name      : TaskController(TaskService taskService)
            // Purpose   : Initializes the controller with a task service via dependency injection.
            // Re-use    : Instantiated when the controller is used.
            // Input     : TaskService taskService
            //            - instance of task service.
            // Output    : Sets the private variable _taskService.
            _taskService = taskService;
        } // end constructor

        private int GetUserId() =>
            int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? throw new InvalidOperationException("User ID claim not found")
            ); // Retrieves the user ID from the claims in the authenticated user's identity.

        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            // Name      : GetTasks
            // Purpose   : Retrieves all tasks associated with the authenticated user.
            // Re-use    : Called to get the task list for a specific user.
            // Input     : None.
            // Output    : Returns a list of tasks in JSON format.
            var tasks = await _taskService.GetTasksAsync(GetUserId());
            return Ok(tasks);
        } // end GetTasks

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask(int id)
        {
            // Name      : GetTask(int id)
            // Purpose   : Retrieves a specific task by its ID if it belongs to the authenticated user.
            // Re-use    : Called to fetch details of a particular task.
            // Input     : int id - ID of the task to retrieve.
            // Output    : Returns the task details or NotFound if not found.
            var task = await _taskService.GetTaskByIdAsync(id, GetUserId());
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        } // end GetTask

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskDto taskDto)
        {
            // Name      : CreateTask([FromBody] TaskDto taskDto)
            // Purpose   : Creates a new task for the authenticated user.
            // Re-use    : Called when a user wants to add a task.
            // Input     : TaskDto taskDto
            //            - task details.
            // Output    : Returns the created task with a 201 response.
            var task = await _taskService.CreateTaskAsync(taskDto, GetUserId());
            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        } // end CreateTask

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskDto taskDto)
        {
            // Name      : UpdateTask(int id, [FromBody] TaskDto taskDto)
            // Purpose   : Updates an existing task if it belongs to the authenticated user.
            // Re-use    : Called when a user wants to modify a task.
            // Input     : int id
            //            - task ID,
            //             TaskDto taskDto
            //            - updated task details.
            // Output    : Returns 204 No Content on success, or 404 Not Found if the task does not exist.
            var updated = await _taskService.UpdateTaskAsync(id, taskDto, GetUserId());
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        } // end UpdateTask

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            // Name      : DeleteTask(int id)
            // Purpose   : Deletes a specific task if it belongs to the authenticated user.
            // Re-use    : Called when a user wants to remove a task.
            // Input     : int id
            //            - ID of the task to delete.
            // Output    : Returns 204 No Content on success, or 404 Not Found if the task does not exist.
            var deleted = await _taskService.DeleteTaskAsync(id, GetUserId());
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        } // end DeleteTask
    } // end TasksController
}
