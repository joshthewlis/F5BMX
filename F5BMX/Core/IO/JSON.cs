using F5BMX.Interfaces;
using System;
using System.Collections;
using System.IO;
using System.Text.Json;

namespace F5BMX.Core.IO;

static class JSON
{

    public static T ReadModel<T>(string fileName) where T : IModel, new()
    {
        var file = $"{Directories.baseDirectory}\\{fileName}.json";

        if (File.Exists(file) == false)
            return new T();

        var json = JsonSerializer.Deserialize<T>(File.ReadAllText(file));

        if (json == null)
            return new T();

        return json;
    }
    public static T ReadCollection<T>(string fileName) where T : ICollection
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
        var json = JsonSerializer.Serialize(obj, obj.GetType(), new JsonSerializerOptions() { WriteIndented = true });
        File.WriteAllText($"{Directories.baseDirectory}\\{fileName}.json", json);
    }

}
