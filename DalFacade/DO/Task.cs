namespace DO;

/// <summary>
/// Task Entity
/// </summary>
/// <param name="Id">Iunique ID (created automatically)</param>
/// <param name="Description"></param>
/// <param name="Alias"></param>
/// <param name="IsMilestone"></param>
/// <param name="CreatedAtDate">Task Creation Date</param>
/// <param name="RequiredEffortTime">How many days needed to the task</param>
/// <param name="StartDate">Start Date of Work on Task</param>
/// <param name="ScheduledDate">Scheduled date for completion.(In the First Plane)</param>
/// <param name="ForecastDate">Forecast updated date for completion</param>
/// <param name="DeadLineDate">Possible final end date</param>
/// <param name="CompleteDate">Actual end date</param>
/// <param name="Deliverables">The Product of the Task</param>
/// <param name="Remarks"></param>
/// <param name="EngineerId">The engineer ID assigned to the task</param>
/// <param name="ComplexityLevel">The difficulty level of the task</param>
public record Task
(
    int Id,
    bool IsMilestone,
    DateTime CreatedAtDate,
    TimeSpan? RequiredEffortTime = null,
    string? Deliverables = null,
    int? EngineerId = null,
    EngineerExperience? ComplexityLevel = null
)
{
    public string Description { get; set; } = string.Empty;
    public string Alias { get; set; } = string.Empty;
    public DateTime? StartDate { get; set; } = null;
    public DateTime? ScheduledDate { get; set; } = null;
    public DateTime? ForecastDate { get; set; } = null;
    public DateTime? DeadLineDate { get; set; } = null;
    public DateTime? CompleteDate { get; set; } = null;
    public string? Remarks { get; set; } = null;
    public Task() : this(0, false, new(1900, 1, 1)) { }//empty ctor

    public Task(string description, string alias, bool isMilestone, DateTime createdAtDate) : //parametrize ctor
        this(0, isMilestone, createdAtDate, null, null, null)
    {
        Alias = alias;
        Description = description;
    }

};



