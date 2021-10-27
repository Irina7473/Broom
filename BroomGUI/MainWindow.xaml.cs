using System;
using System.IO;
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
        ObservableCollection<FindPathsFolders> removeList;
        List<string> selectedList;

        public MainWindow()
        {
            InitializeComponent();

            removeList = RemoveList.GetRemoveList();
            ListView_folders.ItemsSource = removeList;
            selectedList = new List<string>();
        }

        private void Button_startCleaning_Click(object sender, RoutedEventArgs e)
        {                       
            foreach (var item in selectedList)
            {
                MessageBox.Show(item);
                foreach (var element in removeList)
                {                    
                    if (item == element.Name)
                    {
                        MessageBox.Show(element.Path);
                    }
                }
            }            
        }

        private void CheckBox_select_Checked(object sender, RoutedEventArgs e)
        {                        
            if (((CheckBox)sender).IsChecked == true)
            {
                selectedList.Add((sender as CheckBox).Content.ToString());
            }            
        }
    }
}