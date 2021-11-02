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
using FilesAndFolders;
using Logger;

namespace BroomGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<ActionsWithFilesAndFolders> removeList;
        List<string> selectedList;
        //LogToDB log;
        LogToFile log2;
        Message record;

        public MainWindow()
        {
            InitializeComponent();
            /*
            log = new LogToDB();
            record = log.RecordToLog;
            record += AppendFormattedText;
            ReadPaths.Info = log.RecordToLog;
            ReadPaths.Info += AppendFormattedText;
            ActionsWithFilesAndFolders.Info = log.RecordToLog;
            ActionsWithFilesAndFolders.Info += AppendFormattedText;
            */
            log2 = new LogToFile();
            record = log2.RecordToLog;
            record += AppendFormattedText;
            ReadPaths.Info = log2.RecordToLog;
            ReadPaths.Info += AppendFormattedText;
            ActionsWithFilesAndFolders.Info = log2.RecordToLog;
            ActionsWithFilesAndFolders.Info += AppendFormattedText;

            record?.Invoke("INFO", "Запуск программы.");

            removeList = RemoveList.GetRemoveList();             
            if (removeList != null)
            {
                record?.Invoke("INFO", $"Считано {removeList.Count} записей кофигурационного файла.");
                selectedList = new List<string>();
                ListView_folders.ItemsSource = removeList;

                /*
                foreach (var item in ListView_folders.ItemsSource)
                {
                    //if (element.NFiles==0 && element.NFolders==0 && element.SizeDir==0)
                    string str = (item as TextBlock).Text;
                    MessageBox.Show(str);

                    if ((Convert.ToString( item as TextBlock)) == "Найдено 0 файлов, 0 папок, 0 Мб") (item as CheckBox).IsEnabled = false;
                } 
                */                
            }        
        }

        private void Button_startCleaning_Click(object sender, RoutedEventArgs e)
        {                       
            foreach (var item in selectedList)
            {
                if (item == "Очистить все")
                {
                    //to do
                }
                else if (item == "Очистить корзину")
                {
                    //to do
                }
                else
                    foreach (var element in removeList)
                    {                    
                        if (element.Name == item)
                        {
                            record?.Invoke("INFO", $"Подготовлено к удалению {element.NFiles+element.NFolders} объектов");
                            element.DeleteSelected(element.Path, element.Path);                            
                        }
                    }
            }

            record?.Invoke("INFO", "Удаление завершено");                        
            selectedList = new List<string>();
            removeList.Clear();
            removeList = RemoveList.GetRemoveList(); 
        }

        private void CheckBox_select_Checked(object sender, RoutedEventArgs e)
        {                        
            if (((CheckBox)sender).IsChecked == true)
            {
                selectedList.Add((sender as CheckBox).Content.ToString());
            }            
        }
        private void CheckBox_select_Unchecked(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == false)
            {
                var element=(sender as CheckBox).Content.ToString();
                selectedList.Remove(element);
            }
        }

        private void Button_showAll_Click(object sender, RoutedEventArgs e)
        {
            RichTextBox_log.Document.Blocks.Clear();
            //RichTextBox_log.AppendText(log.ReadTheLog()+ "\r");
            RichTextBox_log.AppendText(log2.ReadTheLog() + "\r");
        }
        private void Button_clearLog_Click(object sender, RoutedEventArgs e)
        {
            //log.ClearLog();
            log2.ClearLog();
            RichTextBox_log.Document.Blocks.Clear();
        }
        
        private void AppendFormattedText(string type, string text)
        {
            TextRange rangeOfText1 = new TextRange(RichTextBox_log.Document.ContentEnd, RichTextBox_log.Document.ContentEnd);
            rangeOfText1.Text = type;
            rangeOfText1.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
            if (type=="INFO") rangeOfText1.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Blue);
            if (type == "SUCCESS") rangeOfText1.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Green);
            if (type == "WARN") rangeOfText1.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Purple);
            if (type == "ERROR") rangeOfText1.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);

            TextRange rangeOfWord = new TextRange(RichTextBox_log.Document.ContentEnd, RichTextBox_log.Document.ContentEnd);
            //rangeOfWord.Text = " " + text + "\r";
            rangeOfWord.Text = " " + DateTime.Now + " " + Environment.UserName + " " + text + "\r";
            rangeOfWord.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Regular);
            rangeOfWord.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Black);
        }
    }
}