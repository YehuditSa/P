using BlApi;
using BO;
using System.Text.RegularExpressions;

namespace BlImplementation;

/// <summary>
/// A class that implements the CRUD methods for Logic Task entity
/// </summary>
internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    /// <summary>
    /// Creates a new Task in DAL
    /// </summary>
    /// <param name="boTask"></param>
    /// <returns>The id of the new Task</returns>
    /// <exception cref="BO.BlAlreadyExistsException"></exception>
    public int Create(BO.Task boTask)
    {
        CheckingValidationOfInput(boTask);
        int idTask = 0;
        try
        {
            idTask = _dal.Task.Create(new DO.Task()
            {
                Id = boTask.Id,
                Description = boTask.Description ?? throw new BO.BlNullPropertyException("Task description can't be null"),
                Alias = boTask.Alias ?? throw new BO.BlNullPropertyException("Task alias can't be null"),
                IsMilestone = false,
                CreatedAtDate = boTask.CreatedAtDate,
                RequiredEffortTime = boTask.RequiredEffortTime,
                StartDate = boTask.StartDate,
                ScheduledDate = boTask.ScheduledDate,
                DeadLineDate = boTask.DeadLineDate,
                CompleteDate = boTask.CompleteDate,
                Deliverables = boTask.Deliverables,
                Remarks = boTask.Remarks,
                EngineerId = boTask.Engineer?.Id,
                ComplexityLevel = (DO.EngineerExperience?)boTask.ComplexityLevel
            });
        }
        catch (Exception ex)
        {
            if (ex is DO.DalAlreadyExistsException)
                throw new BO.BlAlreadyExistsException($"Task with ID={boTask.Id} already exists");
            else
                throw new BO.BlUnknownException($"Unknown Exception in create the new task");
        }
        return idTask;
    }

    /// <summary>
    /// Reads a task by its Id from dal and creates a logical Task entity
    /// </summary>
    /// <param name="id"></param>
    /// <returns>The required logic task</returns>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    /// <exception cref="BO.BlNullException"></exception>
    /// <exception cref="BlNullException"></exception>
    public BO.Task Read(int id)
    {
        DO.Task? doTask = _dal.Task.Read(id);
        if (doTask is null)
            throw new BO.BlDoesNotExistException($"Task with ID={id} does not exist");

        List<DO.Dependency?> allDependency = _dal.Dependency.ReadAll().ToList();
        var dependsOnTasks = (from dep in allDependency
                              where dep.DependentTask == doTask.Id
                              select dep.DependsOnTask).ToList();

        var doMilestone = (from taskId in dependsOnTasks
                           let task = _dal.Task.Read(taskId ?? throw new BO.BlNullException(""))
                           where task.IsMilestone == true
                           select task).FirstOrDefault();

        BO.MilestoneInTask? milestoneInTask = null;

        if (doMilestone is not null)
        {
            milestoneInTask = new MilestoneInTask()
            {
                Id = doMilestone.Id,
                Alias = doMilestone.Alias
            };
        }


        var dependsTasksId = (from dep in allDependency
                              where dep.DependsOnTask == doTask.Id
                              select dep.DependentTask).ToList();

        var dependsTasks = (from taskId in dependsTasksId
                            let task = _dal.Task.Read(taskId ?? throw new BO.BlNullException(""))
                            let taskInList = new BO.TaskInList()
                            {
                                Id = task.Id,
                                Description = task.Description,
                                Alias = task.Alias,
                                Status = calculateTasksStatus(task)
                            }
                            select taskInList).ToList();

        EngineerInTask? engineerInTask = null;

        if (doTask.EngineerId is not null)
        {
            List<DO.Engineer?> engineers = _dal.Engineer.ReadAll().ToList();
            engineerInTask = new()
            {
                Id = doTask.EngineerId ?? throw new BlNullException(""),
                Name = (from eng in engineers
                        where eng.Id == doTask.EngineerId
                        select eng.Name).FirstOrDefault()
            };
        }


        return new BO.Task()
        {
            Id = doTask.Id,
            Description = doTask.Description,
            Alias = doTask.Alias,
            CreatedAtDate = doTask.CreatedAtDate,
            Status = calculateTasksStatus(doTask),
            Dependencies = dependsTasks,
            Milestone = milestoneInTask,
            RequiredEffortTime = doTask.RequiredEffortTime,
            StartDate = doTask.StartDate,
            ScheduledDate = doTask.ScheduledDate,
            DeadLineDate = doTask.DeadLineDate,
            CompleteDate = doTask.CompleteDate,
            Deliverables = doTask.Deliverables,
            Remarks = doTask.Remarks,
            Engineer = engineerInTask,
            ComplexityLevel = (BO.EngineerExperience?)doTask.ComplexityLevel
        };
    }

    /// <summary>
    /// Reads all tasks with option of reading by a filter function
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>A list of the reqiered tasks or all tasks - if there is no filter function</returns>
    public IEnumerable<BO.Task?> ReadAll(Func<BO.Task?, bool>? filter = null)
    {
        List<DO.Task?> doTasks = _dal.Task.ReadAll().ToList();
        var boTasks = from task in doTasks
                      where task.IsMilestone == false
                      select Read(task.Id);
        if (filter is null)
        {
            return boTasks;
        }
        else
        {
            return from task in boTasks
                   where filter(task)
                   select task;
        }
    }


    /// <summary>
    /// Deletes a task from dal by its Id
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="BO.BlDeletionImpossibleException"></exception>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public void Delete(int id)
    {
        List<DO.Dependency?> doDep = _dal.Dependency.ReadAll().ToList();
        BO.Task boTask = Read(id);
        if (boTask.Milestone is not null)
            throw new BO.BlDeletionImpossibleException($"Can't delete task {boTask.Id} because the project has already been scheduled");
        var tasksInDepList = (from dep in doDep
                              where dep.DependentTask == id || dep.DependsOnTask == id
                              select dep).ToList();
        if (tasksInDepList is not null && tasksInDepList.Count != 0)
            throw new BO.BlDeletionImpossibleException($"Can't delete task {boTask.Id} because it is in the dependencies");
        try
        {
            _dal.Task.Delete(id);
        }
        catch (Exception ex)
        {
            if (ex is DO.DalDoesNotExistException)
                throw new BO.BlDoesNotExistException($"Task with ID={id} doesn't exist");
            else
                throw new BO.BlUnknownException($"Unknown Exception in Delete the Task");
        }
      
    }

    /// <summary>
    /// Updates an task detailes in dal
    /// </summary>
    /// <param name="boTask">An entity with updated data</param>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public void Update(BO.Task boTask)
    {
        CheckingValidationOfInput(boTask);
        try
        {
            _dal.Task.Update(new DO.Task()
            {
                Id = boTask.Id,
                Description = boTask.Description ?? throw new BO.BlNullPropertyException("Task description can't be null"),
                Alias = boTask.Alias ?? throw new BO.BlNullPropertyException("Task alias can't be null"),
                IsMilestone = false,
                CreatedAtDate = boTask.CreatedAtDate,
                RequiredEffortTime = boTask.RequiredEffortTime,
                StartDate = boTask.StartDate,
                ScheduledDate = boTask.ScheduledDate,
                DeadLineDate = boTask.DeadLineDate,
                CompleteDate = boTask.CompleteDate,
                Deliverables = boTask.Deliverables,
                Remarks = boTask.Remarks,
                EngineerId = boTask.Engineer?.Id,
                ComplexityLevel = (DO.EngineerExperience?)boTask.ComplexityLevel
            });
        }
        catch (Exception ex)
        {
            if (ex is DO.DalDoesNotExistException)
                throw new BO.BlDoesNotExistException($"Task with ID={boTask.Id} doesn't exist");
            else
                throw new BO.BlUnknownException($"Unknown Exception in Update the Task");
        }
    }

    /// <summary>
    /// An auxiliary method - Calculating the status of a task based on its dates
    /// </summary>
    /// <param name="doTask"></param>
    /// <returns></returns>
    private BO.Status calculateTasksStatus(DO.Task doTask)
    {
        BO.Status status;
        if (doTask.ScheduledDate is null)
            status = BO.Status.Unscheduled;
        else if (doTask.StartDate is null)
            status = BO.Status.Scheduled;
        else if (doTask.CompleteDate is null)
            status = BO.Status.OnTrack;
        else
            status = BO.Status.Completed;
        return status;
    }

    /// <summary>
    /// Auxiliary method - Checks for logical errors in the input
    /// </summary>
    /// <param name="boTask"></param>
    /// <exception cref="BO.BlInvalidInputException"></exception>
    private void CheckingValidationOfInput(BO.Task boTask)
    {
        if (boTask.Id < 0)
            throw new BO.BlInvalidInputException("Task id must be a positive number");

        if (boTask.Alias == "")
            throw new BO.BlInvalidInputException("Task alias can't be empty string");
    }
}
