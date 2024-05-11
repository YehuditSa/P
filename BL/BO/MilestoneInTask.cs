namespace BO;

/// <summary>
/// Logical auxiliary entity - Represents a milestone in Task Entity 
/// </summary>
public class MilestoneInTask
{
    public int Id { get; init; }
    public string? Alias { get; set; }
    public override string ToString() => this.ToStringProperty();

}
