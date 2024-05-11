using DalApi;
using DalTest;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Xml.Linq;

internal class Program
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    private enum MainMenuOptions { Exit, Engineer, Task, Milestone };
    private enum EngineerMenuOptions { Exit, Create, Read, ReadAll, Update, Delete };
    private enum TaskMenuOptions { Exit, Create, Read, ReadAll, Update, Delete };

    private enum MilestoneMenuOptions { Exit, CreateSchedule, Read, Update }
    private static void Main(string[] args)
    {
        try
        {
            Console.Write("Would you like to create Initial data? (Y/N) ");
            string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
            if (ans == "Y")
                DalTest.Initialization.Do();

            mainMenu();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    private static void mainMenu()
    {

        Console.WriteLine("\nEnter an entity you want to check: \n 0- Exit from main menu \n 1- Engineer \n 2- Task \n 3- Milestone");

        MainMenuOptions choose = (MainMenuOptions)Convert.ToInt32(Console.ReadLine());

        switch (choose)
        {
            case MainMenuOptions.Exit:
                return;
            case MainMenuOptions.Engineer:
                engineerMenu();
                break;
            case MainMenuOptions.Task:
                taskMenu();
                break;
            case MainMenuOptions.Milestone:
                milestoneMenu();
                break;
            default:
                return;
        }

    }

    /// <summary>
    /// A menu for selecting the desired method
    /// </summary>
    private static void engineerMenu()
    {
        try
        {
            while (true)
            {
                Console.WriteLine($"\nEnter a method you want to execute: \n 0- Exit to main menu \n 1- Create \n 2- Read \n 3- Read All \n 4- Update \n 5- Delete");
                EngineerMenuOptions choose = (EngineerMenuOptions)Convert.ToInt32(Console.ReadLine());

                switch (choose)
                {
                    case EngineerMenuOptions.Exit:
                        mainMenu();
                        return;

                    case EngineerMenuOptions.Create:
                        createEngineer();
                        break;

                    case EngineerMenuOptions.Read:
                        readEngineer();
                        break;

                    case EngineerMenuOptions.ReadAll:
                        readAllEngineers();
                        break;

                    case EngineerMenuOptions.Update:
                        updateEngineer();
                        break;

                    case EngineerMenuOptions.Delete:
                        deleteEngineer();
                        break;

                    default:
                        return;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message + "\n" + e.StackTrace + "\n");
            engineerMenu();

        }

    }

    /// <summary>
    /// A menu for selecting the desired method
    /// </summary>
    private static void taskMenu()
    {
        try
        {
            while (true)
            {
                Console.WriteLine($"\nEnter a method you want to execute: \n 0- Exit to main menu \n 1- Create \n 2- Read \n 3- Read All \n 4- Update \n 5- Delete");
                TaskMenuOptions choose = (TaskMenuOptions)Convert.ToInt32(Console.ReadLine());

                switch (choose)
                {
                    case TaskMenuOptions.Exit:
                        mainMenu();
                        return;

                    case TaskMenuOptions.Create:
                        createTask();
                        break;

                    case TaskMenuOptions.Read:
                        readTask();
                        break;

                    case TaskMenuOptions.ReadAll:
                        readAllTasks();
                        break;

                    case TaskMenuOptions.Update:
                        updateTask();
                        break;

                    case TaskMenuOptions.Delete:
                        deleteTask();
                        break;

                    default:
                        return;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message + "\n" + e.StackTrace + "\n");
            taskMenu();

        }

    }

    /// <summary>
    /// A menu for selecting the desired method
    /// </summary>
    private static void milestoneMenu()
    {
        try
        {
            while (true)
            {
                Console.WriteLine($"\nEnter a method you want to execute: \n 0- Exit to main menu \n 1- Create Project Schedule \n 2- Read \n 3- Update \n");
                MilestoneMenuOptions choose = (MilestoneMenuOptions)Convert.ToInt32(Console.ReadLine());

                switch (choose)
                {
                    case MilestoneMenuOptions.Exit:
                        mainMenu();
                        return;

                    case MilestoneMenuOptions.CreateSchedule:
                        CreateProjectSchedule();
                        break;

                    case MilestoneMenuOptions.Read:
                        readMilestone();
                        break;

                    case MilestoneMenuOptions.Update:
                        updateMilestone();
                        break;

                    default:
                        return;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message + "\n" + e.StackTrace + "\n");
            milestoneMenu();

        }

    }

    #region Create Methods
    private static void createEngineer()
    {

        Console.WriteLine("Enter the Engineer's ID: ");
        int id;
        if (!int.TryParse(Console.ReadLine(), out id))
            throw new BO.BlInvalidInputException($"Engineer id {id} is invalid");

        Console.WriteLine("Enter the Engineer's name: ");
        string? name = Console.ReadLine();
        if (String.IsNullOrEmpty(name))
            throw new BO.BlInvalidInputException($"Engineer must has a name");

        Console.WriteLine("Enter the Engineer's level:\n  0- Novice, 1- AdvancedBeginner, 2- Competent, 3- Proficient, 4- Expert");
        int level;
        if (!int.TryParse(Console.ReadLine(), out level))
            throw new BO.BlInvalidInputException($"Engineer level {level} is invalid");

        Console.WriteLine("Enter the Engineer's email: ");
        string? email = Console.ReadLine();
        if (String.IsNullOrEmpty(email))
            throw new BO.BlInvalidInputException($"Engineer must has an email");

        Console.WriteLine("Enter the Engineer's cost: ");
        double cost;
        if (!double.TryParse(Console.ReadLine(), out cost))
            throw new BO.BlInvalidInputException($"Engineer level {cost} is invalid");

        BO.Engineer newBoEng = new()
        {
            Id = id,
            Name = name,
            Email = email,
            Level = (BO.EngineerExperience)level,
            Cost = cost,
            Task = null
        };

        s_bl.Engineer.Create(newBoEng);

    }
    private static void createTask()
    {
        string? input;

        Console.WriteLine("Enter the Task's description: ");
        string? description = Console.ReadLine();
        if (String.IsNullOrEmpty(description))
            throw new BO.BlInvalidInputException($"Task must has description");

        Console.WriteLine("Enter the Task's alias: ");
        string? alias = Console.ReadLine();
        if (String.IsNullOrEmpty(alias))
            throw new BO.BlInvalidInputException($"Task must has an alias");

        Console.WriteLine("Enter how many days needed to the task: ");
        TimeSpan? requiredEffortTime = null;
        input = Console.ReadLine();
        if (!String.IsNullOrEmpty(input))
            requiredEffortTime = new TimeSpan(Convert.ToInt32(input), 0, 0, 0);

        Console.WriteLine("Enter the Task's Start Date: ");
        DateTime? startDate;
        input = Console.ReadLine();
        if (String.IsNullOrEmpty(input))
            startDate = null;
        else
            startDate = Convert.ToDateTime(input);

        Console.WriteLine("Enter the Task's Complete Date: ");
        DateTime? completeDate;
        input = Console.ReadLine();
        if (String.IsNullOrEmpty(input))
            completeDate = null;
        else
            completeDate = Convert.ToDateTime(input);

        Console.WriteLine("Enter the Task's Deliverables: ");
        string? deliverables = Console.ReadLine();
        if (String.IsNullOrEmpty(deliverables))
            deliverables = null;

        Console.WriteLine("Enter the Task's Remarks: ");
        string? remarks = Console.ReadLine();
        if (String.IsNullOrEmpty(remarks))
            remarks = null;

        Console.WriteLine("Enter the engineer's ID  ");
        BO.EngineerInTask? engineerInTask = null;
        input = Console.ReadLine();
        if (!String.IsNullOrEmpty(input))
            engineerInTask = new() { Id = Convert.ToInt32(input) };


        Console.WriteLine("Enter the Complexity's level: ");
        int? complexityLevel;
        input = Console.ReadLine();
        if (String.IsNullOrEmpty(input))
            complexityLevel = null;
        else
            complexityLevel = Convert.ToInt32(input);


        BO.Task newTask = new()
        {
            Id = 0,
            Description = description,
            Alias = alias,
            Milestone = null,
            Dependencies = null,
            RequiredEffortTime = requiredEffortTime,
            CreatedAtDate = DateTime.Now,
            StartDate = startDate,
            DeadLineDate = null,
            ForecastDate = null,
            ScheduledDate = null,
            CompleteDate = completeDate,
            Deliverables = deliverables,
            Remarks = remarks,
            Engineer = engineerInTask,
            ComplexityLevel = (BO.EngineerExperience?)complexityLevel
        };
        s_bl.Task.Create(newTask);
    }

    private static void CreateProjectSchedule()
    {
        Console.WriteLine("Enter the Project Start Date: ");
        DateTime start;
        if (!DateTime.TryParse(Console.ReadLine(), out start))
            throw new BO.BlInvalidInputException($"Start Project Date {start} is invalid");

        Console.WriteLine("Enter the Project End Date: ");
        DateTime end;
        if (!DateTime.TryParse(Console.ReadLine(), out end))
            throw new BO.BlInvalidInputException($"Start Project Date {end} is invalid");

        s_bl.Milestone.CreateProjectSchedule(start, end);

    }

    #endregion

    #region Read Methods
    private static void readEngineer()
    {
        Console.WriteLine("Enter the Engineer's ID: ");
        int id;
        if (!int.TryParse(Console.ReadLine(), out id))
            throw new BO.BlInvalidInputException($"Engineer id {id} is invalid");
        Console.WriteLine(s_bl!.Engineer.Read(id));
    }
    private static void readTask()
    {
        Console.WriteLine("Enter the Task's ID: ");
        int id;
        if (!int.TryParse(Console.ReadLine(), out id))
            throw new BO.BlInvalidInputException($"Task id {id} is invalid");
        Console.WriteLine(s_bl!.Task.Read(id));
    }
    private static void readMilestone()
    {
        Console.WriteLine("Enter the Milestone ID: ");
        int id;
        if (!int.TryParse(Console.ReadLine(), out id))
            throw new BO.BlInvalidInputException($"Mileston id {id} is invalid");
        Console.WriteLine(s_bl!.Milestone.Read(id));
    }

    #endregion

    #region ReadAll Methods
    private static void readAllEngineers()
    {
        List<BO.Engineer>? engineers = s_bl.Engineer.ReadAll()?.ToList();
        if (engineers is not null)
            foreach (BO.Engineer eng in engineers)
            {
                Console.WriteLine(eng);
            }
    }
    private static void readAllTasks()
    {
        List<BO.Task>? tasks = s_bl.Task.ReadAll()?.ToList();
        if (tasks is not null)
            foreach (BO.Task task in tasks)
            {
                Console.WriteLine(task);
            }

    }

    #endregion

    #region Delete Methods
    private static void deleteEngineer()
    {
        Console.WriteLine("Enter the Engineer's ID: ");
        int id;
        if (!int.TryParse(Console.ReadLine(), out id))
            throw new BO.BlInvalidInputException($"Engineer id {id} is invalid");
        Console.WriteLine(s_bl.Engineer.Read(id));
        s_bl.Engineer.Delete(id);
    }
    private static void deleteTask()
    {
        Console.WriteLine("Enter the Task's ID: ");
        int id;
        if (!int.TryParse(Console.ReadLine(), out id))
            throw new BO.BlInvalidInputException($"Task id {id} is invalid");
        Console.WriteLine(s_bl.Task.Read(id));
        s_bl.Task.Delete(id);
    }


    #endregion

    #region Update Methods
    private static void updateEngineer()
    {

        Console.WriteLine("Enter the Engineer's ID: ");
        int id;
        if (!int.TryParse(Console.ReadLine(), out id))
            throw new BO.BlInvalidInputException($"Engineer id {id} is invalid");

        BO.Engineer oldBoEng = s_bl.Engineer.Read(id);
        Console.WriteLine(oldBoEng);

        Console.WriteLine("Enter the Engineer's name: ");
        string? name = Console.ReadLine();
        if (String.IsNullOrEmpty(name))
            name = oldBoEng.Name;

        Console.WriteLine("Enter the Engineer's level:\n  0- Novice, 1- AdvancedBeginner, 2- Competent, 3- Proficient, 4- Expert");
        int level;
        if (!int.TryParse(Console.ReadLine(), out level))
            level = (int)oldBoEng.Level;

        Console.WriteLine("Enter the Engineer's email: ");
        string? email = Console.ReadLine();
        if (String.IsNullOrEmpty(email))
            email = oldBoEng.Email;

        Console.WriteLine("Enter the Engineer's cost: ");
        double cost;
        if (!double.TryParse(Console.ReadLine(), out cost))
            cost = oldBoEng.Cost;


        BO.Engineer newEng = new()
        {
            Id = id,
            Name = name,
            Level = (BO.EngineerExperience)level,
            Email = email,
            Cost = cost,
            Task = null
        };
        s_bl.Engineer.Update(newEng);
    }
    private static void updateTask()
    {

        string? input;

        Console.WriteLine("Enter the Task's ID: ");
        int id;
        if (!int.TryParse(Console.ReadLine(), out id))
            throw new BO.BlInvalidInputException($"Task id {id} is invalid");

        BO.Task oldTask = s_bl.Task.Read(id);
        Console.WriteLine(oldTask);

        Console.WriteLine("Enter the Task's description: ");
        string? description = Console.ReadLine()!;
        if (String.IsNullOrEmpty(description))
            description = oldTask.Description;

        Console.WriteLine("Enter the Task's alias: ");
        string? alias = Console.ReadLine()!;
        if (String.IsNullOrEmpty(alias))
            alias = oldTask.Alias;

        Console.WriteLine("Enter how many days needed to the task: ");
        TimeSpan? requiredEffortTime = null;
        input = Console.ReadLine();
        if (!String.IsNullOrEmpty(input))
            requiredEffortTime = new TimeSpan(Convert.ToInt32(input), 0, 0, 0);

        Console.WriteLine("Enter the Task's Start Date: ");
        DateTime? startDate;
        input = Console.ReadLine();
        if (String.IsNullOrEmpty(input))
            startDate = oldTask.StartDate;
        else
            startDate = Convert.ToDateTime(input);


        Console.WriteLine("Enter the Task's Complete Date: ");
        DateTime? completeDate;
        input = Console.ReadLine();
        if (String.IsNullOrEmpty(input))
            completeDate = oldTask.CompleteDate;
        else
            completeDate = Convert.ToDateTime(input);

        Console.WriteLine("Enter the Task's Deliverables: ");
        string? deliverables = Console.ReadLine();
        if (String.IsNullOrEmpty(deliverables))
            deliverables = oldTask.Deliverables;

        Console.WriteLine("Enter the Task's Remarks: ");
        string? remarks = Console.ReadLine();
        if (String.IsNullOrEmpty(remarks))
            remarks = oldTask.Remarks;


        Console.WriteLine("Enter the engineer's ID  ");
        BO.EngineerInTask? engineerInTask = null;
        input = Console.ReadLine();
        if (!String.IsNullOrEmpty(input))
            engineerInTask = new() { Id = Convert.ToInt32(input) };
        else
            engineerInTask = oldTask.Engineer;

        Console.WriteLine("Enter the Complexity's level: ");
        int? complexityLevel;
        input = Console.ReadLine();
        if (String.IsNullOrEmpty(input))
            complexityLevel = (int?)oldTask.ComplexityLevel;
        else
            complexityLevel = Convert.ToInt32(input);
        BO.Task newTask = new()
        {
            Id = id,
            Description = description,
            Alias = alias,
            Milestone = oldTask.Milestone,
            RequiredEffortTime = requiredEffortTime,
            CreatedAtDate = oldTask.CreatedAtDate,
            StartDate = startDate,
            DeadLineDate = oldTask.DeadLineDate,
            ForecastDate = null,
            ScheduledDate = oldTask.ScheduledDate,
            CompleteDate = completeDate,
            Deliverables = deliverables,
            Remarks = remarks,
            Engineer = engineerInTask,
            ComplexityLevel = (BO.EngineerExperience?)complexityLevel
        };

        s_bl!.Task.Update(newTask);
    }
    private static void updateMilestone()
    {
        Console.WriteLine("Enter the Milestone's ID: ");
        int id;
        if (!int.TryParse(Console.ReadLine(), out id))
            throw new BO.BlInvalidInputException($"Milestone id {id} is invalid");

        BO.Milestone oldMilestone = s_bl.Milestone.Read(id);
        Console.WriteLine(oldMilestone);

        Console.WriteLine("Enter the Milestone's description: ");
        string? description = Console.ReadLine();

        Console.WriteLine("Enter the Milestone's alias: ");
        string? alias = Console.ReadLine();

        Console.WriteLine("Enter the Milestone's remarks: ");
        string? remarks = Console.ReadLine()!;

        s_bl.Milestone.Update(id, description, alias, remarks);
    }

    #endregion

}