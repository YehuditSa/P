
namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Xml.Linq;

/// <summary>
/// A class that implements the CRUD methods for Dependency entity
/// </summary>
internal class DependencyImplementation : IDependency
{
    /// <summary>
    /// Creates a new Dependency in dependencies.xml
    /// </summary>
    /// <param name="item">A dependecy with meaningless id</param>
    /// <returns>The new ID of the new dependency</returns>
    public int Create(Dependency item)
    {
        XElement root = XMLTools.LoadListFromXMLElement("dependencies");

        int id = Config.NextDependencyId; //Creating Id - a running number
        root.Add(new XElement("Dependency",
                new XElement("Id", id),
                (item.DependentTask is not null) ? new XElement("DependentTask", item.DependentTask) : null,
                (item.DependsOnTask is not null) ? new XElement("DependsOnTask", item.DependsOnTask) : null));

        XMLTools.SaveListToXMLElement(root, "dependencies");
        return id;
    }

    /// <summary>
    /// Deletes a dependency by its Id
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int id)
    {
        XElement root = XMLTools.LoadListFromXMLElement("dependencies");

        Dependency? depForDelete = Read(id);

        if (depForDelete is not null)
            root.Descendants("Id")
                .FirstOrDefault(depId => Convert.ToInt32(depId.Value)
                .Equals(id))?.Parent!
                .Remove();

        else  //If this id doesn't exist in the dependencies list
            throw new DalDoesNotExistException($"Dependency with ID={id} doesn't exist");


        XMLTools.SaveListToXMLElement(root, "dependencies");


    }

    /// <summary>
    /// Reads a dependency by its Id 
    /// </summary>
    /// <param name="id"></param>
    /// <returns>The required dependency or null - if it doesn't exist</returns>
    public Dependency? Read(int id)
    {
        XElement root = XMLTools.LoadListFromXMLElement("dependencies");


        XElement? requiredDep = (from dep in root.Descendants("Dependency")
                                 let depId = Convert.ToInt32(dep.Descendants("Id").First().Value)
                                 where depId.Equals(id)
                                 select dep).ToList().First();


        if (requiredDep is not null)
            return new Dependency(Convert.ToInt32(requiredDep.Descendants("Id").First().Value),
                                 (requiredDep.Descendants("DependentTask").FirstOrDefault()?.Value is null) ? null : Convert.ToInt32(requiredDep.Descendants("DependentTask").FirstOrDefault()?.Value),
                                 (requiredDep.Descendants("DependsOnTask").FirstOrDefault()?.Value is null) ? null : Convert.ToInt32(requiredDep.Descendants("DependsOnTask").FirstOrDefault()?.Value));
        return null;
    }

    /// <summary>
    /// Reads a dependency by a filter function
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>The required dependency or null - if it doesn't exist</returns>
    public Dependency? Read(Func<Dependency, bool> filter)
    {
        XElement root = XMLTools.LoadListFromXMLElement("dependencies");

        return root.Descendants("Dependency")
                   .Select(dep =>
                   {
                       return new Dependency(Convert.ToInt32(dep.Descendants("Id").First().Value),
                              (dep.Descendants("DependentTask").FirstOrDefault()?.Value is null) ? null : Convert.ToInt32(dep.Descendants("DependentTask").FirstOrDefault()?.Value),
                              (dep.Descendants("DependsOnTask").FirstOrDefault()?.Value is null) ? null : Convert.ToInt32(dep.Descendants("DependsOnTask").FirstOrDefault()?.Value)
                       );
                   })
                  .Where(dep => filter(dep)).ToList().First();


    }

    /// <summary>
    /// Reads all dependencies with option of reading by a filter function 
    /// </summary>
    /// <param name="filter">The filter function</param>
    /// <returns>A list of the reqiered dependencies or all dependencies - if there is no filter function</returns>
    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {

        XElement root = XMLTools.LoadListFromXMLElement("dependencies");

        if (filter == null)
            return root.Descendants("Dependency")
                   .Select(dep =>
                   {
                       return new Dependency(Convert.ToInt32(dep.Descendants("Id").First().Value),
                              (dep.Descendants("DependentTask").FirstOrDefault()?.Value is null) ? null : Convert.ToInt32(dep.Descendants("DependentTask").FirstOrDefault()?.Value),
                              (dep.Descendants("DependsOnTask").FirstOrDefault()?.Value is null) ? null : Convert.ToInt32(dep.Descendants("DependsOnTask").FirstOrDefault()?.Value)
                       );
                   }).ToList();

        else
            return root.Descendants("Dependency")
                    .Select(dep =>
                    {
                        return new Dependency(Convert.ToInt32(dep.Descendants("Id").First().Value),
                               (dep.Descendants("DependentTask").FirstOrDefault()?.Value is null) ? null : Convert.ToInt32(dep.Descendants("DependentTask").FirstOrDefault()?.Value),
                               (dep.Descendants("DependsOnTask").FirstOrDefault()?.Value is null) ? null : Convert.ToInt32(dep.Descendants("DependsOnTask").FirstOrDefault()?.Value)
                        );
                    })
                   .Where(dep => filter(dep)).ToList();

    }

    /// <summary>
    /// Updates a dependency in dependencies list - by its Id
    /// </summary>
    /// <param name="item">The new dependency </param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Update(Dependency item)
    {
        XElement root = XMLTools.LoadListFromXMLElement("dependencies");


        if (Read(item.Id) is not null)
        {

            var currentDepTag = root.Descendants("Id")
                                     !.FirstOrDefault(depId => Convert.ToInt32(depId.Value)
                                     .Equals(item.Id))!.Parent;


            if (currentDepTag?.Descendants("DependentTask").FirstOrDefault() is null && item.DependentTask is not null)
            {
                root.Descendants("Id")
                    !.FirstOrDefault(depId => Convert.ToInt32(depId.Value)
                    .Equals(item.Id))!.Parent!.Add(new XElement("DependentTask", item.DependentTask));
            }
            else if (item.DependentTask is not null)
            {

                root.Descendants("Id")
                   !.FirstOrDefault(depId => Convert.ToInt32(depId.Value)
                   .Equals(item.Id))!.Parent!.Descendants("DependentTask").First().SetValue(item.DependentTask ?? Read(item.Id)!.DependentTask!);
            }


            if (currentDepTag?.Descendants("DependsOnTask").FirstOrDefault() is null && item.DependsOnTask is not null)
            {
                root.Descendants("Id")
                    !.FirstOrDefault(depId => Convert.ToInt32(depId.Value)
                    .Equals(item.Id))!.Parent!.Add(new XElement("DependsOnTask", item.DependsOnTask));
            }
            else if (item.DependsOnTask is not null)
            {
                root.Descendants("Id")
                     !.FirstOrDefault(depId => Convert.ToInt32(depId.Value)
                     .Equals(item.Id))!.Parent!.Descendants("DependsOnTask").First().SetValue(item.DependsOnTask ?? Read(item.Id)!.DependsOnTask!);
            }


        }
        else  //If this id doesn't exist in the dependencies list
            throw new DalDoesNotExistException($"Dependency with ID={item.Id} doesn't exist");


        XMLTools.SaveListToXMLElement(root, "dependencies");

    }

    /// <summary>
    /// Resets the dependencies data
    /// </summary>
    public void ResetAll()
    {
        XElement root = XMLTools.LoadListFromXMLElement("dependencies");
        root.Descendants("Dependency").Remove();
        XMLTools.SaveListToXMLElement(root, "dependencies");
    }
}
