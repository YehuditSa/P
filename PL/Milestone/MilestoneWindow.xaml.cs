using PL.Task;
using System;
using System.Collections.Generic;
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

namespace PL.Milestone
{


    /// <summary>
    /// Interaction logic for MilestoneWindow.xaml
    /// </summary>
    public partial class MilestoneWindow : Window
    {


        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        /// <summary>
        /// The milestone that is actually displayed - Is binded to the the window
        /// </summary>
        public BO.Milestone CurrentMilestone
        {
            get { return (BO.Milestone)GetValue(CurrentMilestoneProperty); }
            set { SetValue(CurrentMilestoneProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentMilestone.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentMilestoneProperty =
            DependencyProperty.Register("CurrentMilestone", typeof(BO.Milestone), typeof(MilestoneWindow));


        /// <summary>       
        /// Window constructor
        /// </summary>
        /// <param name="Id"></param>
        public MilestoneWindow(int Id)
        {
            InitializeComponent();
            try
            {
                CurrentMilestone = s_bl.Milestone.Read(Id);
            }
            catch
            {
                MessageBox.Show("A system error occurred. Please try again.");
            }
        }


        #region UI Event Handlers

        private void btnUpdateMilestone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.Milestone.Update(CurrentMilestone.Id, CurrentMilestone.Description,CurrentMilestone.Alias,CurrentMilestone.Remarks);
                MessageBox.Show($"Milestone {CurrentMilestone.Id} has been updated successfully");
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
  

        #endregion
    }
}
