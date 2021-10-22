using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

using FindFolders;


namespace TESTConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            string P = @"C:\Users\IrinaS\AppData\Local\Temp";
            Console.WriteLine(P);
            DateTime dt = Directory.GetCreationTime(P);
            Console.WriteLine(dt);
            Console.WriteLine("---------------------------");
            */

            Dictionary<string, string> folders = ReadPaths.GetDirectorySet();
            foreach (var f in folders) Console.WriteLine($"{f.Key} - {f.Value}");
            Console.WriteLine("---------------------------");

            foreach (var f in folders)
            {                
                    Console.WriteLine($"{f.Key} - {f.Value}");
                    var full= new FindPathsFolders();
                    var remove = new List<string>();
                    remove = full.FillFolders(f.Value, remove);
                Console.WriteLine($"{full.nFiles} - {full.nFolders} - {full.sizeDir/1048576}");
                    //Console.WriteLine(remove.Count);               
                Console.WriteLine("---------------------------");
            }
            Console.WriteLine("---------------------------");
            

            /*
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
