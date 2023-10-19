using System.IO;
using System.Text.Json;

namespace F5BMX.Core.IO;

static class JSON
{

    public static T? ReadFile<T>(string fileName) where T : class
    {
        return JsonSerializer.Deserialize<T>(File.ReadAllText($"{Directories.baseDirectory}\\{fileName}.json"));
    }

    public static void WriteFile<T>(string fileName, T obj) where T : class
    {
        var json = JsonSerializer.Serialize<T>(obj);
        File.WriteAllText($"{Directories.baseDirectory}\\{fileName}.json", json);
    }

}
