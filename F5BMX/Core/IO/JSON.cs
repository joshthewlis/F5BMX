using System;
using System.IO;
using System.Text.Json;

namespace F5BMX.Core.IO;

static class JSON
{

    public static T ReadFile<T>(string fileName) where T : class
    {
        var file = $"{Directories.baseDirectory}\\{fileName}.json";

        if (File.Exists(file) == false)
            return Activator.CreateInstance<T>();

        var json = JsonSerializer.Deserialize<T>(File.ReadAllText(file));

        if (json == null)
            return Activator.CreateInstance<T>();

        return json;
    }

    public static void WriteFile<T>(string fileName, T obj) where T : class
    {
        var json = JsonSerializer.Serialize<T>(obj, new JsonSerializerOptions() { WriteIndented = true });
        File.WriteAllText($"{Directories.baseDirectory}\\{fileName}.json", json);
    }

}
