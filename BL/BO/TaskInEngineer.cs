namespace BO;

/// <summary>
/// Logical auxiliary entity - Represents a task in Engineer Entity 
/// </summary>
public class TaskInEngineer
{
    public int Id { get; init; }
    public string? Alias { get; set; }
    public override string ToString() => this.ToStringProperty();

}

