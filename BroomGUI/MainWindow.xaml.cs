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
using Logger;

namespace BroomGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<FindPathsFolders> removeList;
        List<string> selectedList;
        LogToDB log;

        public MainWindow()
        {
            InitializeComponent();
            log = new LogToDB();
            log.RecordToLog("INFO", "Запуск программы.");
            TextBlock_log.Text = log.ReadTheLog();
            //OutputLog1();
            //Paragraph_log.Inlines.Add(log.ReadTheLog());
            OutputLog2();

            removeList = RemoveList.GetRemoveList();
            if (removeList != null)
            {
                log.RecordToLog("INFO", $"Считано {removeList.Count} записей кофигурационного файла.");
                ListView_folders.ItemsSource = removeList;
                selectedList = new List<string>();
            }
            else
            {
                log.RecordToLog("ERROR", ReadPaths.Info.ToString());
                log.RecordToLog("ERROR", FindPathsFolders.Info.ToString());
            }
            TextBlock_log.Text = log.ReadTheLog();
            /*
            Paragraph_log.Inlines.Clear();
            Paragraph_log.Inlines.Add(log.ReadTheLog());*/
            OutputLog2();
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
                            MessageBox.Show(element.Path);
                            log.RecordToLog("INFO", $"Подготовлено к удалению {element.NFiles+element.NFolders} объектов");
                            DeleteSelected(element.Path, element.Path);      
                        }
                    }
            }
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

        private void Button_clearLog_Click(object sender, RoutedEventArgs e)
        {
            log.ClearLog();
            TextBlock_log.Text = "";
            Paragraph_log.Inlines.Clear();
        }

        public void DeleteSelected(string path, string notdelpath)
        {
            if (Directory.Exists(path))
            {
                try
                {
                    int count = 0;
                    string[] files = Directory.GetFiles(path);
                    foreach (var file in files)
                    {
                        try
                        {
                            File.Delete(file);
                            count++;
                            log.RecordToLog("SUCCESS", $"Удален {file}");
                        }
                        catch { log.RecordToLog("ERROR", $"Нет доступа к {file}"); }
                    }

                    string[] folders = Directory.GetDirectories(path);
                    foreach (var folder in folders) DeleteSelected(folder, notdelpath);
                                        
                    if (path!= notdelpath && files.Length == 0 && folders.Length == 0)
                    {
                        Directory.Delete(path);
                        count++;
                    }
                    log.RecordToLog("SUCCESS", $"Удалено {count} объектов");
                }
                catch { log.RecordToLog("WARN", $"Нет доступа к {path}"); }
            }
            else { log.RecordToLog("ERROR", $"{path} не найден"); }
        }

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

    }
}