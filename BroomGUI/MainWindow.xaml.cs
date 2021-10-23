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
using System.Windows.Navigation;
using System.Windows.Shapes;
using FindFolders;

namespace BroomGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<FindPathsFolders> removeList = RemoveList.GetRemoveList();

        public MainWindow()
        {
            InitializeComponent();

            ListView_folders.ItemsSource = removeList;
        }

        private void Button_startCleaning_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}