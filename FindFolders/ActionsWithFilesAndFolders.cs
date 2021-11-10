using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using Logger;

namespace FilesAndFolders
{
    public class ActionsWithFilesAndFolders
    {
        public static Message Info;
        public string Name { get; set; }
        public string Path { get; set; }        
        public int NFiles { get; set; }       
        public int NFolders { get; set; }
        public double SizeDir { get; set; }
        public bool FillDir { get; set; }

        public static bool CheckPaths(string path)
        {
            if (Directory.Exists(path)) return true;
            else
            {
                Info?.Invoke("WARN", $"Каталог {path} не существует");
                return false;
            }
        }

        public void CountFilesAndFolders(string path)
        {         
            if (CheckPaths(path)==true)
            {
                try
                {
                    DirectoryInfo dir = new DirectoryInfo(path);

                    FileInfo[] files = dir.GetFiles();
                    NFiles += files.Length;
                    foreach (var file in files) SizeDir += Convert.ToDouble(file.Length);
                   
                    DirectoryInfo[] folders = dir.GetDirectories();
                    NFolders += folders.Length;
                    foreach (var folder in folders) CountFilesAndFolders(folder.FullName);
                }
                catch (ArgumentException)
                { Info?.Invoke("ERROR", $"{path} содержит недопустимые символы."); }
                catch (UnauthorizedAccessException)
                { Info?.Invoke("ERROR", $"Отсутствует необходимое разрешение на доступ к {path}"); }
                catch (IOException)
                { Info?.Invoke("ERROR", $"Произошла сетевая ошибка при доступе к {path}"); }
                catch
                { Info?.Invoke("ERROR", $"Неизвестная ошибка при доступе к {path}"); }
            }
        }
              
        public void DeleteSelected(string path, string notdelpath)
        {
            int count = 0;
            if (CheckPaths(path) == true)
            {
                try
                {                    
                    string[] files = Directory.GetFiles(path);
                    foreach (var file in files)
                    {
                        try
                        {
                            File.Delete(file);   //TO DO
                            count++;                            
                            Info?.Invoke("SUCCESS", $"{file} удален.");
                        }
                        catch (ArgumentException)
                        { Info?.Invoke("ERROR", $"{file} содержит недопустимые символы."); }
                        catch (IOException)
                        { Info?.Invoke("ERROR", $"{file} используется"); }
                        catch (UnauthorizedAccessException)
                        { Info?.Invoke("ERROR", $"Отсутствует необходимое разрешение на доступ к {file}"); }
                        catch { Info?.Invoke("ERROR", $"{file} не удален"); }
                    }

                    string[] folders = Directory.GetDirectories(path);
                    foreach (var folder in folders) DeleteSelected(folder, notdelpath);

                    if (path != notdelpath && files.Length == 0 && folders.Length == 0)
                    {
                        try
                        {
                            Directory.Delete(path);   //TO DO
                            count++;
                            Info?.Invoke("SUCCESS", $"{path} удален.");
                        }
                        catch (IOException)
                        { Info?.Invoke("ERROR", $"{path} доступен только для чтения или используется другим процессом"); }
                        catch (UnauthorizedAccessException)
                        { Info?.Invoke("ERROR", $"Отсутствует необходимое разрешение на доступ к {path}"); }
                        catch (ArgumentException)
                        { Info?.Invoke("ERROR", $"{path} содержит недопустимые символы."); }
                        catch { Info?.Invoke("ERROR", $"Не удален {path}"); }
                    }                    
                }
                catch (ArgumentException)
                { Info?.Invoke("ERROR", $"{path} содержит недопустимые символы."); }
                catch (UnauthorizedAccessException)
                { Info?.Invoke("ERROR", $"Отсутствует необходимое разрешение на доступ к {path}"); }
                catch (IOException)
                { Info?.Invoke("ERROR", $"Произошла сетевая ошибка при доступе к {path}"); }
                catch
                { Info?.Invoke("ERROR", $"Неизвестная ошибка при доступе к {path}"); }               
            }
            Info?.Invoke("INFO", $"В {path} удалено {count} объектов");
        }
    }
    
    public static class RemoveList
    {
        private static ObservableCollection<ActionsWithFilesAndFolders> RemoveCollection = new ObservableCollection<ActionsWithFilesAndFolders>();
        public static ObservableCollection<ActionsWithFilesAndFolders> GetRemoveList()
        {                       
            Dictionary<string, string> folders = ReadPaths.GetDirectorySet();            

            if (folders != null)
                foreach (var f in folders)
                {        
                    if (ActionsWithFilesAndFolders.CheckPaths(f.Value) == true)
                    {
                        var filling = new ActionsWithFilesAndFolders();
                        filling.Name = f.Key;
                        filling.Path = f.Value;                        
                        filling.CountFilesAndFolders(filling.Path);
                        filling.SizeDir = Math.Round(filling.SizeDir / 1048576, 1);
                        if (filling.NFiles == 0 && filling.NFolders == 0) 
                            filling.FillDir = false; 
                        else filling.FillDir = true;
                        RemoveCollection.Add(filling);
                    }
                }
            else RemoveCollection = null;
            return RemoveCollection;           
        }
    }
}