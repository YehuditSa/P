
namespace Dal;
using DalApi;
using DO;

/// <summary>
/// A class that implements the CRUD methods for Dependency entity
/// </summary>
internal class DependencyImplementation : IDependency
{
    /// <summary>
    /// Creates a new Dependency in DAL
    /// </summary>
    /// <param name="item">A dependecy with meaningless id</param>
    /// <returns>The new ID of the new dependency</returns>
    public int Create(Dependency item)
    {
        int id = DataSource.Config.NextDependencyId; //Creating Id - a running number
        Dependency tempItem = new Dependency(id, item.DependentTask, item.DependsOnTask);
        DataSource.Dependencies.Add(tempItem);
        return id;
    }

    /// <summary>
    /// Deletes a dependency by its Id
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int id)
    {
        Dependency? depForDelete = Read(id);
        if (depForDelete is not null)
        {
            DataSource.Dependencies.Remove(depForDelete);
        }
        else  //If this id doesn't exist in the dependencies list
        {
            throw new DalDoesNotExistException($"Dependency with ID={id} doesn't exist");
        }
    }

    /// <summary>
    /// Reads a dependency by its Id 
    /// </summary>
    /// <param name="id"></param>
    /// <returns>The required dependency or null - if it doesn't exist</returns>
    public Dependency? Read(int id)
    {
        return (from dep in DataSource.Dependencies
                where dep.Id == id
                select dep).FirstOrDefault();
    }

    /// <summary>
    /// Reads a dependency by a filter function
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>The required dependency or null - if it doesn't exist</returns>
    public Dependency? Read(Func<Dependency?, bool> filter)
    {
        return DataSource.Dependencies.Where(filter).FirstOrDefault();
    }

    /// <summary>
    /// Reads all dependencies with option of reading by a filter function 
    /// </summary>
    /// <param name="filter">The filter function</param>
    /// <returns>A list of the reqiered dependencies or all dependencies - if there is no filter function</returns>
    public IEnumerable<Dependency?> ReadAll(Func<Dependency?, bool>? filter = null)
    {
        if (filter == null)
        {
            IEnumerable<Dependency?> x = DataSource.Dependencies.Select(item => item).ToList();
            return x;
        }
        else
            return DataSource.Dependencies.Where(filter).ToList();
    }

    /// <summary>
    /// Updates a dependency in dependencies list - by its Id
    /// </summary>
    /// <param name="item">The new dependency </param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Update(Dependency item)
    {
        if (Read(item.Id) is not null)
        {
            Delete(item.Id);
            DataSource.Dependencies.Add(item);
        }
        else  //If this id doesn't exist in the dependencies list
        {
            throw new DalDoesNotExistException($"Dependency with ID={item.Id} doesn't exist");
        }

    }
    /// <summary>
    /// Resets the list of dependencies 
    /// </summary>
    public void ResetAll() { DataSource.Dependencies.Clear(); }
}
