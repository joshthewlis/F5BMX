using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace F5BMX.Core.IO;

internal static class Directories
{

    private static string _baseDirectory { get => $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/F5BMX/"; }
    private static string seriesDirectory { get; set; } = String.Empty;

    public static string baseDirectory { get => $"{_baseDirectory}{seriesDirectory}"; }

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
        if (Directory.Exists(_baseDirectory) == false)
            Directory.CreateDirectory(_baseDirectory);

        List<string> series = new List<string>();

        foreach (var item in Directory.EnumerateDirectories(_baseDirectory))
        {
            series.Add(item.Replace(_baseDirectory, ""));
        }

        return series;
    }

}
