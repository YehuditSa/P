
namespace BlImplementation;
using BlApi;

/// <summary>
///  Implementates interfaces of all the primary logical entities 
/// </summary>
internal class Bl : IBl
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task =>  new TaskImplementation();

    public IMilestone Milestone => new MilestoneImplementation();

    public void InitializeDB() => DalTest.Initialization.Do();

    public void ResetDB() => _dal.Reset();

    public void addDependency(int dependentTask, int dependensOnTask)
    {
        try
        {
            _dal.Dependency.Create(new DO.Dependency(dependentTask, dependensOnTask));
        }
        catch
        {
            throw new BO.BlUnknownException($"Unknown Exception in Create the new Engineer");
        }
    }


}
