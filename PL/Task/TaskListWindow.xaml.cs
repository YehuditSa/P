using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

using System.Windows.Controls;


namespace PL.Task;

/// <summary>
/// Interaction logic for TaskListWindow.xaml
/// </summary>
public partial class TaskListWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    /// <summary>
    /// The value of comboBox for filter tasks
    /// </summary>
    public BO.Status Status { get; set; } = BO.Status.Unscheduled;

    public bool IsWindowInNormalState { get; set; } = true;
    public event EventHandler? addDependencyEvent;



    public bool IsShowButton
    {
        get { return (bool)GetValue(IsShowButtonProperty); }
        set { SetValue(IsShowButtonProperty, value); }
    }

    // Using a DependencyProperty as the backing store for IsShowButton.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IsShowButtonProperty =
        DependencyProperty.Register("IsShowButton", typeof(bool), typeof(TaskListWindow), new PropertyMetadata(null));




    /// <summary>
    /// The list of tasks that is actually displayed - Is binded to the list in the window
    /// </summary>
    public ObservableCollection<BO.TaskInList> TaskList
    {
        get { return (ObservableCollection<BO.TaskInList>)GetValue(TaskListProperty); }
        set { SetValue(TaskListProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Task List.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TaskListProperty =
        DependencyProperty.Register("TaskList", typeof(ObservableCollection<BO.TaskInList>), typeof(TaskListWindow), new PropertyMetadata(null));

    /// <summary>
    /// Window constructor
    /// </summary>
    public TaskListWindow(bool isForAddDependency = false)
    {
        IsShowButton = false;
        if (isForAddDependency == true)
        {
            IsWindowInNormalState = false;
        }
        InitializeComponent();
        Activated += OnWindowActivated;

    }

    private List<BO.TaskInList> convertTaskToTaskInList(IEnumerable<BO.Task?> origionalTaskList)
    {
        List<BO.TaskInList> newTaskList = new List<BO.TaskInList>();
        foreach (var oldTask in origionalTaskList)
        {
            if (oldTask is not null)
            {
                BO.TaskInList newTask = new()
                {
                    Id = oldTask.Id,
                    Description = oldTask.Description,
                    Alias = oldTask.Alias,
                    Status = oldTask.Status
                };
                newTaskList.Add(newTask);
            }
        }
        return newTaskList;
    }

    /// <summary>
    /// Performing a renewed query of the list of tasks every time the window becomes active
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnWindowActivated(object? sender, EventArgs e)
    {
        IEnumerable<BO.Task?>? temp = s_bl?.Task.ReadAll();
        TaskList = temp is null ? new() : new(convertTaskToTaskInList(temp));
    }

    #region UI Event Handlers

    /// <summary>
    /// Filter the Tasks list - by the Status
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// 

    private void cbStatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var temp = s_bl?.Task.ReadAll(item => item?.Status == Status);
        TaskList = temp is null ? new() : new(convertTaskToTaskInList(temp));
        IsShowButton = true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnShowAll_Click(object sender, RoutedEventArgs e)
    {
        var temp = s_bl?.Task.ReadAll();
        TaskList = temp is null ? new() : new(convertTaskToTaskInList(temp));
        IsShowButton = false;
    }


    /// <summary>
    /// Open a window for adding new task
    /// </summary>
    /// <param name = "sender" ></ param >
    /// < param name="e"></param>
    private void btnAddTask_Click(object sender, RoutedEventArgs e)
    {
        new TaskWindow().ShowDialog();
    }

    /// <summary>
    /// Open a window for updating a specific task details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnUpdateTask_MDClick(object sender, MouseButtonEventArgs e)
    {
            if (IsWindowInNormalState)
            {
                BO.TaskInList? selectedTask = ((sender as ListView)?.SelectedItem as BO.TaskInList);
                if (selectedTask is not null)
                    new TaskWindow(selectedTask!.Id).ShowDialog();
            }
            else
            {
                if(addDependencyEvent is not null)
                    addDependencyEvent(sender, e);
                this.Close();
            }
    }
    #endregion
}