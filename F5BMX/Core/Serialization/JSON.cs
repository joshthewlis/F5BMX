using System.Text.Json.Serialization;

namespace F5BMX.Core.Serialization;

static class JSON
{

    public static T ReadFile<T>(string fileName) where T : class
    {
        return default;
    }

    public static void WriteFile<T>(string fileName, T obj) where T : class
    {
    }

}
