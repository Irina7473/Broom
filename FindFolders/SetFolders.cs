using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FindFolders
{
    public static class SetFolders
    {
        private static Dictionary<string, string> Folders { get; set; }
        private const string jsonFile = "configIrina.json";
        // public static event Message Info;

        public static Dictionary<string, string> GetDirectorySet()
        {
            Dictionary<string, string> Folders = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(jsonFile));
            return Folders;
        }
    }
}

