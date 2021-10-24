using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FindFolders
{
    public delegate void Message(string message);
    public static class ReadPaths
    {
        public static Message Info;

        //private static Dictionary<string, string> Folders { get; set; }
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
                Info?.Invoke($"{jsonFile} не найден");                
                return null;
            }
        }
    }
}