namespace DalTest;
using DalApi;
using DO;
using System.Security.Cryptography;


internal class Program
{
    //static readonly IDal s_dal = new DalList();
    //static readonly IDal s_dal = new DalXml();
    static readonly IDal s_dal = Factory.Get;


    #region Enums
    private enum methodSelectionOptions { Exit, Add, Show, ShowAll, Update, Delete };
    private enum mainMenuOptions { Exit, Engineer, Task, Dependency };
    #endregion
    private static void Main(string[] args)
    {
        try
        {
            Console.Write("Would you like to create Initial data? (Y/N) ");
            string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
            if (ans == "Y")
                Initialization.Do();

            mainMenu();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message + "\n" + e.StackTrace + "\n");
            mainMenu();

        }
    }

    private static void mainMenu()
    {
        Console.WriteLine("\nEnter an entity you want to check: \n 0- Exit from main menu \n 1- Engineer \n 2- Task \n 3- Dependency");

        mainMenuOptions choose = (mainMenuOptions)Convert.ToInt32(Console.ReadLine());


        switch (choose)
        {
            case mainMenuOptions.Exit:
                return;
            case mainMenuOptions.Engineer:
                methodSelectionMenu("Engineer");
                break;
            case mainMenuOptions.Task:
                methodSelectionMenu("Task");
                break;
            case mainMenuOptions.Dependency:
                methodSelectionMenu("dependency");
                break;
            default:
                return;
        }

    }

    /// <summary>
    ///A menu for selecting the desired method
    /// </summary>
    /// <param name="entity">The required entity</param>
    private static void methodSelectionMenu(string entity)
    {
        string prefix = ("AEIOU".Contains(entity[0])) ? "an" : "a";
        try
        {
            while (true)
            {
                Console.WriteLine($"\nEnter a method you want to execute: \n 0- Exit to main menu \n 1- Add a new {entity} \n 2- Show {prefix} {entity} \n 3- Show a list of all {entity}s \n 4- Update {prefix} {entity} data \n 5- Delete {prefix} {entity}");
                methodSelectionOptions choose = (methodSelectionOptions)Convert.ToInt32(Console.ReadLine());

                switch (choose)
                {
                    case methodSelectionOptions.Exit:
                        mainMenu();
                        return;

                    case methodSelectionOptions.Add:
                        if (entity == "Engineer")
                            addEngineer();
                        else if (entity == "Task")
                            addTask();
                        else
                            addDependency();
                        break;

                    case methodSelectionOptions.Show:
                        if (entity == "Engineer")
                            showEngineer();
                        else if (entity == "Task")
                            showTask();
                        else
                            showDependency();
                        break;

                    case methodSelectionOptions.ShowAll:
                        if (entity == "Engineer")
                            showAllEngineers();
                        else if (entity == "Task")
                            showAllTasks();
                        else
                            showAllDependencies();
                        break;

                    case methodSelectionOptions.Update:
                        if (entity == "Engineer")
                            updateEngineer();
                        else if (entity == "Task")
                            updateTask();
                        else
                            updateDependency();
                        break;

                    case methodSelectionOptions.Delete:
                        if (entity == "Engineer")
                            deleteEngineer();
                        else if (entity == "Task")
                            deleteTask();
                        else
                            deleteDependency();
                        break;

                    default:
                        return;
                }
            }
        }
        catch (Exception msg)
        {
            Console.WriteLine(msg);

        }

    }

    #region Add Methods
    private static void addEngineer()
    {
        Console.WriteLine("Enter the Engineer's ID: ");
        int _id = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Enter the Engineer's name: ");
        string _name = Console.ReadLine()!;

        Console.WriteLine("Enter the Engineer's level:\n  0- Novice, 1- AdvancedBeginner, 2- Competent, 3- Proficient, 4- Expert");
        EngineerExperience _level = (EngineerExperience)Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Enter the Engineer's email: ");
        string _email = Console.ReadLine()!;

        Console.WriteLine("Enter the Engineer's cost: ");
        double _cost = Convert.ToDouble(Console.ReadLine());


        Engineer newEng = new(_id, _name, _level, _email, _cost);
        s_dal!.Engineer.Create(newEng);

    }
    private static void addTask()
    {
        string? input;

        Console.WriteLine("Enter the Task's description: ");
        string _description = Console.ReadLine()!;

        Console.WriteLine("Enter the Task's alias: ");
        string _alias = Console.ReadLine()!;


        Console.WriteLine("If the task is Milestone - enter 'true' \n Else - enter 'false' ");
        bool _isMilestone = Convert.ToBoolean(Console.ReadLine()!);

        Console.WriteLine("Enter the Task's Creation Date: ");
        DateTime _createdAtDate = Convert.ToDateTime(Console.ReadLine()!);

        Console.WriteLine("Enter how many days needed to the task: ");
        TimeSpan? _requiredEffortTime = null;
        input = Console.ReadLine();
        if (!String.IsNullOrEmpty(input))
            _requiredEffortTime = new TimeSpan(Convert.ToInt32(input), 0, 0, 0);


        Console.WriteLine("Enter the Task's Start Date: ");
        DateTime? _startDate;
        input = Console.ReadLine();
        if (String.IsNullOrEmpty(input))
            _startDate = null;
        else
            _startDate = Convert.ToDateTime(input);

        Console.WriteLine("Enter the Task's Scheduled Date: ");
        DateTime? _scheduledDate;
        input = Console.ReadLine();
        if (String.IsNullOrEmpty(input))
            _scheduledDate = null;
        else
            _scheduledDate = Convert.ToDateTime(input);

        Console.WriteLine("Enter the Task's Forecast Date: ");
        DateTime? _forecastDate;
        input = Console.ReadLine();
        if (String.IsNullOrEmpty(input))
            _forecastDate = null;
        else
            _forecastDate = Convert.ToDateTime(input);


        Console.WriteLine("Enter the Task's Dead Line Date: ");
        DateTime? _deadLineDate;
        input = Console.ReadLine();
        if (String.IsNullOrEmpty(input))
            _deadLineDate = null;
        else
            _deadLineDate = Convert.ToDateTime(input);

        Console.WriteLine("Enter the Task's Complete Date: ");
        DateTime? _completeDate;
        input = Console.ReadLine();
        if (String.IsNullOrEmpty(input))
            _completeDate = null;
        else
            _completeDate = Convert.ToDateTime(input);

        Console.WriteLine("Enter the Task's Deliverables: ");
        string? _deliverables = Console.ReadLine();
        if (String.IsNullOrEmpty(_deliverables))
            _deliverables = null;

        Console.WriteLine("Enter the Task's Remarks: ");
        string? _remarks = Console.ReadLine();
        if (String.IsNullOrEmpty(_remarks))
            _remarks = null;

        Console.WriteLine("Enter the engineer's ID  ");
        int? _engineerId;
        input = Console.ReadLine();
        if (String.IsNullOrEmpty(input))
            _engineerId = null;
        else
            _engineerId = Convert.ToInt32(input);

        Console.WriteLine("Enter the Complexity's level: ");
        int? _complexityLevel;
        input = Console.ReadLine();
        if (String.IsNullOrEmpty(input))
            _complexityLevel = null;
        else
            _complexityLevel = Convert.ToInt32(input);

        Task newTask = new Task()
        {
            Id = 0,
            Description = _description,
            Alias = _alias,
            IsMilestone = _isMilestone,
            CreatedAtDate = _createdAtDate,
            RequiredEffortTime = _requiredEffortTime,
            StartDate = _startDate,
            ScheduledDate = _scheduledDate,
            ForecastDate = _forecastDate,
            DeadLineDate = _deadLineDate,
            CompleteDate = _completeDate,
            Deliverables = _deliverables,
            Remarks = _remarks,
            EngineerId = _engineerId,
            ComplexityLevel = (EngineerExperience?)_complexityLevel
        };
        s_dal!.Task.Create(newTask);
    }
    private static void addDependency()
    {
        string? input;

        Console.WriteLine("Enter the ID of the dependent task: ");
        int? _dependentTask;
        input = Console.ReadLine();
        if (String.IsNullOrEmpty(input))
            _dependentTask = null;
        else
            _dependentTask = Convert.ToInt32(input);


        Console.WriteLine("Enter the ID of the dependence on task: ");
        int? _dependsOnTask;
        input = Console.ReadLine();
        if (String.IsNullOrEmpty(input))
            _dependsOnTask = null;
        else
            _dependsOnTask = Convert.ToInt32(input);

        Dependency newDep = new(0, _dependentTask, _dependsOnTask);
        s_dal!.Dependency.Create(newDep);
    }

    #endregion

    #region Show Methods
    private static void showEngineer()
    {
        Console.WriteLine("Enter the Engineer's ID: ");
        int _id = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine(s_dal!.Engineer.Read(_id));
    }
    private static void showTask()
    {
        Console.WriteLine("Enter the Task's ID: ");
        int _id = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine(s_dal!.Task.Read(_id));
    }
    private static void showDependency()
    {
        Console.WriteLine("Enter the Dependency's ID: ");
        int _id = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine(s_dal!.Dependency.Read(_id));
    }

    #endregion

    #region ShowAll Methods
    private static void showAllEngineers()
    {
        List<Engineer> engineers = (List<Engineer>)s_dal!.Engineer.ReadAll();
        foreach (Engineer eng in engineers)
        {
            Console.WriteLine(eng);
        }
    }
    private static void showAllTasks()
    {
        List<Task> tasks = (List<Task>)s_dal!.Task.ReadAll();
        foreach (Task task in tasks)
        {
            Console.WriteLine(task);
        }
    }
    private static void showAllDependencies()
    {
        List<Dependency> dependencies = (List<Dependency>)s_dal!.Dependency.ReadAll();
        foreach (Dependency dep in dependencies)
        {
            Console.WriteLine(dep);
        }
    }

    #endregion

    #region Delete
    private static void deleteEngineer()
    {
        Console.WriteLine("Enter the Engineer's ID: ");
        int _id = Convert.ToInt32(Console.ReadLine());
        s_dal!.Engineer.Delete(_id);
    }
    private static void deleteTask()
    {
        Console.WriteLine("Enter the Task's ID: ");
        int _id = Convert.ToInt32(Console.ReadLine());
        s_dal!.Task.Delete(_id);
    }
    private static void deleteDependency()
    {
        Console.WriteLine("Enter the Dependency's ID: ");
        int _id = Convert.ToInt32(Console.ReadLine());
        s_dal!.Dependency.Delete(_id);
    }

    #endregion

    #region Update
    /*In the update Methods - if the user entered empty input:
        if the field is not nullable, we used tryParse
        but else, we used a temp string variable ('input') for recieved data from user */

    private static void updateEngineer()
    {

        Console.WriteLine("Enter the Engineer's ID: ");
        int _id;
        int.TryParse(Console.ReadLine(), out _id);

        Engineer oldEng = s_dal!.Engineer.Read(_id) ?? throw new NullReferenceException();
        Console.WriteLine(oldEng);

        Console.WriteLine("Enter the Engineer's name: ");
        string? _name = Console.ReadLine();
        if (String.IsNullOrEmpty(_name))
            _name = oldEng.Name;

        Console.WriteLine("Enter the Engineer's level:\n  0- Novice, 1- AdvancedBeginner, 2- Competent, 3- Proficient, 4- Expert");
        int _level;
        if (!int.TryParse(Console.ReadLine(), out _level))
            _level = (int)oldEng.Level;


        Console.WriteLine("Enter the Engineer's email: ");
        string? _email = Console.ReadLine();
        if (String.IsNullOrEmpty(_email))
            _email = oldEng.Email;

        Console.WriteLine("Enter the Engineer's cost: ");
        double _cost;
        if (!double.TryParse(Console.ReadLine(), out _cost))
            _cost = oldEng.Cost;


        Engineer newEng = new(_id, _name, (EngineerExperience)_level, _email, _cost);
        s_dal!.Engineer.Update(newEng);
    }
    private static void updateTask()
    {
        string? input;

        Console.WriteLine("Enter the Task's ID: ");
        int _id;
        int.TryParse(Console.ReadLine(), out _id);

        Task oldTask = s_dal!.Task.Read(_id) ?? throw new NullReferenceException();
        Console.WriteLine(oldTask);

        Console.WriteLine("Enter the Task's description: ");
        string? _description = Console.ReadLine()!;
        if (String.IsNullOrEmpty(_description))
            _description = oldTask.Description;

        Console.WriteLine("Enter the Task's alias: ");
        string? _alias = Console.ReadLine()!;
        if (String.IsNullOrEmpty(_alias))
            _alias = oldTask.Alias;

        Console.WriteLine("If the task is Milestone - enter 'true' \n Else - enter 'false' ");
        bool _isMilestone;
        if (bool.TryParse(Console.ReadLine(), out _isMilestone))
            _isMilestone = oldTask.IsMilestone;

        Console.WriteLine("Enter the Task's Creation Date: ");
        DateTime _createdAtDate;
        if (DateTime.TryParse(Console.ReadLine(), out _createdAtDate))
            _createdAtDate = oldTask.CreatedAtDate;

        Console.WriteLine("Enter how many days needed to the task: ");
        TimeSpan? _requiredEffortTime = null;
        input = Console.ReadLine();
        if (!String.IsNullOrEmpty(input))
            _requiredEffortTime = new TimeSpan(Convert.ToInt32(input), 0, 0, 0);

        Console.WriteLine("Enter the Task's Start Date: ");
        DateTime? _startDate;
        input = Console.ReadLine();
        if (String.IsNullOrEmpty(input))
            _startDate = oldTask.StartDate;
        else
            _startDate = Convert.ToDateTime(input);

        Console.WriteLine("Enter the Task's Scheduled Date: ");
        DateTime? _scheduledDate;
        input = Console.ReadLine();
        if (String.IsNullOrEmpty(input))
            _scheduledDate = oldTask.ScheduledDate;
        else
            _scheduledDate = Convert.ToDateTime(input);

        Console.WriteLine("Enter the Task's Forecast Date: ");
        DateTime? _forecastDate;
        input = Console.ReadLine();
        if (String.IsNullOrEmpty(input))
            _forecastDate = oldTask.ForecastDate;
        else
            _forecastDate = Convert.ToDateTime(input);


        Console.WriteLine("Enter the Task's Dead Line Date: ");
        DateTime? _deadLineDate;
        input = Console.ReadLine();
        if (String.IsNullOrEmpty(input))
            _deadLineDate = oldTask.DeadLineDate;
        else
            _deadLineDate = Convert.ToDateTime(input);

        Console.WriteLine("Enter the Task's Complete Date: ");
        DateTime? _completeDate;
        input = Console.ReadLine();
        if (String.IsNullOrEmpty(input))
            _completeDate = oldTask.CompleteDate;
        else
            _completeDate = Convert.ToDateTime(input);

        Console.WriteLine("Enter the Task's Deliverables: ");
        string? _deliverables = Console.ReadLine();
        if (String.IsNullOrEmpty(_deliverables))
            _deliverables = oldTask.Deliverables;

        Console.WriteLine("Enter the Task's Remarks: ");
        string? _remarks = Console.ReadLine();
        if (String.IsNullOrEmpty(_remarks))
            _remarks = oldTask.Remarks;

        Console.WriteLine("Enter the engineer's ID  ");
        int? _engineerId;
        input = Console.ReadLine();
        if (String.IsNullOrEmpty(input))
            _engineerId = oldTask.EngineerId;
        else
            _engineerId = Convert.ToInt32(input);

        Console.WriteLine("Enter the Complexity's level: ");
        int? _complexityLevel;
        input = Console.ReadLine();
        if (String.IsNullOrEmpty(input))
            _complexityLevel = (int?)oldTask.ComplexityLevel;
        else
            _complexityLevel = Convert.ToInt32(input);

        Task newTask = new Task()
        {
            Id = _id,
            Description = _description,
            Alias = _alias,
            IsMilestone = _isMilestone,
            CreatedAtDate = _createdAtDate,
            RequiredEffortTime = _requiredEffortTime,
            StartDate = _startDate,
            ScheduledDate = _scheduledDate,
            ForecastDate = _forecastDate,
            DeadLineDate = _deadLineDate,
            CompleteDate = _completeDate,
            Deliverables = _deliverables,
            Remarks = _remarks,
            EngineerId = _engineerId,
            ComplexityLevel = (EngineerExperience?)_complexityLevel
        };
        s_dal!.Task.Update(newTask);
    }
    private static void updateDependency()
    {
        string? input;

        Console.WriteLine("Enter the Dependency's ID: ");
        int _id;
        int.TryParse(Console.ReadLine(), out _id);

        Dependency oldDep = s_dal!.Dependency.Read(_id) ?? throw new NullReferenceException();
        Console.WriteLine(oldDep);

        Console.WriteLine("Enter the ID of the dependent task: ");
        int? _dependentTask;
        input = Console.ReadLine();
        if (String.IsNullOrEmpty(input))
            _dependentTask = oldDep.DependentTask;
        else
            _dependentTask = Convert.ToInt32(input);


        Console.WriteLine("Enter the ID of the dependence on task: ");
        int? _dependsOnTask;
        input = Console.ReadLine();
        if (String.IsNullOrEmpty(input))
            _dependsOnTask = oldDep.DependsOnTask;
        else
            _dependsOnTask = Convert.ToInt32(input);

        Dependency newDep = new(_id, _dependentTask, _dependsOnTask);
        s_dal!.Dependency.Update(newDep);
    }

    #endregion
}