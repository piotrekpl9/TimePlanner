using Domain.Shared;

namespace Domain.Task.TaskErrors;

public class TaskError(string code, string description) : Error(code, description)
{
    public static TaskError TaskAlreadyDeleted => new TaskError("TaskAlreadyDeleted", "Task is already deleted!");
    public static TaskError TaskNotFound => new TaskError("TaskNotFound", "Task not found!");
    public static TaskError TaskIsNotAssignedToUser => new TaskError("RequestedTaskIsNotAssignedToUser", "Requested task is not assigned to user!");
}