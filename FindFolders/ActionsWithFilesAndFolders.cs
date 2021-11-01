using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using Logger;

namespace FilesAndFolders
{
    public class ActionsWithFilesAndFolders
    {
        public static Message Info;
        public string Name { get; set; }
        public string Path { get; set; }        
        public int NFiles { get; set; }       
        public int NFolders { get; set; }
        public double SizeDir { get; set; }

        public static bool CheckPaths(string path)
        {
            if (Directory.Exists(path)) return true;
            else
            {
                Info?.Invoke("WARN", $"{path} не найден");
                return false;
            }
        }

        public void CountFilesAndFolders(string path)
        {         
            if (CheckPaths(path)==true)
            {
                try
                {
                    DirectoryInfo dir = new DirectoryInfo(path);

                    FileInfo[] files = dir.GetFiles();
                    NFiles += files.Length;
                    foreach (var file in files) SizeDir += Convert.ToDouble(file.Length);
                   
                    DirectoryInfo[] folders = dir.GetDirectories();
                    NFolders += folders.Length;
                    foreach (var folder in folders) CountFilesAndFolders(folder.FullName);
                }
                catch { Info?.Invoke("ERROR", $"Не удалось зайти в папку {path}"); } //TO DO сделать обработку всех исключений
            }
            //else { Info?.Invoke("WARN", $"{path} не найден"); }
        }
              
        public void DeleteSelected(string path, string notdelpath)
        {
            if (CheckPaths(path) == true)
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
                            Info?.Invoke("SUCCESS", $"Удален {file}");
                        }
                        catch { Info?.Invoke("ERROR", $"Не удалось удалить {file}"); }
                    }

                    string[] folders = Directory.GetDirectories(path);
                    foreach (var folder in folders) DeleteSelected(folder, notdelpath);

                    if (path != notdelpath && files.Length == 0 && folders.Length == 0)
                    {
                        Directory.Delete(path);
                        count++;
                    }
                    
                    Info?.Invoke("SUCCESS", $"Удалено {count} объектов");
                }
                catch { Info?.Invoke("ERROR", $"Не удалось зайти в папку {path}"); } //TO DO сделать обработку всех исключений
            }
            //else { Info?.Invoke("WARN", $"{path} не найден"); }
        }
    }
    
    public static class RemoveList
    {
        private static ObservableCollection<ActionsWithFilesAndFolders> RemoveCollection = new ObservableCollection<ActionsWithFilesAndFolders>();
        public static ObservableCollection<ActionsWithFilesAndFolders> GetRemoveList()
        {
            //LogToDB log1 = new LogToDB();
            //ReadPaths.Info = log1.RecordToLog;

            Dictionary<string, string> folders = ReadPaths.GetDirectorySet();            

            if (folders != null)
                foreach (var f in folders)
                {        
                    if (ActionsWithFilesAndFolders.CheckPaths(f.Value) == true)
                    {
                        var filling = new ActionsWithFilesAndFolders();
                        filling.Name = f.Key;
                        filling.Path = f.Value;
                        //ActionsWithFilesAndFolders.Info = log1.RecordToLog;
                        filling.CountFilesAndFolders(filling.Path);
                        filling.SizeDir = Math.Round(filling.SizeDir / 1048576, 1);
                        RemoveCollection.Add(filling);
                    }
                }
            else RemoveCollection = null;
            return RemoveCollection;           
        }
    }
}