using System.IO;

namespace F5BMX.Core.IO;

internal static class HTML
{

    public static void WriteFile(string fileName, string content)
    {
        File.WriteAllText($"{Directories.baseDirectory}\\{fileName}.html", content);
    }

}
