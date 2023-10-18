using F5BMX.Models;
using System;

namespace F5BMX;

static class Static
{

    // BASE FOLDER
    public static readonly string baseDirectory = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\F5BMX\\";

    // SERIES
    private static Series? _series;
    public static Series? series { get => _series; set { if (_series == null) _series = value; } }

}
