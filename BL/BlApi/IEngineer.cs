namespace BlApi;

/// <summary>
///  An interface that contains the CRUD methods for Logic Engineer entity
/// </summary>
public interface IEngineer
{
    public int Create(BO.Engineer boEngineer);
    public BO.Engineer Read(int id);
    public IEnumerable<BO.Engineer>? ReadAll(Func<DO.Engineer?, bool>? filter = null); 
    public void Delete(int  id);
    public void Update(BO.Engineer boEngineer);
}
