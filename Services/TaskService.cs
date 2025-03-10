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
            _context = context;
        }

        public async Task<List<TaskItem>> GetTasksAsync(int userId) =>
            await _context.Tasks.Where(t => t.UserId == userId).ToListAsync();

        public async Task<TaskItem?> GetTaskByIdAsync(int taskId, int userId) =>
            await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);

        public async Task<TaskItem> CreateTaskAsync(TaskDto taskDto, int userId)
        {
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
        }

        public async Task<bool> UpdateTaskAsync(int taskId, TaskDto taskDto, int userId)
        {
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
        }

        public async Task<bool> DeleteTaskAsync(int taskId, int userId)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t =>
                t.Id == taskId && t.UserId == userId
            );
            if (task == null)
                return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
