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
        private readonly string PathLoggerDir=Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),"Log");
        //в разработке вариант с config файлом

        public LogToFile()
        {
            //создается 1 раз при 1 запуске программы
            Directory.CreateDirectory(PathLoggerDir);
            //создается 1 раз при 1 запуске программы
            TotalPath = Path.Combine(PathLoggerDir, "TotalLog.log");
            //создаются по 1 на каждую текущую дату
            var creatTime = DateTime.Now.ToShortDateString();
            var sLog = "SuccessLog" + "_" + creatTime + ".log";
            SuccessPath = Path.Combine(PathLoggerDir, sLog); 
            var eLog = "ErrorsLog" + "_" + creatTime + ".log";
            ErrorsPath = Path.Combine(PathLoggerDir, eLog);  
        }

        public LogToFile(string path)
        {
            PathLoggerDir = path;
            var creatTime = DateTime.Now.ToShortDateString();
            try
            {
                //создается 1 раз при 1 запуске программы
                TotalPath = Path.Combine(PathLoggerDir, "TotalLog.log");
                //создаются по 1 на каждую текущую дату      
                var sLog = "SuccessLog" + "_" + creatTime + ".log";
                SuccessPath = Path.Combine(PathLoggerDir, sLog);
                var eLog = "ErrorsLog" + "_" + creatTime + ".log";
                ErrorsPath = Path.Combine(PathLoggerDir, eLog);
            }
            catch
            {
                throw new Exception("Путь к месту записи файлов не найден");
            }            
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

        public void ReadTheLog() 
        {
            Reader(TotalPath);
            Reader(SuccessPath);
            Reader(ErrorsPath);            
        }

        private async void Reader(string path)
        {
            if (File.Exists(path))
            {
                StreamReader reader = new(path);
                Console.WriteLine(await reader.ReadToEndAsync());
                reader.Close();
            }

        }

        public void ClearLogr()
        {
            if (Directory.Exists(PathLoggerDir))
            {
                File.WriteAllText(TotalPath, null);
                var directoryInfo = new DirectoryInfo(PathLoggerDir);                 
                foreach (var file in directoryInfo.GetFiles()) 
                        if (file.Name != "TotalLog.log") file.Delete(); 
            }
        }
    }
}