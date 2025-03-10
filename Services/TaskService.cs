using CloudBased_TaskManagementAPI.Data;
using CloudBased_TaskManagementAPI.DTOs;
using CloudBased_TaskManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CloudBased_TaskManagementAPI.Services
{
    public class TaskService
    {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context)
        {
            // Name      : TaskService(AppDbContext context)
            // Purpose   : Initializes the task service with the database context.
            // Re-use    : Injected into controllers for task-related operations.
            // Input     : AppDbContext context
            //            - The database context.
            // Output    : Initializes _context for database operations.
            _context = context;
        } // end constructor

        public async Task<List<TaskItem>> GetTasksAsync(int userId) =>
            await _context.Tasks.Where(t => t.UserId == userId).ToListAsync(); // Retrieves all tasks belonging to a specific user.

        public async Task<TaskItem?> GetTaskByIdAsync(int taskId, int userId) =>
            await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId); // Retrieves a specific task by ID and user ID.

        public async Task<TaskItem> CreateTaskAsync(TaskDto taskDto, int userId)
        {
            // Name      : CreateTaskAsync(TaskDto taskDto, int userId)
            // Purpose   : Creates a new task for the user.
            // Re-use    : Called when a user adds a new task.
            // Input     : TaskDto taskDto
            //            - Data transfer object containing task details.
            //             int userId
            //            - The ID of the user creating the task.
            // Output    : The created task.
            var task = new TaskItem
            {
                Title = taskDto.Title,
                Description = taskDto.Description,
                IsCompleted = taskDto.IsCompleted,
                UserId = userId,
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        } // end CreateTaskAsync

        public async Task<bool> UpdateTaskAsync(int taskId, TaskDto taskDto, int userId)
        {
            // Name      : UpdateTaskAsync(int taskId, TaskDto taskDto, int userId)
            // Purpose   : Updates an existing task for the user.
            // Re-use    : Called when a user modifies a task.
            // Input     : int taskId
            //            - The ID of the task to update.
            //             TaskDto taskDto
            //            - DTO containing updated task details.
            //             int userId
            //            - The ID of the user making the update.
            // Output    : True if task was updated successfully, otherwise false.
            var task = await _context.Tasks.FirstOrDefaultAsync(t =>
                t.Id == taskId && t.UserId == userId
            );
            if (task == null)
                return false;
            task.Title = taskDto.Title;
            task.Description = taskDto.Description;
            task.IsCompleted = taskDto.IsCompleted;

            await _context.SaveChangesAsync();
            return true;
        } // end UpdateTaskAsync

        public async Task<bool> DeleteTaskAsync(int taskId, int userId)
        {
            // Name      : DeleteTaskAsync(int taskId, int userId)
            // Purpose   : Deletes a task owned by the user.
            // Re-use    : Called when a user removes a task.
            // Input     : int taskId
            //            - The ID of the task to delete.
            //             int userId
            //            - The ID of the user deleting the task.
            // Output    : True if task was deleted successfully, otherwise false.
            var task = await _context.Tasks.FirstOrDefaultAsync(t =>
                t.Id == taskId && t.UserId == userId
            );
            if (task == null)
                return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        } //end DeleteTaskAsync
    } // end TaskService
}
