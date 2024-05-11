using DalApi;
namespace Dal;

/// <summary>
/// A class that contains the interfaces of all the entities that exist in the project
/// </summary>
sealed internal class DalList : IDal
{
    public static IDal Instance { get; } = new DalList();
    private DalList() { }

    public IEngineer Engineer => new EngineerImplementation();

    public IDependency Dependency => new DependencyImplementation();

    public ITask Task => new TaskImplementation();

    public void Reset()
    {
        Engineer.ResetAll();
        Dependency.ResetAll();
        Task.ResetAll();
    }

    public DateTime? StartProjectDate
    {
        get => DataSource.Config.startProjectDate;
        set => DataSource.Config.startProjectDate = value;
    }

    public DateTime? EndProjectDate
    {
        get => DataSource.Config.endProjectDate;
        set => DataSource.Config.endProjectDate = value;
    }
}
