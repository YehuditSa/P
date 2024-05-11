using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;


namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EngineerListWindow.xaml
    /// </summary>
    public partial class EngineerListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        /// <summary>
        /// The value of comboBox for filter engineers
        /// </summary>
        public BO.EngineerExperience Level { get; set; } = BO.EngineerExperience.None;

        /// <summary>
        /// The list of engineers that is actually displayed - Is binded to the list in the window
        /// </summary>
        public ObservableCollection<BO.Engineer> EngineerList
        {
            get { return (ObservableCollection<BO.Engineer>)GetValue(EngineerListProperty); }
            set { SetValue(EngineerListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Engineer List.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EngineerListProperty =
            DependencyProperty.Register("EngineerList", typeof(ObservableCollection<BO.Engineer>), typeof(EngineerListWindow), new PropertyMetadata(null));
        
        /// <summary>
        /// Window constructor
        /// </summary>
        public EngineerListWindow()
        {
            InitializeComponent();
            Activated += OnWindowActivated;

        }

        /// <summary>
        /// Performing a renewed query of the list of engineers every time the window becomes active
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowActivated(object? sender, EventArgs e)
        {
            var temp = s_bl?.Engineer.ReadAll();
            EngineerList = temp == null ? new() : new(temp);
        }

        #region UI Event Handlers

            /// <summary>
            /// Filter the Engineers list - by the level
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void cbLevelSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                var temp = Level == BO.EngineerExperience.None ?
                s_bl?.Engineer.ReadAll() :
                s_bl?.Engineer.ReadAll(item => (BO.EngineerExperience?)item?.Level == Level);
                EngineerList = temp == null ? new() : new(temp);

            }

            /// <summary>
            /// Open a window for adding new engineer
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void btnAddEngineer_Click(object sender, RoutedEventArgs e)
            {
                new EngineerWindow().ShowDialog();
            }

            /// <summary>
            /// Open a window for updating a specific engineer details
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void btnUpdateEngineer_MDClick(object sender, RoutedEventArgs e)
            {
                int? selectedEngId = ((sender as ListView)?.SelectedItem as BO.Engineer)?.Id;
                if (selectedEngId is not null)
                    new EngineerWindow(selectedEngId.Value).ShowDialog();
            }

        #endregion

    }
}
