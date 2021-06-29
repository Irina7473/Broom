using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;

namespace Logger
{
    public class LogToFile
    {
        private readonly string RegistrationPath;
        private readonly string SuccessPath;
        private readonly string ErrorsPath;
        private readonly string loggerDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BroomLogger");
        //вариант с расположением журналов в папке исполнительного файла
        // private readonly string loggerDir=Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),"BroomLogger");

        public LogToFile()
        {
            Directory.CreateDirectory(loggerDir);  //создается 1 раз при 1 запуске программы
            RegistrationPath = Path.Combine(loggerDir, "RegistrationLog.txt");  //создается 1 раз при 1 запуске программы
            var creatTime = DateTime.Now.ToShortDateString();
            var sLog = "SuccessLog" + creatTime + ".txt";
            SuccessPath = Path.Combine(loggerDir, sLog); //создается каждый раз при запуске программы
            var eLog = "ErrorsLog" + creatTime + ".txt";
            ErrorsPath = Path.Combine(loggerDir, eLog);  //создается каждый раз при запуске программы
        }
        
        public async void RecordEventToFile(string nameFile, string locationFile)
        {
            var text = DateTime.Now + " | " + nameFile + " | " + locationFile + " | " + Environment.UserName + " \n";
            await File.AppendAllTextAsync(SuccessPath, text);
        }

        public void ClearToFile()
        {
            if (File.Exists(SuccessPath)) File.WriteAllText(SuccessPath, "");
            Console.WriteLine("Журнал очищен");
        }

        public void DeleteToFile()
        {
            if (File.Exists(SuccessPath)) File.Delete(SuccessPath);
            Console.WriteLine("Журнал удален");
        }

        public void ReadFromFile()
        {
            StreamReader reader = new StreamReader(SuccessPath);
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
