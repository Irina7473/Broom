using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;

using FilesAndFolders;


namespace TESTConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //Вариант 1
            ObservableCollection<ActionsWithFilesAndFolders> removeList = RemoveList.GetRemoveList();
            foreach (var f in removeList)
                Console.WriteLine($"{f.Name} - {f.NFiles} файлов - {f.NFolders} папок - {f.SizeDir} Мб");
            Console.WriteLine("---------------------------");

            //Вариант 1
            /*
            Dictionary<string, string> folders = ReadPaths.GetDirectorySet();
            foreach (var f in folders) Console.WriteLine($"{f.Key} - {f.Value}");
            Console.WriteLine("---------------------------");
            
            int nFiles = 0;
            int nFolders = 0;
            long sizeDir = 0;

            foreach (var f in folders)
            {
                if (PathCheck(f.Value) != null)
                {
                    nFiles = 0;
                    nFolders = 0;
                    sizeDir = 0;
                    Console.WriteLine(f.Value);
                    FillFolders(f.Value);
                    sizeDir /= 1048576;
                    Console.WriteLine($"{nFiles} - {nFolders} - {sizeDir}");
                }
                Console.WriteLine("---------------------------");
            }

            void FillFolders(string path)
            {
                if (PathCheck(path) != null)
                {
                    DirectoryInfo dir = new DirectoryInfo(path);

                    FileInfo[] files = dir.GetFiles();
                    nFiles += files.Length;
                    foreach (var file in files) sizeDir += file.Length;

                    DirectoryInfo[] folders = dir.GetDirectories();
                    nFolders += folders.Length;
                    foreach (var folder in folders) FillFolders(folder.FullName);
                }
            }

            //Проверка пути
            string PathCheck(string path)
            {
                if (Directory.Exists(path))
                {
                    try
                    {
                        Directory.GetFiles (path);
                        return path;
                    }
                    catch 
                    { 
                        Console.WriteLine($"! Нет доступа к папке {path}");
                        return null;
                    }
                }
                else 
                { 
                    Console.WriteLine($"{path} не найден");
                    return null; 
                }
            }  */
        }
    }
}
