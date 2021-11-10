using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Logger;

namespace FilesAndFolders

{
    public delegate void Message(string type, string message);
    public static class ReadPaths
    {
        public static Message Info;
        
        private const string jsonFile = "configPaths.json";

        public static Dictionary<string, string> GetDirectorySet()
        {
            string text = "";
            try
            {
                text = File.ReadAllText(jsonFile);
                Info?.Invoke("INFO", $"{jsonFile} прочитан.");
            }
            catch (FileNotFoundException)
            {                
                Info?.Invoke("ERROR", $"{jsonFile} не найден.");
                return null;
            }
            catch (IOException)
            {
                Info?.Invoke("ERROR", $"При открытии файла {jsonFile} произошла ошибка ввода-вывода.");
                return null;
            }
            catch (NotSupportedException)
            {
                Info?.Invoke("ERROR", $"Параметр {jsonFile} задан в недопустимом формате.");
                return null;
            }
            catch
            {
                Info?.Invoke("ERROR", $"Неизвестная ошибка при чтении {jsonFile}");
                return null;
            }
            try
            { 
            Dictionary<string, string> Folders = JsonSerializer.Deserialize<Dictionary<string, string>>(text);

                var userPath = $@"C:\Users\{Environment.UserName}";
                foreach (var key in Folders.Keys)
                    if (Folders[key].StartsWith("%homepath%") || Folders[key].StartsWith("%HOMEPATH%"))
                    {
                        Folders[key] = Folders[key].Replace("%homepath%", userPath);
                        Folders[key] = Folders[key].Replace("%HOMEPATH%", userPath);
                    }
                Info?.Invoke("INFO", $"Данные из {jsonFile} загружены.");
                return Folders;
            }
            catch (JsonException)
            {
                Info?.Invoke("ERROR", $"Недопустимый JSON {jsonFile}");                
                return null;
            }
            catch (NotSupportedException)
            {
                Info?.Invoke("ERROR", "Совместимые объекты JsonConverter для TValue или его сериализуемых членов отсутствуют.");
                return null;
            }
        }
    }
}