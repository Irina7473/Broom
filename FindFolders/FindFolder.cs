using System;
using System.IO;
using System.Collections.Generic;

namespace TestFindFolders
{
    public delegate void Message(string str);
    public class FindFolders
    {
        private static string _userName;
        private string _userPath;
        public List<string> directory { get; set; }
        private List<string> _fullDirectory;
        private List<string> _findDirectory;

        public event Message Info;
        public FindFolders()
        {
            _userName = Environment.UserName;
            _userPath = $"C:\\Users\\{_userName}\\";
            _fullDirectory = new List<string>();
            _findDirectory = new List<string>();
        }

        private void CreateFullDirectory()
        {
            try
            {
                if (directory.Count > 0)
                {
                    for (int i = 0; i < directory.Count; i++)
                    {
                        _fullDirectory.Add(_userPath + directory[i]);
                    }
                }
                else
                {
                    throw new Exception("Не загружены директории поиска папок");
                }
            }
            catch (Exception error)
            {
                Info?.Invoke(error.Message);
            }

        }

        public void SearchFolders()
        {
            try
            {
                CreateFullDirectory();
                if (_fullDirectory.Count > 0)
                {
                    for (int i = 0; i < _fullDirectory.Count; i++)
                    {
                        if (Directory.Exists(_fullDirectory[i]))
                        {
                            _findDirectory.Add(_fullDirectory[i]);
                        }
                    }
                }
                else
                {
                    throw new Exception("Отсутствуют пути для поиска папок");
                }
            }
            catch (Exception error)
            {
                Info?.Invoke(error.Message);
            }

        }

        public List<string> GetDirectoryForSearch()
        {
            return _fullDirectory;
        }

        public List<string> GetSearchFolders()
        {
            return _findDirectory;
        }
    }
}
