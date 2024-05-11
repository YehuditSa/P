using BO;
using PL.Engineer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.Task;

/// <summary>
/// Interaction logic for TaskWindow.xaml
/// </summary>
public partial class TaskWindow : Window
{

    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    private StateOfWindow taskWindowState;

    private List<int> newDependenciesTasksIds = new List<int>();
    void addNewDependencyHandler(object? sender, EventArgs eventArgs)
    {
        BO.TaskInList? selectedTask = ((sender as ListView)?.SelectedItem as BO.TaskInList); 
        if(selectedTask != null)
        {
            DependenciesList.Add(selectedTask);
            newDependenciesTasksIds.Add(selectedTask.Id);
        }
    }

    /// <summary>
    /// The task that is actually displayed - Is binded to the the window
    /// </summary>
    public BO.Task CurrentTask
    {
        get { return (BO.Task)GetValue(CurrentTaskProperty); }
        set { SetValue(CurrentTaskProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CurrentTask.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CurrentTaskProperty =
        DependencyProperty.Register("CurrentTask", typeof(BO.Task), typeof(TaskWindow));


    public ObservableCollection<BO.TaskInList> DependenciesList
    {
        get { return (ObservableCollection<BO.TaskInList>)GetValue(DependenciesListProperty); }
        set { SetValue(DependenciesListProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Task List.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty DependenciesListProperty =
        DependencyProperty.Register("DependenciesList", typeof(ObservableCollection<BO.TaskInList>), typeof(TaskWindow), new PropertyMetadata(null));


    /// <summary>       
    /// Window constructor
    /// </summary>
    /// <param name="Id"></param>
    public TaskWindow(int Id = 0)
    {
        InitializeComponent();
        if (Id == 0)
        {
            CurrentTask = new BO.Task();
            taskWindowState = StateOfWindow.Add;
        }
        else
        {
            try
            {
                CurrentTask = s_bl.Task.Read(Id);
                taskWindowState = StateOfWindow.Update;
            }
            catch
            {
                MessageBox.Show("A system error occurred. Please try again.");
            }
        }
        if(CurrentTask.Dependencies is not null)
            DependenciesList = new(CurrentTask.Dependencies);
    }


    #region UI Event Handlers

    private void btnAddOrUpdateTask_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            CurrentTask.Dependencies = DependenciesList.ToList();
            if (taskWindowState == StateOfWindow.Add)
            {
                s_bl.Task.Create(CurrentTask);
                MessageBox.Show($"Task {CurrentTask.Id} has been added successfully");
            }
            else
            {
                s_bl.Task.Update(CurrentTask);
                MessageBox.Show($"Task {CurrentTask.Id} has been updated successfully");
            }
            foreach(int id in newDependenciesTasksIds)
            {
                s_bl.addDependency(id, CurrentTask.Id);
            }
            this.Close();
        }
        catch (Exception ex)
        {
            if (ex is BO.BlNullPropertyException || ex is BO.BlInvalidInputException)
                MessageBox.Show($"One of the inputs is wrong: {ex.Message}");
            else
                MessageBox.Show("A system error occurred. Please try again.");
        }
    }
    private void btnAddDependencyTask_Click(object sender, RoutedEventArgs e)
    {
        var taskListWindow = new TaskListWindow(true);
        taskListWindow.addDependencyEvent += addNewDependencyHandler;
        taskListWindow.ShowDialog();

    }

    #endregion


}



