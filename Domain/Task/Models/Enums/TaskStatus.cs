using System.Text.Json.Serialization;

namespace Domain.Task.Models.Enums;
[JsonConverter(typeof(JsonStringEnumConverter<TaskStatus>))]
public enum TaskStatus
{
    NotStarted,
    InProgress,
    Completed,
    OnHold,
    Cancelled,
}