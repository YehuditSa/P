namespace BlApi;

/// <summary>
/// An interface that contains the methods for Logic Milestone entity
/// </summary>
public interface IMilestone
{
    public void CreateProjectSchedule(DateTime startProjectDate, DateTime endProjectDate);
    public BO.Milestone Read(int id);
    public BO.Milestone Update(int id, string? description = null, string? alias = null, string? remarks = null);
    public IEnumerable<BO.Milestone?> ReadAll(Func<BO.Milestone?, bool>? filter = null);

}
