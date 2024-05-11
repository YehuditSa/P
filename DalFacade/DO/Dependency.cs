namespace DO;
/// <summary>
/// Dependency Entity
/// </summary>
/// <param name="Id">unique ID (created automatically)</param>
/// <param name="DependentTask">ID number of pending task</param>
/// <param name="DependsOnTask">Previous Task ID number</param>
public record Dependency
(
    int Id,
    int? DependentTask = null,
    int? DependsOnTask = null
)
{
    Dependency() : this(0) { } //empty ctor
    public Dependency(int dependentTask, int dependsOnTask) : this(0, dependentTask, dependsOnTask)
    {
        DependentTask = dependentTask;
        DependsOnTask = dependsOnTask;
    }
};

