﻿using System;
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
        LogToFile log; //для текст файла
        Message record;

        public MainWindow()
        {
            InitializeComponent();            

            //ProgressBar_slider.Visibility = Visibility.Hidden;
            //ProgressBar_slider.IsIndeterminate = false;
                                    
            log = new LogToFile();
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

        private void Button_updateList_Click(object sender, RoutedEventArgs e)
        {
            selectedList = new List<string>();
            removeList.Clear();
            removeList = RemoveList.GetRemoveList();
            CheckBox_clearAll.IsChecked = false;
            CheckBox_clearBasket.IsChecked = false;
        }

        private void Button_startCleaning_Click(object sender, RoutedEventArgs e)
        {
            if (selectedList.Count == 0)
            {
                TextBlock_sbar.Text = "Не выбраны места очистки";
                MessageBox.Show("Не выбраны места очистки");
                return;
            }
            else
            {
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
                                record?.Invoke("INFO", $"Подготовлено к удалению {element.NFiles + element.NFolders} объектов");
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
        }

        private void CheckBox_select_Checked(object sender, RoutedEventArgs e)
        {
            TextBlock_sbar.Text = "Выбор мест очистки";
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
            //TextBlock_sbar.Text = "Загружаю журнал удалений";
            RichTextBox_log.Document.Blocks.Clear();
            RichTextBox_log.AppendText(log.ReadTheLog() + "\r");
            ProgressBar_slider.IsIndeterminate = false;
            TextBlock_sbar.Text = "Журнал удалений загружен";            
        }
        private void Button_clearLog_Click(object sender, RoutedEventArgs e)
        {
            //TextBlock_sbar.Text = "Очищаю журнал удалений";.
            log.ClearLog();
            TextBlock_sbar.Text = "Журнал удалений очищен";
        }

        private void Button_clearShowing_Click(object sender, RoutedEventArgs e)
        {
            //TextBlock_sbar.Text = "Очищаю показ";
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
            rangeOfWord.Text = " " + DateTime.Now + " " + Environment.UserName + " " + text + "\r";
            rangeOfWord.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Regular);
            rangeOfWord.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Black);
        }
    }
}

/*
 *  Task.Run(() =>
                            {
                                element.DeleteSelected(element.Path, element.Path);
                            });  
 * 
 * var processor = new Processor();
            processor.Progress += ProcessorProgress;
            var thread = new Thread(processor.DoWork);
            thread.Start();
            //Window_ContentRendered(sender, e);

 //Вариант1
<!-- ContentRendered="Window_ContentRendered"-->
        private void Window_ContentRendered(object sender, EventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;

            worker.RunWorkerAsync();
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                (sender as BackgroundWorker).ReportProgress(i);
                Thread.Sleep(10);
            }
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBar_status.Value = e.ProgressPercentage;
        }
                
        //Вариант 2
        public delegate void ProgressHandler(int progress);
        public class Processor
        {
            public event ProgressHandler Progress;

            public void DoWork()
            {
                for (int i = 1; i <= 100; ++i)
                {
                    Thread.Sleep(10);
                    if (Progress != null)
                        Progress(i);
                }
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            var processor = new Processor();
            processor.Progress += ProcessorProgress;

            var thread = new Thread(processor.DoWork);
            thread.Start();
        }

        void ProcessorProgress(int progress)
        {            
            if (!ProgressBar_status.Dispatcher.CheckAccess())
            {
                ProgressBar_status.Dispatcher.Invoke(new ProgressHandler(ProcessorProgress), progress);
            }
            else
            {
                ProgressBar_status.Value = progress;
            }
        }
  */