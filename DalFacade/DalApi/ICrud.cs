
namespace DalApi;
using DO;

/// <summary>
/// An interface that contains the CRUD methods for entity T
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ICrud<T> where T : class
{
    int Create(T item); //Creates new entity object in DAL
    T? Read(int id); //Reads entity object by its ID 
    T? Read(Func<T?, bool> filter); //Reads entity object by a filter function
    IEnumerable<T?> ReadAll(Func<T?, bool>? filter = null); //Reads all entities object with option of reading by a filter function 
    void Update(T item); //Updates entity object
    void Delete(int id); //Deletes an object by its Id
    void ResetAll(); //Resets all data of the entity 
}
