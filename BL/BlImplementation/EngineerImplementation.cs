using BlApi;
using System.Net.Mail;

namespace BlImplementation;

/// <summary>
/// A class that implements the CRUD methods for Logic Engineer entity
/// </summary>
internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    /// <summary>
    /// Auxiliary method - Checks for logical errors in the input
    /// </summary>
    /// <param name="boEngineer"></param>
    /// <exception cref="BO.BlNullPropertyException"></exception>
    /// <exception cref="BO.BlInvalidInputException"></exception>
    private void CheckingValidationOfInput(BO.Engineer boEngineer)
    {
        if (boEngineer.Name is null)
            throw new BO.BlNullPropertyException("Engineer name can't be null");

        if (boEngineer.Email is null)
            throw new BO.BlNullPropertyException("Engineer email can't be null");

        if (boEngineer.Id <= 0)
            throw new BO.BlInvalidInputException("Engineer id must be a positive number");

        if (boEngineer.Name == "")
            throw new BO.BlInvalidInputException("Engineer name can't be empty string");

        try
        {
            MailAddress mail = new MailAddress(boEngineer.Email);
        }
        catch
        {
            throw new BO.BlInvalidInputException("Engineer email is not valid");
        }

        if (boEngineer.Cost <= 0)
            throw new BO.BlInvalidInputException("Engineer cost must be a positive number");
    }

    /// <summary>
    /// Creates a new Engineer in DAL
    /// </summary>
    /// <param name="boEngineer"></param>
    /// <returns> The id of the new Engineer</returns>
    /// <exception cref="BO.BlAlreadyExistsException"></exception>
    public int Create(BO.Engineer boEngineer)
    {
        CheckingValidationOfInput(boEngineer);
        int idEng = 0;
        try
        {
            idEng = _dal.Engineer.Create(new DO.Engineer(boEngineer.Id, boEngineer.Name!, (DO.EngineerExperience)boEngineer.Level, boEngineer.Email!, boEngineer.Cost));
        }
        catch(Exception ex)
        {
            if (ex is DO.DalAlreadyExistsException)
                throw new BO.BlAlreadyExistsException($"Engineer with ID={boEngineer.Id} already exists");
            else
                throw new BO.BlUnknownException($"Unknown Exception in Create the new Engineer");
        }
        return idEng;
    }

    /// <summary>
    /// Reads an engineer by its Id from dal and creates a logical Engineer entity
    /// </summary>
    /// <param name="id"></param>
    /// <returns>The required engineer</returns>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public BO.Engineer Read(int id)
    {
        DO.Engineer? doEngineer = _dal.Engineer.Read(id);
        if (doEngineer is null)
            throw new BO.BlDoesNotExistException($"Engineer with ID={id} does not exist");

        List<DO.Task?> allTasks = _dal.Task.ReadAll().ToList();


        //Engineer current task == After Start Date and Before Complited Date
        DO.Task? doTask = (from task in allTasks
                           where task.EngineerId == id && task.StartDate <= DateTime.Now && task.CompleteDate is null
                           select task).FirstOrDefault();

        BO.TaskInEngineer? taskInEngineer = null;
        if (doTask is not null)
        {
            taskInEngineer = new BO.TaskInEngineer()
            {
                Id = doTask.Id,
                Alias = doTask.Alias
            };
        }

        return new BO.Engineer()
        {
            Id = id,
            Name = doEngineer.Name,
            Email = doEngineer.Email,
            Level = (BO.EngineerExperience)doEngineer.Level,
            Cost = doEngineer.Cost,
            Task = taskInEngineer
        };
    }

    /// <summary>
    /// Reads all engineers with option of reading by a filter function
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>A list of the reqiered engineers or all engineers - if there is no filter function</returns>
    public IEnumerable<BO.Engineer>? ReadAll(Func<DO.Engineer?, bool>? filter = null)
    {
        List<DO.Engineer?> doEngineers = _dal.Engineer.ReadAll(filter).ToList();
        return (from engineer in doEngineers
                select Read(engineer.Id));
    }

    /// <summary>
    /// Deletes an engineer from dal by its Id
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="BO.BlDeletionImpossibleException"></exception>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public void Delete(int id)
    {
        BO.Engineer boEngineer = Read(id);

        List<DO.Task?> allTasks = _dal.Task.ReadAll().ToList();
        DO.Task? doTask = (from task in allTasks
                           where task.EngineerId == id && task.StartDate <= DateTime.Now
                           select task).FirstOrDefault();
        if (doTask is not null)
            throw new BO.BlDeletionImpossibleException("Can't delete an engineer who is busy performing a task");
        try
        {
            _dal.Engineer.Delete(id);
        }
        catch (Exception ex)
        {
            if (ex is DO.DalDoesNotExistException)
                throw new BO.BlDoesNotExistException($"Engineer with ID={id} doesn't exist");
            else
                throw new BO.BlUnknownException($"Unknown Exception in Delete the Engineer");
        }
    }

    /// <summary>
    ///  Updates an engineer detailes in dal
    /// </summary>
    /// <param name="boEngineer">An entity with updated data</param>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public void Update(BO.Engineer boEngineer)
    {
        CheckingValidationOfInput(boEngineer);
        try
        {
            _dal.Engineer.Update(new DO.Engineer(boEngineer.Id, boEngineer.Name!, (DO.EngineerExperience)boEngineer.Level, boEngineer.Email!, boEngineer.Cost));
        }
        catch (Exception ex)
        {
            if (ex is DO.DalDoesNotExistException)
                throw new BO.BlDoesNotExistException($"Engineer with ID={boEngineer.Id} doesn't exist");
            else
                throw new BO.BlUnknownException($"Unknown Exception in Update the Engineer details");
        }
    }
}
