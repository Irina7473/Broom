using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace TestFindFolders
{
    public static class DeserializingDataFromFile
    {
        public static async Task<FindFolders> DeserializeAsync(string filePathToDeserialize, FindFolders obj)
        {
            using (FileStream fs = new FileStream(filePathToDeserialize, FileMode.Open))
            {
                obj = await JsonSerializer.DeserializeAsync<FindFolders>(fs);
            }
            return obj;
        }
    }
}