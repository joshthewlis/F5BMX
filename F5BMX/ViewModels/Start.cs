using F5BMX.Core;
using F5BMX.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace F5BMX.ViewModels;

internal class Start : NotifyBase
{

    public Start()
    {
        if(Directory.Exists(Static.baseDirectory) == false)
            Directory.CreateDirectory(Static.baseDirectory);

        foreach (var item in Directory.EnumerateDirectories(Static.baseDirectory))
        {
            series.Add(
                new Series(item.Replace(Static.baseDirectory, ""))
            );
        };
    }

    public ObservableCollection<Series> series { get; init; } = new ObservableCollection<Series>();

    public Series? selectedSeries { get; set; }

}
