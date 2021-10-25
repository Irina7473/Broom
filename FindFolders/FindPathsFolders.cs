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
        public List<string> RemoveList { get; set; }
        //public int nFiles = 0;
        public int NFiles { get; set; }
        //public int nFolders = 0;
        public int NFolders { get; set; }
        //public long sizeDir = 0;
        public double SizeDir { get; set; }

        public FindPathsFolders() { }
        
        public List<string> FillFolders(string path, List<string> RemoveList)
        {
            if (Directory.Exists(path))
            {
                try
                {
                    DirectoryInfo dir = new DirectoryInfo(path);

                    FileInfo[] files = dir.GetFiles();
                    NFiles += files.Length;
                    foreach (var file in files)
                    {
                        SizeDir += file.Length;
                        RemoveList.Add(file.FullName);
                    }

                    DirectoryInfo[] folders = dir.GetDirectories();
                    NFolders += folders.Length;
                    foreach (var folder in folders)
                    {
                        //RemoveList.Add(folder.FullName);
                        FillFolders(folder.FullName, RemoveList);
                    }
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
            
            return RemoveList;
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
                var remove = new List<string>();
                remove = full.FillFolders(f.Value, remove);
                full.RemoveList = remove;
                full.SizeDir= Math.Round(full.SizeDir/1048576, 1);
                RemoveCollection.Add(full);
            }
            return RemoveCollection;
        }
    }
}