

namespace DalApi;
/// <summary>
/// An interface that contains the interfaces of all the entities that exist in the project and the project dates
/// </summary>
public interface IDal
{
    IEngineer Engineer { get; }
    IDependency Dependency { get; }
    ITask Task { get; }
    void Reset();
    DateTime? StartProjectDate { get; set; }
    DateTime? EndProjectDate { get; set; }
}
