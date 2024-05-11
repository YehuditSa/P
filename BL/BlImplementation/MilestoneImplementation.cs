using BlApi;
using BO;


namespace BlImplementation;

internal class MilestoneImplementation : IMilestone
{
    private DalApi.IDal _dal = DalApi.Factory.Get;


    /// <summary>
    /// Creating milestones depending on the dependencies and creating new dependencies list depending on the new milestones
    /// </summary>
    /// <param name="recievedDeps">List of the previous Dependencies</param>
    /// <returns>List of the new dependencies</returns>
    /// <exception cref="BO.BlNullException"></exception>
    private List<DO.Dependency> createMilestones(List<DO.Dependency?> recievedDeps)
    {
        List<DO.Dependency> newDepsList = new List<DO.Dependency>();


        //Creating a group list: the key is the dependent task, and the value is the list of dependens on task
        var ListOfTaskAndItsDependetTaskList = (
            from dep in recievedDeps
            where dep.DependentTask is not null && dep.DependsOnTask is not null
            group dep by dep.DependentTask into depListAfterGrouping
            let depList = (from dep in depListAfterGrouping
                           select dep.DependsOnTask)
            select new { _Key = depListAfterGrouping.Key, Value = depList.Order() }).ToList();

        //Creating a list of lists of tasks
        var listAfterDistinct = (from elem in ListOfTaskAndItsDependetTaskList
                                 select elem.Value.ToList()).Distinct(new BO.Tools.NullableListIntComparer()).ToList();


        int runningNumberForAlias = 1;

        foreach (List<int?> tasks in listAfterDistinct)
        {
            int idOfMilestone = _dal.Task.Create(new DO.Task("I am a Milestone:)", $"M{runningNumberForAlias++}", true, DateTime.Now) { RequiredEffortTime = TimeSpan.Zero });

            foreach (var task in tasks)
            {
                newDepsList.Add(new DO.Dependency(0, idOfMilestone, task));
            }

            foreach (var taskAndDependetTaskList in ListOfTaskAndItsDependetTaskList)
            {
                if (taskAndDependetTaskList.Value.ToList().SequenceEqual(tasks))
                    newDepsList.Add(new DO.Dependency(0, taskAndDependetTaskList._Key!.Value, idOfMilestone));
            }

        }

        int idOfStartMilestone = _dal.Task.Create(new DO.Task("description", "start", true, DateTime.Now) { RequiredEffortTime = TimeSpan.Zero });
        int idOfEndMilestone = _dal.Task.Create(new DO.Task("description", "end", true, DateTime.Now) { RequiredEffortTime = TimeSpan.Zero });

        List<DO.Task?> allTasks = _dal.Task.ReadAll().ToList();

        var allTasksId = (from task in allTasks
                          where task.IsMilestone == false
                          select task.Id).ToList();

        var tasksThatDepentensOnSomething = from taskAndDependetTaskList in ListOfTaskAndItsDependetTaskList
                                            where taskAndDependetTaskList.Value is not null
                                            select taskAndDependetTaskList._Key!.Value;

        var tasksThatDependensOnStart = allTasksId.Except(tasksThatDepentensOnSomething);

        foreach (var task in tasksThatDependensOnStart)
        {
            newDepsList.Add(new DO.Dependency(task, idOfStartMilestone));
        }

        List<int> tasksThatSomethingDepentensOnthem = new();
        foreach (var taskAndDependetTaskList in ListOfTaskAndItsDependetTaskList)
        {
            foreach (int? task in taskAndDependetTaskList.Value)
                tasksThatSomethingDepentensOnthem.Add(task ?? throw new BO.BlNullException("task id can't be null"));
        }

        var tasksThatEndDependensOnthem = allTasksId.Except(tasksThatSomethingDepentensOnthem).ToList();

        foreach (var task in tasksThatEndDependensOnthem)
        {
            newDepsList.Add(new DO.Dependency(idOfEndMilestone, task));
        }

        return newDepsList;
    }

    /// <summary>
    /// Recursively going (from end to start) over the tasks and updating the deadline of each task
    /// </summary>
    /// <param name="idOfTask">The current task id</param>
    /// <param name="idOfStartMilestone"></param>
    /// <param name="dependenciesList"></param>
    /// <exception cref="BO.BlNullException"></exception>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    private void updateDeadLineDate(int? idOfTask, int idOfStartMilestone, List<DO.Dependency?> dependenciesList)
    {
        //Stop condition
        if (idOfTask == idOfStartMilestone)
            return;

        //The data of current checked task
        DO.Task? dependentTask = _dal.Task.Read(idOfTask ?? throw new BO.BlNullException("id Of Task can't be null"));

        var DependsOnTaskList = dependenciesList.Where(dep => dep?.DependentTask == idOfTask)
                                                .Select(dep => dep?.DependsOnTask).ToList();

        if (DependsOnTaskList is not null)
        {
            foreach (int? taskId in DependsOnTaskList)
            {
                DO.Task? currentTask = _dal.Task.Read(taskId ?? throw new BO.BlNullException("id Of Task can't be null"));

                if (currentTask is null)
                    throw new BO.BlDoesNotExistException($"task with id {taskId} is not exist");

                DateTime? deadLineDate = dependentTask?.DeadLineDate - dependentTask?.RequiredEffortTime;


                if (currentTask.IsMilestone is false || (currentTask.IsMilestone is true && (currentTask.DeadLineDate is null || deadLineDate < currentTask.DeadLineDate)))
                    currentTask.DeadLineDate = deadLineDate;

                _dal.Task.Update(currentTask);

                updateDeadLineDate(taskId, idOfStartMilestone, dependenciesList);
            }
        }

    }

    /// <summary>
    /// Recursively going (from start to end) over the tasks and updating the Scheduled Date of each task
    /// </summary>
    /// <param name="idOfTask">The current task id</param>
    /// <param name="idOfEndMilestone"></param>
    /// <param name="dependenciesList"></param>
    /// <exception cref="BO.BlNullException"></exception>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    /// <exception cref="BlPlanningOfProjectTimesException"></exception>
    private void updateScheduledDate(int? idOfTask, int idOfEndMilestone, List<DO.Dependency?> dependenciesList)
    {
        //Stop condition
        if (idOfTask == idOfEndMilestone)
            return;

        //The data of current checked task
        DO.Task? dependentTask = _dal.Task.Read(idOfTask ?? throw new BO.BlNullException("id Of Task can't be null"));

        var DependentTaskList = dependenciesList.Where(dep => dep?.DependsOnTask == idOfTask)
            .Select(dep => dep?.DependentTask).ToList();

        foreach (int? taskId in DependentTaskList)
        {
            DO.Task? currentTask = _dal.Task.Read(taskId ?? throw new BO.BlNullException("id Of Task can't be null"));

            if (currentTask is null)
                throw new BO.BlDoesNotExistException($"task with id {taskId} is not exist");
            if (dependentTask?.DeadLineDate + currentTask.RequiredEffortTime > currentTask.DeadLineDate)
                throw new BlPlanningOfProjectTimesException($"According to the date restrictions, the task {taskId} does not have time to be completed in its entirety");

            DateTime? scheduledDate = dependentTask?.ScheduledDate + dependentTask?.RequiredEffortTime;

            if (currentTask.IsMilestone is false || (currentTask.IsMilestone is true && (currentTask.ScheduledDate is null || scheduledDate > currentTask.ScheduledDate)))
                currentTask.ScheduledDate = scheduledDate;

            _dal.Task.Update(currentTask);

            updateScheduledDate(taskId, idOfEndMilestone, dependenciesList);
        }
    }

    /// <summary>
    /// Rename the milestones alias to be the ids of the dependent on tasks
    /// </summary>
    /// <param name="dependenciesList"></param>
    private void renameMilestonesAlias(List<DO.Dependency?> dependenciesList)
    {
        List<DO.Task?> allTasks = _dal.Task.ReadAll().ToList();
        foreach (var task in allTasks)
        {
            if (task is not null && task.IsMilestone is true && task.Alias != "start" && task.Alias != "end")
            {
                string alias = "M";
                var dependsOnTaskList = dependenciesList.Where(dep => dep?.DependentTask == task.Id)
                .Select(dep => dep?.DependsOnTask).ToList();
                foreach (var dependsOnTask in dependsOnTaskList)
                {
                    alias += $"{dependsOnTask}";
                }
                task.Alias = alias;
            }

        }

    }

    /// <summary>
    /// Creating Milestones and Project Time Line
    /// </summary>
    public void CreateProjectSchedule(DateTime startProjectDate, DateTime endProjectDate)
    {
        try
        {
            #region Changing all the dependencies in dal  

            List<DO.Dependency?> dependenciesList = _dal.Dependency.ReadAll().ToList();
            List<DO.Dependency> newDepList = createMilestones(dependenciesList);
            _dal.Dependency.ResetAll();
            foreach (var dependency in newDepList)
            {
                _dal.Dependency.Create(dependency);

            }
            #endregion

            #region Calculation of times to complete the tasks and milestones

            _dal.StartProjectDate = startProjectDate;
            _dal.EndProjectDate = endProjectDate;


            List<DO.Task?> allTasks = _dal.Task.ReadAll().ToList();
            int idOfStartMilestone = allTasks.Where(task => task!.Alias == "start").Select(task => task!.Id).First();
            int idOfEndMilestone = allTasks.Where(task => task!.Alias == "end").Select(task => task!.Id).First();

            DO.Task? endMilestoneData = _dal.Task.Read(idOfEndMilestone);
            if (endMilestoneData is not null)
            {
                endMilestoneData.DeadLineDate = _dal.EndProjectDate;
                endMilestoneData.ScheduledDate = _dal.EndProjectDate;
                _dal.Task.Update(endMilestoneData);
            }
            DO.Task? startMilestoneData = _dal.Task.Read(idOfStartMilestone);
            if (startMilestoneData is not null)
            {
                startMilestoneData.ScheduledDate = _dal.StartProjectDate;
                startMilestoneData.ScheduledDate = _dal.StartProjectDate;
                _dal.Task.Update(startMilestoneData);
            }


            updateDeadLineDate(idOfEndMilestone, idOfStartMilestone, _dal.Dependency.ReadAll().ToList());
            updateScheduledDate(idOfStartMilestone, idOfEndMilestone, _dal.Dependency.ReadAll().ToList());

            #endregion

            renameMilestonesAlias(_dal.Dependency.ReadAll().ToList());
        }
        catch(Exception ex)
        {
            if(ex is DO.DalDoesNotExistException)
            {
                throw new BlDoesNotExistException($"Something was wrong in createMilestons code");
            }
            else if(ex is DO.DalAlreadyExistsException) 
            {
                throw new BlAlreadyExistsException($"Something was wrong in createMilestons code");
            }
        }

        

    }

    /// <summary>
    /// Calculates the status of task by its dates
    /// </summary>
    /// <param name="doTask">The requiered task in DO entity</param>
    /// <returns>The requiered task in BO entity</returns>
    private BO.Status calculateTaskStatus(DO.Task doTask)
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
    /// Reads a milestone by its ID
    /// </summary>
    /// <param name="id">The requiered milestone id</param>
    /// <returns>The requiered milestone</returns>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    /// <exception cref="BO.BlTaskIsNotMilestoneException"></exception>
    /// <exception cref="BO.BlNullException"></exception>
    public BO.Milestone Read(int id)
    {
        DO.Task? doMilestone = _dal.Task.Read(id);
        if (doMilestone is null)
            throw new BO.BlDoesNotExistException($"Milestone with ID={id} does not exist");
        if (doMilestone.IsMilestone == false)
            throw new BO.BlTaskIsNotMilestoneException($"Task with ID={id} isn't Milestone");

        List<DO.Dependency?> allDependenciesList = _dal.Dependency.ReadAll().ToList();
        var dependenciesId = (from dep in allDependenciesList
                              where dep.DependentTask == doMilestone.Id
                              select dep.DependsOnTask).ToList();

        //Creating a list for the dependencies property of milestone
        List<BO.TaskInList> dependencies = (from task in dependenciesId
                                            let taskData = _dal.Task.Read(task ?? throw new BO.BlNullException("")) ?? throw new BO.BlDoesNotExistException($"Task with ID={task} does not exist")
                                            let taskInList = new BO.TaskInList()
                                            {
                                                Id = taskData.Id,
                                                Description = taskData.Description,
                                                Alias = taskData.Alias,
                                                Status = calculateTaskStatus(taskData)
                                            }
                                            select taskInList).ToList();

        BO.Milestone boMilestone = new BO.Milestone()
        {
            Id = doMilestone.Id,
            Description = doMilestone.Description,
            Alias = doMilestone.Alias,
            CreatedAtDate = doMilestone.CreatedAtDate,
            Status = calculateTaskStatus(doMilestone),
            StartDate = doMilestone.StartDate,
            ForecastDate = doMilestone.ForecastDate,
            DeadlineDate = doMilestone.DeadLineDate,
            CompleteDate = doMilestone.CompleteDate,
            //CompletionPercentage
            Remarks = doMilestone.Remarks,
            Dependencies = dependencies
        };

        return boMilestone;
    }

    /// <summary>
    /// Reads all tasks with option of reading by a filter function
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>A list of the reqiered tasks or all tasks - if there is no filter function</returns>
    public IEnumerable<BO.Milestone?> ReadAll(Func<BO.Milestone?, bool>? filter = null)
    {
        List<DO.Task?> doTasks = _dal.Task.ReadAll().ToList();
        var boMiletones = from task in doTasks
                      where task.IsMilestone == true
                      select Read(task.Id);
        if (filter is null)
        {
            return boMiletones;
        }
        else
        {
            return from milestone in boMiletones
                   where filter(milestone)
                   select milestone;
        }
    }

    /// <summary>
    /// Updates certain values in a milestone 
    /// </summary>
    /// <param name="id"> The milestone id</param>
    /// <param name="description"></param>
    /// <param name="alias"></param>
    /// <param name="remarks"></param>
    /// <returns>An updated Milestone object of a logical entity</returns>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    /// <exception cref="BO.BlTaskIsNotMilestoneException"></exception>
    public BO.Milestone Update(int id, string? description = null, string? alias = null, string? remarks = null)
    {
        DO.Task doMilestone = _dal.Task.Read(id) ?? throw new BO.BlDoesNotExistException($"Milestone with ID={id} does not exist");
        if (doMilestone.IsMilestone == false)
            throw new BO.BlTaskIsNotMilestoneException($"Task with ID={id} isn't Milestone");
        if (description is not null)
            doMilestone.Description = description;
        if (alias is not null)
            doMilestone.Alias = alias;
        if (remarks is not null)
            doMilestone.Remarks = remarks;

        try
        {
             _dal.Task.Update(doMilestone);
        }
        catch (Exception ex)
        {
            if (ex is DO.DalDoesNotExistException)
                throw new BO.BlDoesNotExistException($"Milestone with ID={doMilestone.Id} doesn't exist");
            else
                throw new BO.BlUnknownException($"Unknown Exception in Update the Milestone details");
        }

        BO.Milestone boMilestone = Read(id);
        return boMilestone;
    }
}