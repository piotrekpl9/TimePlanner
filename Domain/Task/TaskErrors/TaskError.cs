using Domain.Shared;

namespace Domain.Task.TaskErrors;

public class TaskError(string name, string code, string description) : Error(name, code, description)
{
    public static TaskError TaskAlreadyDeleted => new TaskError("TaskAlreadyDeleted", "TASK_1","Task is already deleted!");
    public static TaskError TaskNotFound => new TaskError("TaskNotFound", "TASK_2","Task not found!");
    public static TaskError TaskIsNotAssignedToUser => new TaskError("RequestedTaskIsNotAssignedToUser", "TASK_3","Requested task is not assigned to user!");
}