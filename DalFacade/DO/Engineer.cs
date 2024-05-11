
namespace DO;

/// <summary>
/// Engineer Entity
/// </summary>
/// <param name="Id">Unique ID</param>
/// <param name="Name">Full name</param>
/// <param name="Level">The level of the engineer</param>
/// <param name="Email"></param>
/// <param name="Cost">Cost per Hour</param>
public record Engineer
(
    int Id,
    string Name,
    EngineerExperience Level,
    string Email,
    double Cost
)
{
    Engineer() : this(0, "", 0, "", 0.0) { } //empty ctor
};