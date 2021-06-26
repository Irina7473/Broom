using System;
using System.IO;
using System.Collections.Generic;

namespace Find_Folders
{
    public delegate void Message(string str);
    public class FindFolders
    {
        private static string _userName = Environment.UserName;
        private string _userPath = $"C:\\Users\\{_userName}\\";
        //для десериализации данных (части пути)
        public List<string> directory { get; set; }
        //после конкатенации строк будет записан полный пусть
        //private string[] _fullDirectory;
        private List<string> _fullDirectory;
        //после поиска будут записаны пути которые были найдены на устройстве
        private List<string> _findDirectory;

        //Эвент для логирования
        public event Message Info;

        //Метод создания полного пути
        private void CreateFullDirectory()
        {
            try
            {
                _fullDirectory = new List<string>(directory.Count);
                for (int i = 0; i < directory.Count; i++)
                {
                    _fullDirectory.Add(_userPath + directory[i]);
                }
            }
            catch (Exception NullReferenceException)
            {
                Info?.Invoke(NullReferenceException.Message);
            }

        }
        //Основной метод поиска папок
        public void SearchFolders()
        {
            try
            {
                CreateFullDirectory();
                _findDirectory = new List<string>(_fullDirectory.Count);
                for (int i = 0; i < _fullDirectory.Count; i++)
                {
                    if (Directory.Exists(_fullDirectory[i]))
                    {
                        _findDirectory.Add(_fullDirectory[i]);
                    }
                }
            }
            catch (Exception NullReferenceException)
            {
                Info?.Invoke(NullReferenceException.Message);
            }

        }

        //временный метод для тестирования
        //в проде необходимо убрать
        public void ShowDirectoryForSearch()
        {
            if (_fullDirectory != null)
            {
                foreach (var item in _fullDirectory)
                {
                    Console.WriteLine(item);
                }
            }
            else
            {
                Console.WriteLine("Директории не были созданы");
            }
        }
        //Получение директорий для поиска
        public List<string> GetDirectoryForSearch()
        {
            return _fullDirectory;
        }

        //временный метод для тестирования
        //в проде необходимо убрать
        public void ShowSearchFolders()
        {
            foreach (var item in _findDirectory)
            {
                Console.WriteLine(item);
            }
        }
        //Получение найденных директорий на устройстве пользователя
        public List<string> GetSearchFolders()
        {
            return _findDirectory;
        }
    }
}
