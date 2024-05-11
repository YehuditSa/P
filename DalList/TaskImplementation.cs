namespace Dal;
using DalApi;
using DO;


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
        int id = DataSource.Config.NextTaskId; //Creating Id - a running number
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

        DataSource.Tasks.Add(tempItem);
        return id;
    }

    /// <summary>
    /// Deletes a task by its Id
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int id)
    {
        Task? taskForDelete = Read(id);
        if (taskForDelete is not null)
        {
            DataSource.Tasks.Remove(taskForDelete);
        }
        else  //If this id doesn't exist in the task's list
        {
            throw new DalDoesNotExistException($"Task with ID={id} doesn't exist");
        }

    }

    /// <summary>
    /// Reads a task by its Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>The required task or null - if it doesn't exist</returns>
    public Task? Read(int id)
    {
        return (from task in DataSource.Tasks
                where task.Id == id
                select task).FirstOrDefault();
    }

    /// <summary>
    /// Reads a task by a filter function
    /// </summary>
    /// <param name="filter">The required task or null - if it doesn't exist</param>
    /// <returns>The required task or null - if it doesn't exist</returns>
    public Task? Read(Func<Task?, bool> filter)
    {
        return DataSource.Tasks.Where(filter).FirstOrDefault();
    }

    /// <summary>
    /// Reads all tasks with option of reading by a filter function 
    /// </summary>
    /// <param name="filter">The filter function</param>
    /// <returns>A list of the reqiered tasks or all tasks - if there is no filter function</returns>
    public IEnumerable<Task?> ReadAll(Func<Task?, bool>? filter = null)
    {
        if (filter == null)
            return DataSource.Tasks.Select(item => item).ToList();
        else
            return DataSource.Tasks.Where(filter).ToList();
    }

    /// <summary>
    /// Updates a task in tasks list - by its Id
    /// </summary>
    /// <param name="updateTask">The new Task</param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Update(Task updateTask)
    {
        if (Read(updateTask.Id) is not null)
        {
            Delete(updateTask.Id);
            DataSource.Tasks.Add(updateTask);
        }
        else  //If this id doesn't exist in the task's list
        {
            throw new DalDoesNotExistException($"Task with ID={updateTask.Id} doesn't exist");
        }

    }

    /// <summary>
    /// Resets the list of tasks 
    /// </summary>
    public void ResetAll() { DataSource.Tasks.Clear(); }
}
