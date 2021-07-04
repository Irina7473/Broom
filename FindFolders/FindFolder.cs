using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;

namespace FindFolders
{
    public static class FindFolders
    {
        private static string _userName;
        private static string _userPath;
        private static List<string> _folders;
        private static DirectoryList _DirectoryList { get; set; }
        private const string FILE = "config.json";
        public static event Message Info;

        private static void GetDirectoryList()
        {
            using FileStream fs = new FileStream(FILE, FileMode.Open);
            _DirectoryList = JsonSerializer.DeserializeAsync<DirectoryList>(fs).Result;
        }
        
        public static List<string> GetFolders()
        {
            _folders = new List<string>();

            _userName = Environment.UserName;
            _userPath = $@"C:\{_userName}\";

            GetDirectoryList();

            //TODO Добавить использование журналирования
            foreach (var dir in _DirectoryList.DList)
            {
                if (Directory.Exists(dir))
                {
                    _folders.Add(_userPath + dir);
                }
            }
            
            return _folders;
        }
    }
}