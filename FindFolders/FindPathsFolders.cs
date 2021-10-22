using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FindFolders
{
    public class FindPathsFolders
    {
        public Message Info;

        public long nFiles = 0;
        public long nFolders = 0;
        public long sizeDir = 0;

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
            return RemoveList;
        }
    }
}