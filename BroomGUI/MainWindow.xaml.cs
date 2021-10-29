﻿using System;
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
                            //FindPathsFolders.Info =msg => log.RecordToLog("INFO", msg);
                        }
                    }
            }            
            TextBlock_log.Text = log.ReadTheLog();
            //очистить чекбоксы !!            
            selectedList = new List<string>();            
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
    }
}