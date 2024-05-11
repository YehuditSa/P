namespace BO;


/// <summary>
/// An exception that occurs when a value that does not exist is accessed
/// </summary>
[Serializable]
public class BlDoesNotExistException : Exception
{
    public BlDoesNotExistException(string? message) : base(message) { }
}

/// <summary>
/// An exception that occurs when an attempt is made to add a value that already exists
/// </summary>
[Serializable]
public class BlAlreadyExistsException : Exception
{
    public BlAlreadyExistsException(string? message) : base(message) { }
}

/// <summary>
/// An exception that occurs when an attempt is made to delete a non-deletable value
/// </summary>
[Serializable]
public class BlDeletionImpossibleException : Exception
{
    public BlDeletionImpossibleException(string? message) : base(message) { }
}

/// <summary>
/// An exception that occurs when there is any problem in accessing to XMl file
/// </summary>


[Serializable]
public class BlXMLFileLoadCreateException : Exception
{
    public BlXMLFileLoadCreateException(string? message) : base(message) { }
}

/// <summary>
///An exception that occurs when trying to assign null to not-nullable property 
/// </summary>

[Serializable]
public class BlNullPropertyException : Exception
{
    public BlNullPropertyException(string? message) : base(message) { }
}

/// <summary>
/// An exception that occurs when there is a general error with null
/// </summary>

[Serializable]
public class BlNullException : Exception
{
    public BlNullException(string? message) : base(message) { }
}

/// <summary>
/// An exception for invalid input 
/// </summary>

[Serializable]
public class BlInvalidInputException : Exception
{
    public BlInvalidInputException(string? message) : base(message) { }
}

/// <summary>
/// An exception that occurs when the manager didn't allocate enough time to complete the tasks
/// </summary>

[Serializable]
public class BlPlanningOfProjectTimesException : Exception
{
    public BlPlanningOfProjectTimesException(string? message) : base(message) { }
}

/// <summary>
/// An exception that occurs when trying to get id of milestone - and this id is of a regular task
/// </summary>

[Serializable]
public class BlTaskIsNotMilestoneException : Exception
{
    public BlTaskIsNotMilestoneException(string? message) : base(message) { }
}

[Serializable]
public class BlUnknownException : Exception
{
    public BlUnknownException(string? message) : base(message) { }
}

