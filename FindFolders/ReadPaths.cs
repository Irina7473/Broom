using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Logger;

namespace FilesAndFolders

{
    public delegate string Message(string type, string message);
    public static class ReadPaths
    {
        public static Message Info;        
        private const string jsonFile = "configPaths.json";

        public static Dictionary<string, string> GetDirectorySet()
        {
            try
            {
                Dictionary<string, string> Folders = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(jsonFile));

                var userPath = $@"C:\Users\{Environment.UserName}";
                foreach (var key in Folders.Keys)
                    if (Folders[key].StartsWith("%homepath%") || Folders[key].StartsWith("%HOMEPATH%"))
                    {
                        Folders[key] = Folders[key].Replace("%homepath%", userPath);
                        Folders[key] = Folders[key].Replace("%HOMEPATH%", userPath);
                    }
                Info?.Invoke("INFO", $"{jsonFile} прочитан");
                return Folders;
            }
            catch
            {
                Info?.Invoke("ERROR", $"{jsonFile} не найден");                
                return null;
            }
        }
    }
}