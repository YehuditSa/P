namespace BlApi;

/// <summary>
/// An interface that contains the CRUD methods for Logic Task entity
/// </summary>
public interface ITask
{
    public int Create(BO.Task task);
    public BO.Task Read(int id);
    public IEnumerable<BO.Task?> ReadAll(Func<BO.Task?, bool>? filter = null);
    public void Delete(int id);
    public void Update(BO.Task engineer);


}
