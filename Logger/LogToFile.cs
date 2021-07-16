using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

namespace Logger
{
    public class LogToFile: ILogger
    {
        private readonly string TotalPath;
        private readonly string SuccessPath;
        private readonly string ErrorsPath;
        private readonly string loggerDir=Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),"BroomLogger");
        //в разработке вариант с config файлом

        public LogToFile()
        {
            //создается 1 раз при 1 запуске программы
            Directory.CreateDirectory(loggerDir);
            //создается 1 раз при 1 запуске программы
            TotalPath = Path.Combine(loggerDir, "TotalLog.log");

            //создаются по 1 на каждую текущую дату
            var creatTime = DateTime.Now.ToShortDateString();
            var sLog = "SuccessLog" + "_" + creatTime + ".log";
            SuccessPath = Path.Combine(loggerDir, sLog); 
            var eLog = "ErrorsLog" + "_" + creatTime + ".log";
            ErrorsPath = Path.Combine(loggerDir, eLog);  
        }        

        public async void RecordToLog(string typeevent, string message) 
        {
            var text = typeevent + " | " + DateTime.Now + " | " + Environment.UserName + " | " + message + " \n";
            Console.WriteLine(text);
            switch (typeevent)
            {
                case "INFO":
                case "WARN":
                    await File.AppendAllTextAsync(TotalPath, text);
                    break;
                case "SUCCESS":
                    await File.AppendAllTextAsync(SuccessPath, text);
                    break;
                case "ERROR":
                    await File.AppendAllTextAsync(ErrorsPath, text);
                    break;
            }
        }

        public async void ReadTheLog() 
        {
            if (File.Exists(TotalPath))
            {
                StreamReader readerTotal = new(TotalPath);
                Console.WriteLine(await readerTotal.ReadToEndAsync());
                readerTotal.Close();
            }

            if (File.Exists(SuccessPath))
            {
                StreamReader readerSuccess = new(SuccessPath);
                Console.WriteLine(await readerSuccess.ReadToEndAsync());
                readerSuccess.Close();
            }

            if (File.Exists(ErrorsPath))
            {
                StreamReader readerErrors = new(ErrorsPath);
                Console.WriteLine(await readerErrors.ReadToEndAsync());
                readerErrors.Close();
            }
        }

        public void ClearLogr()
        {
            if (Directory.Exists(loggerDir))
            {
                File.WriteAllText(TotalPath, null);
                var directoryInfo = new DirectoryInfo(loggerDir);                 
                foreach (var file in directoryInfo.GetFiles()) 
                {
                    if (file.Name != "TotalLog.log") file.Delete();
                        
                }
            }
        }
        
        /*
        public async void RecordEventToFile(string type, string message)
        {
            var text = type + " | " + DateTime.Now + " | " + Environment.UserName + " | " +  message + " \n";
            switch (type)
            {
                case "INFO":                     
                case "WARN":
                    await File.AppendAllTextAsync(TotalPath, text);
                    break;
                case "SUCCESS":
                    await File.AppendAllTextAsync(SuccessPath, text);
                    break;
                case "ERROR":
                    await File.AppendAllTextAsync(ErrorsPath, text);
                    break;
            }           
        }

        //Выводит данные таблицы в консоль на этапе отладки
        public async void ReadFromFile()
        {
            StreamReader readerTotal = new StreamReader(TotalPath);
            Console.WriteLine(await readerTotal.ReadToEndAsync());
            readerTotal.Close();
            
            StreamReader readerSuccess = new StreamReader(SuccessPath);
            Console.WriteLine(await readerSuccess.ReadToEndAsync());
            readerSuccess.Close();

            StreamReader readerErrors = new StreamReader(ErrorsPath);
            Console.WriteLine(await readerErrors.ReadToEndAsync());
            readerErrors.Close();
        }*/
    }
}