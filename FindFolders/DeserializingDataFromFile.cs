using System.IO;
using System.Text.Json;


namespace Find_Folders
{
    public static class DeserializingDataFromFile
    {
        public static void Deserialize(string filePathToDeserialize, out FindFolders obj)
        {
            string json = File.ReadAllText(filePathToDeserialize);
            obj = JsonSerializer.Deserialize<FindFolders>(json);
        }
    }
}
