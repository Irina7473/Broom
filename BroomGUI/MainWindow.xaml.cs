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
        LogToDB log;
        Message record;

        public MainWindow()
        {
            InitializeComponent();
            log = new LogToDB();
            record = LogToDB.RecordToLog;
            //record += str => { Paragraph_log.Inlines.Add(new Run(str)};
            record?.Invoke("INFO", "Запуск программы.");            

            TextBlock_log.Text = log.ReadTheLog();
            //OutputLog1();
            Paragraph_log.Inlines.Add(log.ReadTheLog());
            //OutputLog2();

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
            
            TextBlock_log.Text = log.ReadTheLog();            
            Paragraph_log.Inlines.Clear();
            Paragraph_log.Inlines.Add(log.ReadTheLog());            
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
            TextBlock_log.Text = log.ReadTheLog();
            Paragraph_log.Inlines.Clear();
            Paragraph_log.Inlines.Add(log.ReadTheLog());
                        
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

        private void Button_clearLog_Click(object sender, RoutedEventArgs e)
        {
            log.ClearLog();
            TextBlock_log.Text = "";
            Paragraph_log.Inlines.Clear();
        }
        /*
        private void OutputLog1()
        {
            TextPointer text = TextBlock_log.ContentStart;
            string typeInfo = "INFO";
            while (true)
            {
                TextPointer next = text.GetNextContextPosition(LogicalDirection.Forward);
                if (next == null)
                {
                    break;
                }
                TextRange txt = new TextRange(text, next);

                int indx = txt.Text.IndexOf(typeInfo);
                if (indx > 0)
                {
                    TextPointer sta = text.GetPositionAtOffset(indx);
                    TextPointer end = text.GetPositionAtOffset(indx + typeInfo.Length);
                    TextRange textR = new TextRange(sta, end);
                    textR.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Yellow));
                }
                text = next;
            }
        }
                
        private void OutputLog2()
        {
            Paragraph_log.Inlines.Clear();            
            Paragraph_log.Inlines.Add(log.ReadTheLog());

            Paragraph_log.Foreground = new SolidColorBrush(Colors.MediumVioletRed);
                        
            TextPointer text = Paragraph_log.ContentStart;            
            string typeInfo = "INFO";
            while (text!=null)
            {
                TextPointer next = text.GetNextContextPosition(LogicalDirection.Forward);
                if (next == null)
                {
                    break;
                }
                TextRange txt = new TextRange(text, next);

                int indx = txt.Text.IndexOf(typeInfo);
                if (indx > 0)
                {
                    TextPointer sta = text.GetPositionAtOffset(indx);
                    TextPointer end = text.GetPositionAtOffset(indx + typeInfo.Length);
                    TextRange textR = new TextRange(sta, end);
                    textR.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Yellow));
                }
                text = next;
            }
        }

        private void SettingColorEvent ( string type)
        {            
            TextPointer text = Paragraph_log.ContentStart;
            while (true)
            {
                TextPointer next = text.GetNextContextPosition(LogicalDirection.Forward);
                if (next == null)
                {
                    break;
                }
                TextRange txt = new TextRange(text, next);

                int indx = txt.Text.IndexOf(type);
                if (indx > 0)
                {
                    TextPointer sta = text.GetPositionAtOffset(indx);
                    TextPointer end = text.GetPositionAtOffset(indx + type.Length);
                    TextRange textR = new TextRange(sta, end);
                    textR.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Yellow));
                }
                text = next;
            }

            
        }
        */
    }
}