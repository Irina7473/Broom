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
        public string name { get; set; }
        public string path { get; set; }
        public List<string> RemoveList { get; set; }
        public long nFiles { get; set; }
        public long nFolders { get; set; }
        public long sizeDir { get; set; }

        public FindPathsFolders() { }
        
        public List<string> FillFolders(string path, List<string> RemoveList)
        {
            if (Directory.Exists(path))
            {
                try
                {
                    DirectoryInfo dir = new DirectoryInfo(path);

                    FileInfo[] files = dir.GetFiles();
                    nFiles += files.Length;
                    foreach (var file in files)
                    {
                        sizeDir += file.Length;
                        RemoveList.Add(file.FullName);
                    }

                    DirectoryInfo[] folders = dir.GetDirectories();
                    nFolders += folders.Length;
                    foreach (var folder in folders)
                    {
                        RemoveList.Add(folder.FullName);
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
            sizeDir /= 1048576;
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
                full.name = f.Key;
                full.path = f.Value;
                var remove = new List<string>();
                remove = full.FillFolders(f.Value, remove);
                full.RemoveList = remove;
                RemoveCollection.Add(full);
            }
            return RemoveCollection;
        }
    }
}