using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace FilesAndFolders
{
    public static class RecycleBinFolder
    {
        public static Message Info;

        public static string path = "Shell:RecycleBinFolder";

            //"Shell:RecycleBinFolder";
            //Для Windows 7, 8 или для Server 2008: rd / s c:\$Recycle.Bin
            // Для Windows XP или Server 2003: rd / s c:\recycler

        public static void Delete()
        {
            int count = 0;
            if (Directory.Exists(path))
            {
                try
                {
                    string[] files = Directory.GetFiles(path);
                    foreach (var file in files)
                    {
                        try
                        {
                            File.Delete(file);
                            count++;
                            Info?.Invoke("SUCCESS", $"Удален {file}");
                        }
                        catch { Info?.Invoke("ERROR", $"Не удалось удалить {file}"); }
                    }

                    string[] folders = Directory.GetDirectories(path);
                    foreach (var folder in folders)
                    {
                        try
                        {
                            Directory.Delete(folder);
                            count++;
                            Info?.Invoke("SUCCESS", $"Удален {folder}");
                        }
                        catch { Info?.Invoke("ERROR", $"Не удален {folder}"); }
                    }
                }
                catch { Info?.Invoke("ERROR", $"Не удалось зайти в папку {path}"); }
                Info?.Invoke("SUCCESS", $"Удалено {count} объектов");
            }
            else Info?.Invoke("ERROR", $"{path} не найден");
        }
    }
}
