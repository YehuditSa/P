namespace Dal;
using DalApi;
using DO;


/// <summary>
/// A class that implements the CRUD methods for Engineer entity
/// </summary>
internal class EngineerImplementation : IEngineer
{
    /// <summary>
    /// Creates a new Engineer in DAL
    /// </summary>
    /// <param name="item">An engineer</param>
    /// <returns>The ID of the new engineer</returns>
    /// <exception cref="DalAlreadyExistsException"></exception>
    public int Create(Engineer item)
    {
        if (Read(item.Id) is not null) //If this id already exist in the engineer's list
            throw new DalAlreadyExistsException($"Engineer with ID={item.Id} already exists");
        DataSource.Engineers.Add(item);
        return item.Id;
    }

    /// <summary>
    /// Deletes an engineer by its Id
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int id)
    {
        Engineer? engForDelete = Read(id);
        if (engForDelete is not null)
        {
            DataSource.Engineers.Remove(engForDelete);
        }
        else  //If this id doesn't exist in the engineer's list
        {
            throw new DalDoesNotExistException($"Engineer with ID={id} doesn't exist");
        }
    }

    /// <summary>
    /// Reads an engineer by its Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>The required engineer or null - if it doesn't exist</returns>
    public Engineer? Read(int id)
    {
        return (from eng in DataSource.Engineers
                where eng.Id == id
                select eng).FirstOrDefault();
    }

    /// <summary>
    /// Reads an engineer by a filter function
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>The required engineer or null - if it doesn't exist</returns>
    public Engineer? Read(Func<Engineer?, bool> filter)
    {
        return DataSource.Engineers.Where(filter).FirstOrDefault();
    }

    /// <summary>
    /// Reads all engineers with option of reading by a filter function 
    /// </summary>
    /// <param name="filter">The filter function</param>
    /// <returns>A list of the reqiered engineers or all engineers - if there is no filter function</returns>
    public IEnumerable<Engineer?> ReadAll(Func<Engineer?, bool>? filter = null)
    {
        if (filter == null)
            return DataSource.Engineers.Select(item => item).ToList();
        else
            return DataSource.Engineers.Where(filter).ToList();
    }

    /// <summary>
    /// Updates an engineer in engineers list - by its Id
    /// </summary>
    /// <param name="item">The new engineer</param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Update(Engineer item)
    {
        if (Read(item.Id) is not null)
        {
            Delete(item.Id);
            DataSource.Engineers.Add(item);
        }
        else  //If this id doesn't exist in the engineer's list
        {
            throw new DalDoesNotExistException($"Engineer with ID={item.Id} doesn't exist");
        }
    }
    /// <summary>
    /// Resets the list of engineers 
    /// </summary>
    public void ResetAll() { DataSource.Engineers.Clear(); }
}
