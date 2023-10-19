using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace F5BMX.Core.IO;

internal static class Directories
{

    private static string seriesDirectory { get; set; }

    public static string baseDirectory { get => $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/F5BMX/{seriesDirectory}"; }

    public static void SetSeries(string seriesName)
    {
        seriesDirectory = seriesName;
    }

    public static void CreateSeriesDirectory(int year, string name)
    {   
        var dir = $"{year}-{name.Replace(" ", "_")}";

        if (Directory.Exists($"{baseDirectory}{dir}") == false)
            Directory.CreateDirectory($"{baseDirectory}{dir}");
    }

    public static List<string> LoadSeries()
    {
        if (Directory.Exists(baseDirectory) == false)
            Directory.CreateDirectory(baseDirectory);

        List<string> series = new List<string>();

        foreach (var item in Directory.EnumerateDirectories(baseDirectory))
        {
            series.Add(item.Replace(baseDirectory, ""));
        }

        return series;
    }

}
