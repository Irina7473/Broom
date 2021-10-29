using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using Logger;

namespace FindFolders
{
    public class FindPathsFolders
    {
        public static Message Info;
        public string Name { get; set; }
        public string Path { get; set; }        
        public int NFiles { get; set; }       
        public int NFolders { get; set; }
        public double SizeDir { get; set; }
         
        public FindPathsFolders() { }
        
        public void FillFolders(string path)
        {
            if (Directory.Exists(path))
            {
                try
                {
                    DirectoryInfo dir = new DirectoryInfo(path);

                    FileInfo[] files = dir.GetFiles();
                    NFiles += files.Length;
                    foreach (var file in files) SizeDir += Convert.ToDouble(file.Length);
                   
                    DirectoryInfo[] folders = dir.GetDirectories();
                    NFolders += folders.Length;
                    foreach (var folder in folders) FillFolders(folder.FullName);
                }
                catch { Info?.Invoke($"Нет доступа к папке {path}"); }
            }
            else { Info?.Invoke($"{path} не найден"); }
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
                            Info?.Invoke($"Удален {file}");
                        }
                        catch { Info?.Invoke($"Нет доступа к {file}"); }
                    }

                    string[] folders = Directory.GetDirectories(path);
                    foreach (var folder in folders) DeleteSelected(folder, notdelpath);

                    if (path != notdelpath && files.Length == 0 && folders.Length == 0)
                    {
                        Directory.Delete(path);
                        count++;
                    }
                    
                    Info?.Invoke($"Удалено {count} объектов");
                }
                catch { Info?.Invoke($"Нет доступа к {path}"); }
            }
            else { Info?.Invoke($"{path} не найден"); }
        }   
    }
    
    public static class RemoveList
    {
        private static ObservableCollection<FindPathsFolders> RemoveCollection = new ObservableCollection<FindPathsFolders>();
        public static ObservableCollection<FindPathsFolders> GetRemoveList()
        {
            Dictionary<string, string> folders = ReadPaths.GetDirectorySet();
            if (folders != null)
                foreach (var f in folders)
                {
                    var full = new FindPathsFolders();
                    full.Name = f.Key;
                    full.Path = f.Value;
                    full.FillFolders(full.Path);
                    full.SizeDir = Math.Round(full.SizeDir / 1048576, 1);
                    RemoveCollection.Add(full);                    
                }
            else RemoveCollection = null;
            return RemoveCollection;           
        }
    }
}