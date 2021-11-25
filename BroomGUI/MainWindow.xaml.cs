using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

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
        LogToDB log; //для базы данных        
        Message record;

        public MainWindow()
        {
            InitializeComponent();            

            //ProgressBar_slider.Visibility = Visibility.Hidden;
            //ProgressBar_slider.IsIndeterminate = false;

            //Для журналирования в базу данных
            log = new LogToDB();
            record = log.RecordToLog;
            record += AppendFormattedText;
            ReadPaths.Info = log.RecordToLog;
            ReadPaths.Info += AppendFormattedText;
            ActionsWithFilesAndFolders.Info = log.RecordToLog;
            ActionsWithFilesAndFolders.Info += AppendFormattedText;            
            RecycleBinFolder.Info = log.RecordToLog;
            RecycleBinFolder.Info += AppendFormattedText;

            record?.Invoke("INFO", "Запуск программы.");            
            removeList = RemoveList.GetRemoveList();               
            if (removeList != null)
            {
                record?.Invoke("INFO", $"Считано {removeList.Count} записей кофигурационного файла.");
                selectedList = new List<string>();
                ListView_folders.ItemsSource = removeList;
            }
            TextBlock_sbar.Text = "Не выбраны места очистки";
        }

        private void Button_startCleaning_Click(object sender, RoutedEventArgs e)
        {
            if (selectedList.Count == 0)
            {
                TextBlock_sbar.Text = "Не выбраны места очистки";
                MessageBox.Show ("Не выбраны места очистки");
                return;
            }

            TextBlock_sbar.Text = "Удаление";
            foreach (var item in selectedList)
            {
                if (item == "Очистить все")
                {
                    foreach (var element in removeList)
                    {
                        record?.Invoke("INFO", $"Подготовлено к удалению {element.NFiles + element.NFolders} объектов");
                        element.DeleteSelected(element.Path, element.Path);                        
                    }
                    RecycleBinFolder.Delete();
                }
                else if (item == "Очистить корзину")
                {                    
                    RecycleBinFolder.Delete();
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
            TextBlock_sbar.Text = "Удаление завершено";
            record?.Invoke("INFO", "Удаление завершено");                        
            selectedList = new List<string>();
            removeList.Clear();
            removeList = RemoveList.GetRemoveList();
            CheckBox_clearAll.IsChecked = false;
            CheckBox_clearBasket.IsChecked = false;
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
            RichTextBox_log.AppendText(log.ReadTheLog()+ "\r"); 
            ProgressBar_slider.IsIndeterminate = false;
            TextBlock_sbar.Text = "Журнал удалений загружен";            
        }
        private void Button_clearLog_Click(object sender, RoutedEventArgs e)
        {
            log.ClearLog(); 
            TextBlock_sbar.Text = "Журнал удалений очищен";
        }

        private void Button_clearShowing_Click(object sender, RoutedEventArgs e)
        {
            RichTextBox_log.Document.Blocks.Clear();
            TextBlock_sbar.Text = "Показ очищен";
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
            //rangeOfWord.Text = " " + text + "\r";  //для базы данных
            rangeOfWord.Text = " " + DateTime.Now + " " + Environment.UserName + " " + text + "\r";
            rangeOfWord.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Regular);
            rangeOfWord.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Black);
        }    
    }
}