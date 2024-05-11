namespace BO;

/// <summary>
/// Logical auxiliary entity - Represents an engineer in Task Entity 
/// </summary>
public class EngineerInTask
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public override string ToString() => this.ToStringProperty();
}
