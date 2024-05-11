
namespace BlApi;

/// <summary>
/// Implementates the design pattern "Simple Factory" 
/// </summary>
public static class Factory
{
    public static IBl Get() => new BlImplementation.Bl();
}
