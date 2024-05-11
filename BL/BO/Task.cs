namespace BO;

/// <summary>
/// Primary logical entity for Task
/// </summary>
public class Task
{
    public int Id { get; init; }
    public string? Description { get; set; }
    public string? Alias { get; set; }
    public DateTime CreatedAtDate { get; init; }
    public Status? Status { get; set; } 
    public List<TaskInList>? Dependencies { get; set; }
    public MilestoneInTask? Milestone { get; set; }
    public TimeSpan? RequiredEffortTime { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? ScheduledDate { get; init; }
    public DateTime? ForecastDate { get; set; }
    public DateTime? DeadLineDate { get; set; }
    public DateTime? CompleteDate { get; set; }
    public string? Deliverables { get; set; }
    public string? Remarks { get; set; }
    public EngineerInTask? Engineer { get; set; }
    public EngineerExperience? ComplexityLevel { get; set; }
    public override string ToString() => this.ToStringProperty();

}




