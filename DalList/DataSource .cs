

namespace Dal;
using DO;

/// <summary>
/// Creating the data lists for the entities that exists in the project
/// Contains a class for creating a running number
/// </summary>
internal static class DataSource
{
    internal static List<DO.Task?> Tasks { get; } = new();
    internal static List<DO.Dependency?> Dependencies { get; } = new();
    internal static List<DO.Engineer?> Engineers { get; } = new();

    /// <summary>
    /// Creating a running id for Task and Dependency
    /// </summary>
    internal static class Config
    {
        internal const int StartTaskId = 1;
        private static int nextTaskId = StartTaskId;
        internal static int NextTaskId { get => nextTaskId++; }

        internal const int StartDependencyId = 1;
        private static int nextDependencyId = StartDependencyId;
        internal static int NextDependencyId { get => nextDependencyId++; }

        internal static DateTime? startProjectDate = null;
        internal static DateTime? endProjectDate = null;

    }


}


