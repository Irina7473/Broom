using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace FindFolders
{
    public class FindPathsFolders
    {
        public Message Info;
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
                catch
                {
                    Info?.Invoke($"Нет доступа к папке {path}");
                    // Console.WriteLine($"Нет доступа к папке {path}");
                }
            }
            else
            {
                Info?.Invoke($"{path} не найден");
                Console.WriteLine($"{path} не найден");
            }
        }
    }
    
    public static class RemoveList
    {
        private static ObservableCollection<FindPathsFolders> RemoveCollection = new ObservableCollection<FindPathsFolders>();
        public static ObservableCollection<FindPathsFolders> GetRemoveList()
        {
            var folders = ReadPaths.GetDirectorySet();
            foreach (var f in folders)
            {
                var full = new FindPathsFolders();
                full.Name = f.Key;
                full.Path = f.Value;
                full.FillFolders(full.Path);
                full.SizeDir= Math.Round(full.SizeDir/1048576, 1);
                RemoveCollection.Add(full);
            }
            return RemoveCollection;
        }
    }
}