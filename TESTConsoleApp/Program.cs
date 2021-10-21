using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

using FindFolders;


namespace TESTConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, string> folders = SetFolders.GetDirectorySet();
            foreach (var f in folders) Console.WriteLine($"{f.Key} - {f.Value}");
        }
    }
}
