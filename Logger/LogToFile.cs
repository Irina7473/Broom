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
        //в разработке вариант с config файлом

        public LogToFile()
        {
            //создается 1 раз при 1 запуске программы
            Directory.CreateDirectory(loggerDir);
            //создается 1 раз при 1 запуске программы
            RegistrationPath = Path.Combine(loggerDir, "RegistrationLog.txt"); 
            
            //текущая дата без времени
            var creatTime = DateTime.Now.ToShortDateString();
            //проверка количества файлов созданных в текущую дату
            string[] filesSuccess = Directory.GetFiles(loggerDir, $"SuccessLog {creatTime}*");
            string[] filesErrors = Directory.GetFiles(loggerDir, $"ErrorsLog {creatTime}*");
            //номер следующего на текущую дату файла
            var numberSuccess = filesSuccess.Length + 1;
            var numberErrors = filesErrors.Length + 1;

            //создаются каждый раз при запуске программы со следующим в текущую дату номером
            var sLog = "SuccessLog" + " " + creatTime + " " + numberSuccess + ".txt";
            SuccessPath = Path.Combine(loggerDir, sLog); 
            var eLog = "ErrorsLog" + " " + creatTime + " " + numberErrors + ".txt";
            ErrorsPath = Path.Combine(loggerDir, eLog);  
        }

        public async void RecordEventToFile(string type, string message)
        {
            var text = DateTime.Now + " | " + type + " | " + message + " | " + Environment.UserName + " \n";
            await File.AppendAllTextAsync(RegistrationPath, text);
            if (type == "SUCCESS") this.RecordSuccessToFile(message);
            if (type == "ERROR") this.RecordErrorsToFile(message);
        }
            
        public async void RecordSuccessToFile(string message)
        {
            var text = DateTime.Now + " | " + message + " \n";
            await File.AppendAllTextAsync(SuccessPath, text);
        }

        public async void RecordErrorsToFile(string message)
        {
            var text = DateTime.Now + " | " + message + " \n";
            await File.AppendAllTextAsync(ErrorsPath, text);
        }
                        
        public async void ReadFromFile()
        {
            StreamReader reader = new StreamReader(RegistrationPath);
            Console.WriteLine(await reader.ReadToEndAsync());
            reader.Close();
        }
    }

}
