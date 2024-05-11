using BO;
using PL.Manager;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for EntryWindow.xaml
    /// </summary>
    public partial class EntryWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public EntryWindow()
        {
            InitializeComponent();
        }

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

            new ManagerWindow().ShowDialog();
        }

        private void btnKeepTrackOfTime_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

