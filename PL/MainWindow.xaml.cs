using PL.Engineer;
using PL.Manager;
using PL.Task;
using System.Windows;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public MainWindow()
        {
            InitializeComponent();
        }

        #region UI Event Handlers

        private void btnEngineer_Click(object sender, RoutedEventArgs e)
        {
            string id = Microsoft.VisualBasic.Interaction.InputBox("Enter Engineer ID", "ID");
            try
            {
                BO.Engineer boEng = s_bl.Engineer.Read(int.Parse(id));
                //פתיחת חלון מהנדס
            }
            catch
            {
                MessageBox.Show($"No engineer was found with this {id} ID number. Please try again.");
            }

        }


        private void btnManager_Click(object sender, RoutedEventArgs e)
        {

            new ManagerWindow().Show();
        }

        private void btnKeepTrackOfTime_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion


    }
}
