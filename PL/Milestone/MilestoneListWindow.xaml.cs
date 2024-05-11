using BO;
using PL.Task;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

namespace PL.Milestone;



/// <summary>
/// Interaction logic for MilestoneListWindow.xaml
/// </summary>
public partial class MilestoneListWindow : Window
{

    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    /// <summary>
    /// The value of comboBox for filter tasks
    /// </summary>
    public BO.Status Status { get; set; } = BO.Status.Unscheduled;



    public bool IsShowButton
    {
        get { return (bool)GetValue(IsShowButtonProperty); }
        set { SetValue(IsShowButtonProperty, value); }
    }

    // Using a DependencyProperty as the backing store for IsShowButton.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IsShowButtonProperty =
        DependencyProperty.Register("IsShowButton", typeof(bool), typeof(TaskListWindow), new PropertyMetadata(null));
    /// <summary>
    /// Window constructor
    /// </summary>
    public MilestoneListWindow()
    {
        InitializeComponent();
        IsShowButton = false;
        Activated += OnWindowActivated;

    }

    /// <summary>
    /// The list of tasks that is actually displayed - Is binded to the list in the window
    /// </summary>
    public ObservableCollection<BO.MilestoneInList> MilestoneList
    {
        get { return (ObservableCollection<BO.MilestoneInList>)GetValue(MilestoneListProperty); }
        set { SetValue(MilestoneListProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Task List.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty MilestoneListProperty =
        DependencyProperty.Register("MilestoneList", typeof(ObservableCollection<BO.MilestoneInList>), typeof(MilestoneListWindow), new PropertyMetadata(null));

    private List<BO.MilestoneInList> convertMilestoneToMilestoneInList(IEnumerable<BO.Milestone?> origionalMilestoneList)
    {
        List<BO.MilestoneInList> newMilestoneList = new List<BO.MilestoneInList>();
        foreach (var oldMilestone in origionalMilestoneList)
        {
            if (oldMilestone is not null)
            {
                BO.MilestoneInList newMilestone = new()
                {
                    Id = oldMilestone.Id,
                    Description = oldMilestone.Description,
                    Alias = oldMilestone.Alias,
                    Status = oldMilestone.Status,
                    CompletionPercentage = oldMilestone.CompletionPercentage
                };
                newMilestoneList.Add(newMilestone);
            }
        }
        return newMilestoneList;
    }

    /// <summary>
    /// Performing a renewed query of the list of tasks every time the window becomes active
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnWindowActivated(object? sender, EventArgs e)
    {
        IEnumerable<BO.Milestone?>? temp = s_bl?.Milestone.ReadAll();
        MilestoneList = temp is null ? new() : new(convertMilestoneToMilestoneInList(temp));
    }

    #region UI Event Handlers

        /// <summary>
        /// Filter the Tasks list - by the Status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

        private void cbStatusSelector_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            var temp = s_bl?.Milestone.ReadAll(item => item?.Status == Status);
            MilestoneList = temp is null ? new() : new(convertMilestoneToMilestoneInList(temp));
            IsShowButton = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShowAll_Click(object? sender, RoutedEventArgs e)
        {
            var temp = s_bl?.Milestone.ReadAll();
            MilestoneList = temp is null ? new() : new(convertMilestoneToMilestoneInList(temp));
            IsShowButton = false;
        }

        /// <summary>
        /// Open a window for updating a specific Milestone details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnUpdateMilestone_MDClick(object sender, MouseButtonEventArgs e)
        {

            BO.MilestoneInList? selectedMilestone = ((sender as ListView)?.SelectedItem as BO.MilestoneInList);
            if (selectedMilestone is not null)
                new MilestoneWindow(selectedMilestone!.Id).ShowDialog();
        }

        private void ListView_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
         
        }
    #endregion
}
