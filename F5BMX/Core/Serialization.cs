using System.Text.Json.Serialization;

namespace F5BMX.Core;

static class Serialization
{

    public static T ReadFile<T>(string fileName) where T : class
    {
        return default(T);
    }

    public static void WriteFile<T>(string fileName, T obj) where T : class
    {
    }

}
