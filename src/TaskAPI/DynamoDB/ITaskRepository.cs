using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskAPI.Model;

namespace TaskAPI.DynamoDB
{
    public interface ITaskRepository
    {
        Task<TaskItem> GetByIdAsync(string taskId);
        Task DeleteByIdAsync(string taskId);
        Task AddOrUpdateAsync(TaskItem task);
        Task<List<TaskItem>> GetAllAsync();
    }
}