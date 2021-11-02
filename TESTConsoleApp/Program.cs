using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;

using FilesAndFolders;
using Logger;


namespace TESTConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var log = new LogToFile();
            log.ClearLog();
            Message record;
            record = log.RecordToLog;
            ReadPaths.Info = log.RecordToLog;
            ActionsWithFilesAndFolders.Info = log.RecordToLog;

            record?.Invoke("INFO", "1");
            Console.WriteLine(log.ReadTheLog());
            Console.WriteLine("---------------------------");            

            ObservableCollection<ActionsWithFilesAndFolders> removeList = RemoveList.GetRemoveList();
            foreach (var f in removeList)
                Console.WriteLine($"{f.Name} - {f.NFiles} файлов - {f.NFolders} папок - {f.SizeDir} Мб");
            Console.WriteLine("---------------------------");
            Console.WriteLine(log.ReadTheLog());
            Console.WriteLine("---------------------------");


            /*Проверка пути
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
