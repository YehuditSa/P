namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;



/// <summary>
/// A class that implements the CRUD methods for Task entity
/// </summary>
internal class TaskImplementation : ITask
{
    /// <summary>
    /// Creates a new Task in DAL
    /// </summary>
    /// <param name="item">A task with meaningless id</param>
    /// <returns>The new ID of the new task</returns>
    public int Create(Task item)
    {
        List<Task> tasksList = XMLTools.LoadListFromXMLSerializer<Task>("tasks");

        int id = Config.NextTaskId; //Creating Id - a running number
        Task tempItem = new Task()
        {
            Id = id,
            Description = item.Description,
            Alias = item.Alias,
            IsMilestone = item.IsMilestone,
            CreatedAtDate = item.CreatedAtDate,
            RequiredEffortTime = item.RequiredEffortTime,
            StartDate = item.StartDate,
            ScheduledDate = item.ScheduledDate,
            ForecastDate = item.ForecastDate,
            DeadLineDate = item.DeadLineDate,
            CompleteDate = item.CompleteDate,
            Deliverables = item.Deliverables,
            Remarks = item.Remarks,
            EngineerId = item.EngineerId,
            ComplexityLevel = item.ComplexityLevel
        };
        tasksList.Add(tempItem);

        XMLTools.SaveListToXMLSerializer<Task>(tasksList, "tasks");

        return id;

    }

    /// <summary>
    /// Deletes a task by its Id
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int id)
    {
        List<Task> tasksList = XMLTools.LoadListFromXMLSerializer<Task>("tasks");
        Task? taskForDelete = Read(id);

        if (taskForDelete is not null)
            tasksList.Remove(taskForDelete);

        else  //If this id doesn't exist in the task's list
            throw new DalDoesNotExistException($"Task with ID={id} doesn't exist");


        XMLTools.SaveListToXMLSerializer<Task>(tasksList, "tasks");
    }

    /// <summary>
    /// Reads a task by its Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>The required task or null - if it doesn't exist</returns>
    public Task? Read(int id)
    {
        List<Task> tasksList = XMLTools.LoadListFromXMLSerializer<Task>("tasks");

        return (from task in tasksList
                where task.Id == id
                select task).FirstOrDefault();
    }

    /// <summary>
    /// Reads a task by a filter function
    /// </summary>
    /// <param name="filter">The required task or null - if it doesn't exist</param>
    /// <returns>The required task or null - if it doesn't exist</returns>
    public Task? Read(Func<Task, bool> filter)
    {
        List<Task> tasksList = XMLTools.LoadListFromXMLSerializer<Task>("tasks");

        return tasksList.Where(filter).FirstOrDefault();
    }

    /// <summary>
    /// Reads all tasks with option of reading by a filter function 
    /// </summary>
    /// <param name="filter">The filter function</param>
    /// <returns>A list of the reqiered tasks or all tasks - if there is no filter function</returns>
    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null)
    {
        List<Task> tasksList = XMLTools.LoadListFromXMLSerializer<Task>("tasks");

        if (filter == null)
            return tasksList.Select(item => item).ToList();
        else
            return tasksList.Where(filter).ToList();
    }

    /// <summary>
    /// Updates a task in tasks list - by its Id
    /// </summary>
    /// <param name="updateTask">The new Task</param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Update(Task updateTask)
    {
        List<Task> tasksList = XMLTools.LoadListFromXMLSerializer<Task>("tasks");

        if (Read(updateTask.Id) is not null)
        {
            tasksList.Remove(Read(updateTask.Id)!);
            tasksList.Add(updateTask);
        }
        else  //If this id doesn't exist in the task's list
        {
            throw new DalDoesNotExistException($"Task with ID={updateTask.Id} doesn't exist");
        }

        XMLTools.SaveListToXMLSerializer<Task>(tasksList, "tasks");
    }

    /// <summary>
    /// Resets the tasks data
    /// </summary>
    public void ResetAll()
    {
        List<Task> tasksList = XMLTools.LoadListFromXMLSerializer<Task>("tasks");
        tasksList.Clear();
        XMLTools.SaveListToXMLSerializer<Task>(tasksList, "tasks");
    }
}
