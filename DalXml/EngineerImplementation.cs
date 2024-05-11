namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Data.Common;

/// <summary>
/// A class that implements the CRUD methods for Engineer entity
/// </summary>
internal class EngineerImplementation : IEngineer
{

    /// <summary>
    /// Creates a new Engineer in engineers.xml
    /// </summary>
    /// <param name="item">An engineer</param>
    /// <returns>The ID of the new engineer</returns>
    /// <exception cref="DalAlreadyExistsException"></exception>
    public int Create(Engineer item)
    {
        List<Engineer> engineersList = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");

        if (Read(item.Id) is not null) //If this id already exist in the engineer's list
            throw new DalAlreadyExistsException($"Engineer with ID={item.Id} already exists");
        engineersList.Add(item);

        XMLTools.SaveListToXMLSerializer<Engineer>(engineersList, "engineers");

        return item.Id;
    }

    /// <summary>
    /// Deletes an engineer by its Id
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int id)
    {
        List<Engineer> engineersList = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");

        Engineer? engForDelete = Read(id);
        if (engForDelete is not null)
        {
            engineersList.Remove(engForDelete);
        }
        else  //If this id doesn't exist in the engineer's list
        {
            throw new DalDoesNotExistException($"Engineer with ID={id} doesn't exist");
        }

        XMLTools.SaveListToXMLSerializer<Engineer>(engineersList, "engineers");
    }

    /// <summary>
    /// Reads an engineer by its Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>The required engineer or null - if it doesn't exist</returns>
    public Engineer? Read(int id)
    {
        List<Engineer> engineersList = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");
        return (from eng in engineersList
                where eng.Id == id
                select eng).FirstOrDefault();
    }

    /// <summary>
    /// Reads an engineer by a filter function
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>The required engineer or null - if it doesn't exist</returns>
    public Engineer? Read(Func<Engineer, bool> filter)
    {
        List<Engineer> engineersList = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");
        return engineersList.Where(filter).FirstOrDefault();
    }

    /// <summary>
    /// Reads all engineers with option of reading by a filter function 
    /// </summary>
    /// <param name="filter">The filter function</param>
    /// <returns>A list of the reqiered engineers or all engineers - if there is no filter function</returns>
    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
        List<Engineer> engineersList = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");

        if (filter == null)
            return engineersList.Select(item => item).ToList();
        else
            return engineersList.Where(filter).ToList();
    }


    /// Updates an engineer in engineers list - by its Id
    /// </summary>
    /// <param name="item">The new engineer</param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Update(Engineer item)
    {
        List<Engineer> engineersList = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");

        if (Read(item.Id) is not null)
        {
            engineersList.Remove(Read(item.Id)!);
            engineersList.Add(item);
        }
        else  //If this id doesn't exist in the engineer's list
        {

            throw new DalDoesNotExistException($"Engineer with ID={item.Id} doesn't exist");
        }

        XMLTools.SaveListToXMLSerializer<Engineer>(engineersList, "engineers");

    }

    /// <summary>
    /// Resets the engineers data
    /// </summary>
    public void ResetAll()
    {
        List<Engineer> engineersList = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");
        engineersList.Clear();
        XMLTools.SaveListToXMLSerializer<Engineer>(engineersList, "engineers");
    }

}


