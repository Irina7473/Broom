using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FindFolders
{
    public static class ReadPaths
    {
        private static Dictionary<string, string> Folders { get; set; }
        //private static List<string> RemoveList { get; set; }
        private const string jsonFile = "configIrina.json";        
        // public static event Message Info;

        public static Dictionary<string, string> GetDirectorySet()
        {
            try
            {
                Dictionary<string, string> Folders = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(jsonFile));
                var userPath = $@"C:\Users\{Environment.UserName}";
                foreach (var key in Folders.Keys)
                {
                    if (Folders[key].StartsWith("%homepath%") || Folders[key].StartsWith("%HOMEPATH%"))
                    {
                        Folders[key] = Folders[key].Replace("%homepath%", userPath);
                        Folders[key] = Folders[key].Replace("%HOMEPATH%", userPath);
                    }
                }
                return Folders;
            }
            catch
            {
                Console.WriteLine($"{jsonFile} не найден");
                return null;
            }
        }



        public static List<string> FillFolders(string path, List<string> RemoveList)
        {
            if (Directory.Exists(path))
            {
                try
                {
                    DirectoryInfo dir = new DirectoryInfo(path);

                    FileInfo[] files = dir.GetFiles();
                    //nFiles += files.Length;
                    foreach (var file in files)
                    {
                        //sizeDir += file.Length;
                        RemoveList.Add(file.FullName);
                    }

                    DirectoryInfo[] folders = dir.GetDirectories();
                    //nFolders += folders.Length;
                    foreach (var folder in folders)
                    {
                        RemoveList.Add(folder.FullName);
                        FillFolders(folder.FullName, RemoveList);
                    }
                }
                catch { Console.WriteLine($"Нет доступа к папке {path}"); }
            }
            else Console.WriteLine($"{path} не найден");
            return RemoveList;
        }



        //Проверка пути
        private static string PathCheck(string path)
        {
            if (!Directory.Exists(path))
            {
                return null;
            }
            return path;
        }
    }
}