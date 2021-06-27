using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Reflection;

namespace Logger
{
    public static class LogToFile
    {
        //static string fullPath = @"C:\IRINA\STEP\Broom\logBroom.txt";
        static string fullPath;
        public static void CreateToFile() // НЕ ПОЛУЧАЕТСЯ - нет доступа
        {
            //Нужно создать папку для журналов,тк их будет много
            //Исправила ошибку - работает
            /*
            var appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Console.WriteLine(appDir);
            var relativePath = "logBroom1.txt";            
            fullPath = Path.Combine(appDir, relativePath);  
            */
            //Исправила ошибку - работает
            var relativePath = "logBroom2.txt";
            //var baseFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            // или
            var baseFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            Console.WriteLine(baseFolder);
            //var appStorageFolder = Path.Combine(baseFolder, "Broom");
            fullPath = Path.Combine(baseFolder, relativePath);
            Console.WriteLine(fullPath);
            
        }

        public static void RecordEventToFile(string dataDelete, string nameFile, string locationFile, string userDelete)
        {
            var text = dataDelete + " | " + nameFile + " | " + locationFile + " | " + userDelete + " \n";
            File.AppendAllText(fullPath, text);
        }

        public static void ClearToFile()
        {
            if (File.Exists(fullPath)) File.WriteAllText(fullPath, "");
            Console.WriteLine("Журнал очищен");
        }

        public static void DeleteToFile()
        {
            if (File.Exists(fullPath)) File.Delete(fullPath);
            Console.WriteLine("Журнал удален");
        }

        public static void ReadFromFile()
        {
            StreamReader reader = new StreamReader(fullPath);
            Console.WriteLine(reader.ReadToEnd());
            reader.Close();
        }

        // асинхронное чтение слишком быстро - не успеваю записать
        /*public static async void ReadFromFile()
        {
            StreamReader reader = new StreamReader(_path);
            Console.WriteLine(await reader.ReadToEndAsync());
            reader.Close();
        }*/
    }

}
