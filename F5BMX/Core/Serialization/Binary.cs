using F5BMX.Interfaces;

namespace F5BMX.Core.Serialization;

static class Binary
{

    public static T Read<T>(string fileName) where T : IRound
    {
        return default(T);
    }

    public static void Write<T>(string fileName, T obj) where T : IRound
    {

    }

}
