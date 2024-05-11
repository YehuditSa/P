namespace BlApi;

/// <summary>
///  An interface that contains the interfaces of all the primary logical entities 
/// </summary>
public interface IBl
{
    public IEngineer Engineer { get; }
    public ITask Task { get; }
    public IMilestone Milestone { get; }
    public void InitializeDB();
    public void ResetDB();
    public void addDependency(int dependentTask, int dependensOnTask);

}


