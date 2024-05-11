namespace DalTest;
using DalApi;
using DO;
using System.Security.Cryptography;
using System.Xml.Linq;

/// <summary>
/// Adding the beginning data of the project (5 engineers, 20 tasks and 40 dependencies)
/// </summary>
public static class Initialization
{

    private static IDal? s_dal;

    private static readonly Random s_rand = new(); //A variable for random numbers in the class

    /// <summary>
    /// Creating 5 engineers
    /// </summary>
    private static void createEngineers()
    {
        string[] engineersNames =
        {
        "Shimon Levi", "Moshe Amar", "Chaim Cohen",
        "Ariel Levin", "Avi Klein"
        };

        foreach (string _name in engineersNames)
        {
            int _id;
            do
                _id = s_rand.Next(200000000, 400000000); //Creating id to the engineer 
            while (s_dal!.Engineer.Read(_id) != null); //Making sure that the new id does not exist

            string _email = _name.Replace(" ", "") + "@gmail.com"; //The engineer email is: ENGINEER_NAME (without spaces)@GMAIL.COM

            EngineerExperience _level = (EngineerExperience)s_rand.Next(0, 5); //Rand the level of the engineer

            double _cost = Math.Round(s_rand.NextDouble() * 99 + 1, 2); //rand double from 1 to 100 in 2 decimal places
            _cost = _cost + 100 * ((int)_level + 1); //The cost per hour to engineer is according to his level.
                                                     //For example: AdvancedBeginner gets 200 - 300 


            Engineer newEng = new(_id, _name, _level, _email, _cost);

            s_dal!.Engineer.Create(newEng);
        }

        Engineer newEng1 = new(123, "rut", (EngineerExperience)s_rand.Next(0, 5), "ruy@", 3000);

        s_dal!.Engineer.Create(newEng1);
    }

    /// <summary>
    /// Creating 20 tasks
    /// </summary>
    private static void createTasks()
    {
        #region Tasks Descriptions
        string[] tasksDescriptions ={"Conduct research and analysis to develop new engineering designs or improve existing ones.",
    "Create and review technical drawings, blueprints, and specifications for construction projects.",
    "Collaborate with other engineers and professionals to brainstorm and solve complex engineering problems.",
    "Perform calculations and simulations to determine the feasibility, safety, and efficiency of engineering solutions.",
    "Test and evaluate prototypes or products to ensure they meet design specifications and performance requirements.",
    "Develop and implement quality control measures to ensure the reliability and consistency of engineering projects.",
    "Manage project timelines, budgets, and resources to ensure successful completion of engineering projects.",
    "Stay up-to-date with the latest advancements and trends in engineering technology and techniques.",
    "Conduct site visits and inspections to assess construction progress and compliance with engineering standards.",
    "Collaborate with manufacturing teams to optimize production processes and improve product quality.",
    "Troubleshoot and resolve technical issues or malfunctions in engineering systems or equipment.",
    "Provide technical guidance and support to junior engineers or technicians.",
    "Maintain accurate records and documentation of engineering designs, tests, and modifications.",
    "Ensure compliance with relevant safety, health, and environmental regulations.",
    "Conduct feasibility studies and cost-benefit analyses for potential engineering projects.",
    "Collaborate with clients or stakeholders to understand project requirements and objectives.",
    "Develop and implement strategies for sustainable and environmentally friendly engineering practices.",
    "Participate in professional development activities, such as conferences or workshops, to enhance engineering skills and knowledge.",
    "Prepare and present reports, proposals, or recommendations to clients, management, or regulatory authorities.",
    "Collaborate with cross-functional teams, including marketing and sales, to bring engineering projects to market."};
        #endregion

        #region Deliverables
        string[] deliverables = {
                "Designs",
                "Drawings",
                "Solutions",
                "Calculations",
                "Testing results",
                "Quality reports",
                "Completed projects",
                "Knowledge",
                "Inspection reports",
                "Optimized processes",
                "Resolved issues",
                "Guidance",
                "Documentation",
                "Compliance reports",
                "Feasibility studies",
                "Requirements",
                "Sustainability strategies",
                "Skills and knowledge",
                "Reports and proposals",
                "Collaboration"};
        #endregion

        for (int i = 0; i < tasksDescriptions.Length; i++)
        {
            string _alias = "T" + i.ToString();

            var randomTest = new Random();
            TimeSpan timeSpan = DateTime.Now - new DateTime(2022, 1, 1);
            TimeSpan newSpan = new TimeSpan(0, randomTest.Next(0, (int)timeSpan.TotalMinutes), 0);
            DateTime _createdAtDate = new DateTime(2022, 1, 1) + newSpan;

            TimeSpan hoursFromCreatingToStartDate = new TimeSpan(s_rand.Next(0, 6), 0, 0, 0); //0-5 days
            DateTime? _startDate = _createdAtDate + hoursFromCreatingToStartDate;
            TimeSpan _hoursFromStartDateTodeadLine = new TimeSpan(s_rand.Next(20, 31), 0, 0, 0); //20-30 days
            DateTime? _deadLineDate = _startDate + _hoursFromStartDateTodeadLine;

            EngineerExperience? _complexityLevel = (EngineerExperience)s_rand.Next(0, 5);

            Task newTask = new()
            {
                Id = 0,
                Description = tasksDescriptions[i],
                Alias = _alias,
                IsMilestone = false,
                CreatedAtDate = DateTime.Now,
                RequiredEffortTime = _hoursFromStartDateTodeadLine,
                StartDate = _startDate,
                DeadLineDate = _deadLineDate,
                Remarks = "(:",
                ComplexityLevel = (EngineerExperience?)_complexityLevel
            };

            s_dal!.Task.Create(newTask);

        }

        //Data for checking correctness of createMilestons
        //s_dal!.Task.Create(new Task()
        //{
        //    Id = 0,
        //    Description = "task1",
        //    Alias = "1",
        //    IsMilestone = false,
        //    CreatedAtDate = DateTime.Now,
        //    RequiredEffortTime = new TimeSpan(2, 0, 0, 0)
        //});

        //s_dal!.Task.Create(new Task()
        //{
        //    Id = 0,
        //    Description = "task2",
        //    Alias = "2",
        //    IsMilestone = false,
        //    CreatedAtDate = DateTime.Now,
        //    RequiredEffortTime = new TimeSpan(3, 0, 0, 0)
        //});

        //s_dal!.Task.Create(new Task()
        //{
        //    Id = 0,
        //    Description = "task3",
        //    Alias = "3",
        //    IsMilestone = false,
        //    CreatedAtDate = DateTime.Now,
        //    RequiredEffortTime = new TimeSpan(3, 0, 0, 0)
        //});

        //s_dal!.Task.Create(new Task()
        //{
        //    Id = 0,
        //    Description = "task4",
        //    Alias = "4",
        //    IsMilestone = false,
        //    CreatedAtDate = DateTime.Now,
        //    RequiredEffortTime = new TimeSpan(2, 0, 0, 0)
        //});

        //s_dal!.Task.Create(new Task()
        //{
        //    Id = 0,
        //    Description = "task5",
        //    Alias = "5",
        //    IsMilestone = false,
        //    CreatedAtDate = DateTime.Now,
        //    RequiredEffortTime = new TimeSpan(1, 0, 0, 0)
        //});

    }

    /// <summary>
    /// Creating 40 dependencies
    /// </summary>
    private static void createDependencies()
    {
        List<Task> tasks = (List<Task>)s_dal!.Task.ReadAll();

        for (int i = 0; i < 40; i++)
        {
            int indexOfDepedenceOn = s_rand.Next(0, tasks.Count - 1);
            int indexOfDependent = s_rand.Next(indexOfDepedenceOn + 1, tasks.Count - 1);
            addDependency(indexOfDependent, indexOfDepedenceOn);
        }

        void addDependency(int indexOfDependent, int indexOfDepedenceOn)
        {
            Dependency newDep = new(0, tasks[indexOfDependent].Id, tasks[indexOfDepedenceOn].Id);
            s_dal!.Dependency.Create(newDep);
        }

        //Data for checking correctness of createMilestons
        //s_dal!.Dependency.Create(new Dependency(0, 3, 1));
        //s_dal!.Dependency.Create(new Dependency(0, 3, 2));
        //s_dal!.Dependency.Create(new Dependency(0, 4, 3));
        //s_dal!.Dependency.Create(new Dependency(0, 5, 3));
    }

    /// <summary>
    /// Generating the initializations of the list
    /// </summary>
    /// <param name="dal"></param>
    /// <exception cref="NullReferenceException"></exception>
    public static void Do()
    {
        s_dal = DalApi.Factory.Get;
        createEngineers();
        createTasks();
        createDependencies();
    }

}





