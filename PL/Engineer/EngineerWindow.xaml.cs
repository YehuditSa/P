using System;
using System.Windows;


namespace PL.Engineer
{

    /// <summary>
    /// Interaction logic for EngineerWindow.xaml
    /// </summary>
    public partial class EngineerWindow : Window
    {

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        private StateOfWindow engWindowState;

        /// <summary>
        /// The engineer that is actually displayed - Is binded to the the window
        /// </summary>
        public BO.Engineer CurrentEngineer
        {
            get { return (BO.Engineer)GetValue(CurrentEngineerProperty); }
            set { SetValue(CurrentEngineerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentEngineer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentEngineerProperty =
            DependencyProperty.Register("CurrentEngineer", typeof(BO.Engineer), typeof(EngineerWindow));


        /// <summary>
        /// Window constructor
        /// </summary>
        /// <param name="Id"></param>
        public EngineerWindow(int Id = 0)
        {
            InitializeComponent();
          
            if (Id == 0)
            {
                CurrentEngineer = new BO.Engineer();
                engWindowState = StateOfWindow.Add;
            }
            else
            {
                try
                {
                    CurrentEngineer = s_bl.Engineer.Read(Id);
                    engWindowState = StateOfWindow.Update;
                }
                catch 
                {
                    MessageBox.Show("A system error occurred. Please try again.");
                }
            }
        }

     
        #region UI Event Handlers

        private void btnAddOrUpdateEngineer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (engWindowState == StateOfWindow.Add)
                {
                    s_bl.Engineer.Create(CurrentEngineer);
                    MessageBox.Show($"Engineer {CurrentEngineer.Id} has been added successfully");
                }
                else
                {
                    s_bl.Engineer.Update(CurrentEngineer);
                    MessageBox.Show($"Engineer {CurrentEngineer.Id} has been updated successfully");
                }
                this.Close();
            }
            catch (Exception ex)
            {
                if(ex is BO.BlAlreadyExistsException)
                    MessageBox.Show($"Engineer with {CurrentEngineer.Id} is already exists.\n Please try another id");
                else if(ex is BO.BlNullPropertyException || ex is BO.BlInvalidInputException)
                    MessageBox.Show($"One of the inputs is wrong: {ex.Message}");
                else
                    MessageBox.Show("A system error occurred. Please try again.");
            }
        }

        #endregion


    }
}
